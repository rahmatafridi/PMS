
<style>
  card .card-title, .card .stats, .card .category, .card .description, .card .social-line, .card .actions, .card .card-content, .card .card-footer, .card small, .card a {
    position: initial !important;
  }
</style>
<template>
  <div class="row">



    <div class="col-md-12">
      <div class="card">
        <div class="card-header">
          <div class="row">
            <div class="col-lg-6">
              <label class="title headersize">Document Expiry Report</label>
            </div>
            <div class="col-lg-3">

            </div>
            <div class="col-lg-3">
              <div>
                <vue-excel-xlsx :data="list.items"
                                :columns="columns"
                                :filename="'Empty Room Report'"
                                class="fa fa-file-excel-o exceldowload"
                                :sheetname="'sheetname'">
                </vue-excel-xlsx>
                <button style="float:right;margin-right:5px;" v-on:click="generate"><span class="fa fa-file-pdf-o" title="Pdf"></span></button>

              </div>


              <!--<div>-->
              <!--<el-select class="select-default"
                       size="large"
                       style="float:right;"
                       placeholder="Single Select"
                       >
              <el-option :value="1" label="PDF" @input="generatePDF">
              </el-option>
            </el-select>-->
              <!--<select class="form-control" @change="generate($event)">
                <option value="0">select</option>
                <option value="1">PDF</option>
                <option value="2">

                </option>

              </select>
            </div>-->
              <!--<button class="btn btn-primary btn-small" style="float:right" v-on:click="addNew">New Proivder</button>-->
            </div>
          </div>
          <hr />
        </div>
        <div class="card-content row">
          <div class="col-sm-3">
            <label>Expiring within</label>
            <select class="form-control" v-model="days" v-on:change="dayChange($event)">
              <option :value="0">All</option>
              <option :value="90">90</option>
              <option :value="28">28</option>
              <option :value="14">14</option>
              <option :value="-1">Expiry</option>

            </select>

          </div>
          <div class="col-sm-3">
            <div>
              <label>Docoument Type</label>
              <select class="form-control" v-model="docTypeId" v-on:change="docChange($event)" style="float:right">
                <option :value="0">All</option>
                <option v-for="item in documentList" :value="item.id">
                  {{item.title}}
                </option>
              </select>
            </div>

            <!--<el-select class="select-default"
                     v-model="pagination.perPage"
                     style="float:right"
                     placeholder="Per page">
            <el-option class="select-default"
                       v-for="item in pagination.perPageOptions"
                       :key="item"
                       :label="item"
                       :value="item">
            </el-option>
          </el-select>-->
          </div>
          <div class="col-lg-6">

          </div>
          <div class="card-content table-responsive table-full-width table">
            <el-table :data="queriedData">


              <el-table-column label="Document" property="document"></el-table-column>
              <el-table-column label="Expiry" property="expiry"></el-table-column>
              <el-table-column label="Days" property="days"></el-table-column>
              <el-table-column label="Address" property="address"></el-table-column>

              <el-table-column :min-width="40"
                               fixed="right"
                               label="Action">
                <template slot-scope="props">
                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleView(props.$index, props.row)"><i class="ti-eye"></i></a>

                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleDownload(props.$index, props.row)"><i class="ti-cloud-down"></i></a>
                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-pencil-alt"></i></a>
                </template>
              </el-table-column>
            </el-table>

          </div>
          <div class="col-sm-6 pagination-info paggingcss">
            <p class="category">Showing {{from + 1}} to {{to}} of {{total}} entries</p>
          </div>
          <div class="col-sm-6">
            <p-pagination class="pull-right"
                          v-model="pagination.currentPage"
                          :per-page="pagination.perPage"
                          :total="pagination.total">
            </p-pagination>
          </div>
        </div>
      </div>
    </div>
    <Modal v-model="showModalPreview" title="Document Preview">
      <div class="row">
        <!--<div id="pdfPreview" class="modal-body" style="width:100%!important">
      </div>-->
        <iframe :src="ruta" style="width:100%!important;height:500px;"></iframe>

      </div>
    </Modal>
  </div>
</template>
<script>
  import Vue from 'vue'
  import jsPDF from 'jspdf'
  import 'jspdf-autotable'
  import VueModal from '@kouts/vue-modal'
  import '@kouts/vue-modal/dist/vue-modal.css'
  import XLSX from 'xlsx';
  import VueExcelXlsx from "vue-excel-xlsx";
  Vue.use(VueExcelXlsx);
  import swal from 'sweetalert2'
  import { getComplianceList } from '@/helpers/compliance'

  import { Table, TableColumn, Select, Option } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  import { mapState } from 'vuex'
  import { getCompliancePropDocById } from '@/helpers/compliancedocs'


  export default {
    components: {
      PPagination,
      'Modal': VueModal
    },
    data() {
      return {
        ruta: '',
        tableData: [],
        options: {},
        columns: [
          {
            label: "Document",
            field: "document",
          },
          {
            label: "Expiry",
            field: "expiry",
          },
          {
            label: "Days",
            field: "days",
          },
          {
            label: "Address",
            field: "address",

          }

        ],
        showModalPreview: false,
        client: null,
        errorMessage: null,
        pagination: {
          perPage: 5,
          currentPage: 1,
          perPageOptions: [5, 10, 25, 50],
          total: 0
        },
        searchQuery: '',
        propsToSearch: ['titleNo', 'numberOfRooms', 'address1'],
        documentList: [],
        selectedId: '',
        filename: '',
        autoWidth: true,
        bookType: 'xlsx',
        docTypeId: 0,
        days:0
      }
    },
    methods: {
      getCompliancePropDocById: getCompliancePropDocById,
      getComplianceList: getComplianceList,
      tableRowClassName(row, index) {
        if (index === 0) {
          return 'success'
        } else if (index === 2) {
          return 'info'
        } else if (index === 4) {
          return 'danger'
        } else if (index === 6) {
          return 'warning'
        }
        return ''
      },
      fetchList(days,typeId) {

        this.$store.dispatch('expirydocreport/fetchList', {
          auth: {
            days: days,
            typeId: typeId
          }
        }).catch(e => {
          this.errorMessage = e
          swal({
            title: 'Warning',
            text: this.errorMessage,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })        })
      },
      handleEdit(index, row) {
        this.$router.push(`/property/edit/${row.id}`)

      },
      docChange(event) {
        var typeId = event.target.value;
        var days = this.days;
        this.fetchList(days, typeId);
      },
      dayChange(event) {
        var days = event.target.value;
        var typeId = this.typeId;
        this.fetchList(days, typeId);
      },
      generate() {

        //var value = event.target.value;
        /*      if (value == 1) {*/
        const columns = [
          { title: "Document", dataKey: "document" },
          { title: "Expiry", dataKey: "expiry" },
          { title: "Days", dataKey: "days" },
          { title: "Address", dataKey: "address" }
        ];
        var doc = new jsPDF("l", "pt", "a4");
        // text is placed using x, y coordinates
        //doc.setFontSize(16).text(this.heading, 0.5, 1.0);
        // create a line under heading
        //doc.setLineWidth(0.01).line(0.5, 1.1, 8.0, 1.1);
        // Using autoTable plugin
        var y = 1;

        doc.autoTable({
          columns,
          body: this.list.items,
          margin: { right: 3, left: 0.25, top: 30 },
          styles: {
            overflow: 'linebreak', columnWidth: 'wrap', font: 'arial',
            cellPadding: 8
          }
        });
        doc.text(400, y = y + 25, "Expiry Documnet Report");

        // Using array of sentences
        //doc
        //  .setFont("helvetica")
        //  .setFontSize(12)
        //  .text(this.moreText, 0.5, 3.5, { align: "left", maxWidth: "7.5" });

        // Creating footer and saving file
        doc.save(`ExpiryDocReport.pdf`);
  
      },
      dateFormate(value) {
        return moment(String(value)).format('DD/MM/YYYY')

      },
      formatJson(filterVal, jsonData) {
        return jsonData.map(v => filterVal.map(j => {
          if (j === 'timestamp') {
            return parseTime(v[j])
          } else {
            return v[j]
          }
        }))
      },
      handleDownload(index, row) {
        debugger
        if (row.propDocId != null) {

          this.getCompliancePropDocById(row.propDocId).then(res => {
            const fileName = "";
            console.log(res);
            const linkSource = res.docObject;
            const downloadLink = document.createElement("a");
            if (res.extension == "jpeg") {
              this.fileName = res.name + ".jpeg";

            }
            if (res.extension == "pdf") {
              this.fileName = res.name + ".pdf";

            }
            if (res.extension == "png") {
              this.fileName = res.name + ".png";

            }
            if (res.extension == "txt") {
              this.fileName = res.name + ".txt";

            }
            //if (res.extension == "xscl") {
            //  this.fileName = res.name + ".txt";

            //}
            downloadLink.href = linkSource;
            downloadLink.download = fileName;
            downloadLink.click();


          })
        }

      },
      handleView(index, row) {
        debugger
        if (row.propDocId != null) {

          this.getCompliancePropDocById(row.propDocId).then(res => {
            const fileName = "";
            console.log(res);
            const linkSource = res.docObject;
            if (res.docObject != "") {
              this.ruta = res.docObject;
              //$("#pdfPreview").empty();
              //$("#pdfPreview").append('<embed style="width:700px;height:100%;" type="application/pdf" src="' + res.docObject + '" />');
              this.showModalPreview = true;

            }

          })
        }

      },

    },
    computed: {
      ...mapState({
        list: state => state.expirydocreport.list,
        isLoading: state => state.expirydocreport.isLoading,
      }),
      parsedDirection() {
        return this.direction.split(' ')
      },
      pagedData() {
        return this.list.items.slice(this.from, this.to)
      },


      queriedData() {
        if (!this.searchQuery) {
          if (this.list.items != null) {
            this.pagination.total = this.list.items.length
            return this.pagedData
          }
        }
        if (this.list.items != null) {
          let result = this.list.items
            .filter((row) => {
              let isIncluded = false
              for (let key of this.propsToSearch) {
                let rowValue = row[key].toString()
                if (rowValue.includes && rowValue.includes(this.searchQuery)) {
                  isIncluded = true
                }
              }
              return isIncluded
            })

          console.log(result);
          this.pagination.total = result.length
          return result.slice(this.from, this.to)
        }
      },
      to() {
        let highBound = this.from + this.pagination.perPage
        if (this.total < highBound) {
          highBound = this.total
        }
        return highBound
      },
      from() {
        return this.pagination.perPage * (this.pagination.currentPage - 1)
      },
      total() {
        if (this.list.items != null) {
          this.pagination.total = this.list.items.length
          return this.list.items.length
        }
      }

    },
    mounted: function () {
       this.fetchList(0,0);
      this.getComplianceList().then(res => {
        console.log(res);
        this.documentList = res.items;
      })
    }

  }</script>

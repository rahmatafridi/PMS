
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
              <label class="title headersize">Empty Room</label>
            </div>
            <div class="col-lg-3">

            </div>
            <div class="col-lg-3">
              <div>
                <vue-excel-xlsx :data="list.items"
                                :columns="columns"
                                :filename="'Empty Room Report'"
                                class="fa fa-file-excel-o exceldowload"
                                title="EXCEL"
                                :sheetname="'sheetname'">

                </vue-excel-xlsx>
                <button style="float:right;margin-right:5px;" v-on:click="generate"><span class="fa fa-file-pdf-o" title="Pdf"></span></button>

              </div>
              <div>
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
          <div class="col-sm-6">
            <el-select class="select-default"
                       v-model="pagination.perPage"
                       placeholder="Per page">
              <el-option class="select-default"
                         v-for="item in pagination.perPageOptions"
                         :key="item"
                         :label="item"
                         :value="item">
              </el-option>
            </el-select>
          </div>
          <div class="col-sm-6">
            <!--<div class="pull-right">
              <label>
                <input type="text" class="form-control input-sm" placeholder="Search records" v-model="searchQuery">
              </label>
            </div>-->
          </div>

          <div class="card-content table-responsive table-full-width table">
            <el-table :data="queriedData">
              <el-table-column class="success" label="Provider" property="provider"></el-table-column>
              <el-table-column :min-width="120" label="Property" property="property"></el-table-column>
              <el-table-column class="danger" label="Room" property="room"></el-table-column>
           
              <el-table-column :min-width="40"
                               fixed="right"
                               label="Actions">
                <template slot-scope="props">
                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-pencil-alt"></i></a>
                </template>
              </el-table-column>
            </el-table>

          </div>
          <div class="col-sm-6 pagination-info paggingcss" >
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

  </div>
</template>
<script>
  import Vue from 'vue'
  import swal from 'sweetalert2'
  import jsPDF from 'jspdf'
  import 'jspdf-autotable'

  import XLSX from 'xlsx';
  import VueExcelXlsx from "vue-excel-xlsx";
  Vue.use(VueExcelXlsx);
  import { Table, TableColumn, Select, Option } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  import { mapState } from 'vuex'


  export default {
    components: {
      PPagination
    },
    data() {
      return {
        tableData: [],
        columns: [
          {
            label: "Provider",
            field: "provider",
          },
          {
            label: "Property",
            field: "property",

          },
          {
            label: "Room",
            field: "room",
          }

        ],

        options: {},
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
        filename: '',
        autoWidth: true,
        bookType: 'xlsx'
      }
    },
    methods: {
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
      fetchList() {

        this.$store.dispatch('report/fetchList').catch(e => {
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
      generate() {

        //var value = event.target.value;
        /*      if (value == 1) {*/
        const columns = [
          { title: "Provider", dataKey: "provider" },
          { title: "Property", dataKey: "property" },
          { title: "Room", dataKey: "room" }
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
        doc.text(400, y = y + 25, "Empty Room Report");

        // Using array of sentences
        //doc
        //  .setFont("helvetica")
        //  .setFontSize(12)
        //  .text(this.moreText, 0.5, 3.5, { align: "left", maxWidth: "7.5" });

        // Creating footer and saving file
        doc.save(`EmptyRoom.pdf`);
        //html2pdf(this.$refs.pdf, {
        //  margin: 1,
        //  filename: 'document.pdf',
        //  image: { type: 'jpeg', quality: 0.98 },
        //  html2canvas: { dpi: 192, letterRendering: true },
        //  jsPDF: { unit: 'in', format: 'letter', orientation: 'landscape' }
        //})
        /*  }*/
        //if (value == 2) {
        //  //import('Export2Excel').then(excel => {
        //  //  const tHeader = ['Name', 'Address', 'Start Date', 'End Date', 'CRN', 'Local Authority', 'Gender', 'Ethnicty', 'Referral','Support Plan']
        //  //  const filterVal = ['name', 'address', 'startDate', 'endDate', 'crn', 'localAuthorithy', 'gender', 'ethnicty','referral','supportPlan']
        //  //  const list = this.tableData
        //  //  const data = this.formatJson(filterVal, list)
        //  //  excel.export_json_to_excel({
        //  //    header: tHeader,
        //  //    data,
        //  //    filename: this.filename,
        //  //    autoWidth: this.autoWidth,
        //  //    bookType: this.bookType
        //  //  })
        //  ////  this.downloadLoading = false
        //  //})

        //  //const data = XLSX.utils.json_to_sheet(this.tableData)
        //  //const wb = XLSX.utils.book_new()
        //  //XLSX.utils.book_append_sheet(wb, data, 'data')
        //  //XLSX.writeFile(wb, 'demo.xlsx')
        //}
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
      }
      
    },
    computed: {
      ...mapState({
        list: state => state.report.list,
        isLoading: state => state.report.isLoading,
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
          this.tableData = this.list.items;
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
       this.fetchList();

    }

  }</script>

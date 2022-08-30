<template>
  <div>
    <div class="row">
      <div class="col-lg-6">
        <input @click="addNew" style="float:left;" type="button" class="btn btn-primary" value="Add" />

      </div>
      <div class="col-lg-6">

      </div>
    </div>
    <div class="row">
      <div class="col-sm-6">

      </div>
      <div class="col-sm-6">
        <div class="pull-right">
          <label>
            <input type="text" class="form-control input-sm" placeholder="Search records" v-model="searchQuery">
          </label>
        </div>
      </div>

      <div class="card-content table-responsive table-full-width table">
        <el-table :data="queriedData">
          <el-table-column class="success" label="Document" property="title"></el-table-column>

          <el-table-column :min-width="120" label="Name" property="name"></el-table-column>
          <el-table-column class="success" label="Valid From" property="validFromDate">

            <template slot-scope="props">
              {{ props.row.validFromDate | formatDate }}
            </template>
          </el-table-column>

          <el-table-column label="Valid To" property="validToDate">
            <template slot-scope="props">
              {{ props.row.validToDate | formatDate }}
            </template>

          </el-table-column>

          <el-table-column label="Updated By" property="updatedBy"></el-table-column>

          <el-table-column :min-width="40"
                           fixed="right"
                           label="Actions">
            <template slot-scope="props">
              <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-pencil-alt"></i></a>
              <a class="btn btn-simple btn-xs btn-danger btn-icon remove" @click="handleDelete(props.$index, props.row)"><i class="ti-close"></i></a>
            </template>
          </el-table-column>
        </el-table>

      </div>
      <div class="col-sm-6 pagination-info paggingcss">
        <p style="float:left;" class="category">Showing {{from + 1}} to {{to}} of {{total}} entries</p>
      </div>
      <div class="col-sm-6">
        <p-pagination class="pull-right"
                      v-model="pagination.currentPage"
                      :per-page="pagination.perPage"
                      :total="pagination.total">
        </p-pagination>
      </div>
    </div>
    <Modal v-model="showModal" title="Document">
      <div class="row">
        <div class="col-lg-12">
          <input type="hidden" v-model="compliancePropertyDoc.propertyId" />
          <div class="form-group">
            <div class="form-group">
              <label>Type</label>
              <select class="form-control" @change="onChange($event)" v-model="compliancePropertyDoc.complianceId">
                <option v-for="item in documentList" :value="item.id">
                  {{item.title}}
                </option>
              </select>
            </div>
          </div>
          <div class="form-group">

            <label>Valid From</label>
            <div class="form-group">
              <el-date-picker v-model="compliancePropertyDoc.expiryDateFrom"
                              type="date" placeholder="Select From Valid"
                              format="dd/MM/yyyy"
                              :picker-options="pickerOptions1">
              </el-date-picker>
            </div>
          </div>
          <div class="form-group">

            <label>Valid To</label>
            <div class="form-group">
              <el-date-picker v-model="compliancePropertyDoc.expiryDateTo"
                              type="date" placeholder="Select To Valid"
                              format="dd/MM/yyyy"
                              :picker-options="pickerOptions1">
              </el-date-picker>
            </div>
          </div>
          <div class="form-group">
            <label>File</label>
            <input type="file" class="form-control" id="file" ref="file" v-on:change="handleFileUpload" />

          </div>

          <div class="row modal-footer">
            <div class="col-sm-12">
              <div class="float-right">
                <button class="btn btn-info" type="button" @click.prevent="validate">Submit</button>
                <button class="btn btn-secondary ml-2" type="button" @click="onCancel">Cancel</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Modal>


  </div>
</template>
<script>
  import Vue from 'vue'
  import swal from 'sweetalert2'
  import VueModal from '@kouts/vue-modal'
  import '@kouts/vue-modal/dist/vue-modal.css'
  import { Table, TableColumn, Select, Option, DatePicker } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  import { mapFields } from 'vee-validate'

  import { mapState } from 'vuex'
  Vue.component('Modal', VueModal)
  import { addUpdateComplianceDoc, getComplianceDocById } from '@/helpers/compliancedocs'
  import { getComplianceList } from '@/helpers/compliance'
  export default {
    components: {
      PPagination,
      'Modal': VueModal,
      [DatePicker.name]: DatePicker,

    },
    data() {
      return {
        showModal: false,
        tableData: [],
        secret: "123#$%",
        options: {},
        errorMessage: null,
        pagination: {
          perPage: 10,
          currentPage: 1,
          perPageOptions: [5, 10, 25, 50],
          total: 0
        },
        filedata:'',
        searchQuery: '',
        propsToSearch: ['complianceId'],
        compliancePropertyDoc: {
          name: '',
          compliancePropertyId: 0,
          expiryDateFrom: '',
          expiryDateTo: '',
          fileName:'',
          complianceId: 0,
          extension: '',
          mimeType: '',
          docObject: '',
          size: 0,
          id: 0,
          propertyId:0,
         
        },
        docList: [],
        documentList:[],
        modelValidations: {
         
          complianceId: {
            required: true,
          }

        },
        pickerOptions1: {
          shortcuts: [{
            text: 'Today',
            onClick(picker) {
              picker.$emit('pick', new Date())
            }
          },
          {
            text: 'Yesterday',
            onClick(picker) {
              const date = new Date()
              date.setTime(date.getTime() - 3600 * 1000 * 24)
              picker.$emit('pick', date)
            }
          },
          {
            text: 'A week ago',
            onClick(picker) {
              const date = new Date()
              date.setTime(date.getTime() - 3600 * 1000 * 24 * 7)
              picker.$emit('pick', date)
            }
          }]
        },
        propertyId: 0,
        defalutValue:0,
      }
    },
    methods: {
      addUpdateComplianceDoc: addUpdateComplianceDoc,
      getComplianceDocById: getComplianceDocById,
      getComplianceList: getComplianceList,
  

      async submit() {
        try {
         
          let id = this.$route.params.id;
          this.compliancePropertyDoc.propertyId = parseInt(this.propertyId);
          if (this.docObject == '' || this.docObject == null) {

          }
          const isValidForm = this.IsValid;
          if (isValidForm) {

            //axios.post('http://localhost:51944/v1/compliancePropertyDoc/add', formData).then((res) => {
            //  console.log(res)
            //})

            await this.addUpdateComplianceDoc(this.compliancePropertyDoc)
              .then(res => {
                if (res != null || res != undefined)
                  this.complianceDocList();
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Document Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })
                  this.showModal = false;
                  
              })
              .catch(e => {
                this.errorMessage = e
                swal({
                  title: 'Warning',
                  text: this.errorMessage,
                  type: 'warning',
                  confirmButtonClass: 'btn btn-success btn-fill',
                  buttonsStyling: false
                })              })
          }
        } finally {
          //
        }
      },
      onCancel() {
        this.showModal = false;
      },
      getError(fieldName) {
        return this.errors.first(fieldName)
      },
      validate() {
        this.$validator.validateAll().then(isValid => {
          this.IsValid = isValid;
          this.$emit('submit', this.registerForm, isValid)
          this.submit();
        })
      },
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
      complianceDocList() {

        this.$store.dispatch('compliancedocs/fetchList', {
          auth: {
            proId: this.propertyId
          },
        }).catch(e => {
          this.errorMessage = e
          //swal({
          //  title: 'Worng',
          //  text: this.errorMessage,
          //  type: 'warning',
          //  confirmButtonClass: 'btn btn-success btn-fill',
          //  buttonsStyling: false
          //})
        })

      },
      complianceList() {

        this.$store.dispatch('compliance/fetchList').catch(e => {
          this.errorMessage = e
          //swal({
          //  title: 'Worng',
          //  text: this.errorMessage,
          //  type: 'warning',
          //  confirmButtonClass: 'btn btn-success btn-fill',
          //  buttonsStyling: false
          //})
        })

      },
      handleEdit(index, row) {
        if (row.id != null) {

          this.getDocumentById(row.id).then(res => {
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
      handleDelete(index, row) {
        var root = this;
        swal({
          title: 'Are you sure?',
          text: `You won't be able to revert this!`,
          type: 'warning',
          showCancelButton: true,
          confirmButtonClass: 'btn btn-success btn-fill',
          cancelButtonClass: 'btn btn-danger btn-fill',
          confirmButtonText: 'Yes, delete it!',
          buttonsStyling: false
        }).then(function () {
          root.$store.dispatch('compliancedocs/delete', row.id)
          root.complianceDocList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
       addNew() {
         this.getComplianceList().then(res => {
          console.log(res);
          this.documentList = res.items;
         })
         //this.docData;
        // this.
         this.compliancePropertyDoc.complianceId = 0;

         this.compliancePropertyDoc.compliancePropertyId = 0;
         this.compliancePropertyDoc.propertyId = 0;
        this.showModal = true;

        //this.$router.push('/users/add')

      },
      handleFileUpload(e) {

       // this.formDoc.file =  this.$refs.file.files[0];
        //console.log(this.formDoc);
      var file = e.target.files[0];
        // this.form.file = e.target.files[0];
        var name = file.name;
        var fileExtension = file.name.split('.').pop();
        this.compliancePropertyDoc.extension = fileExtension;
        this.compliancePropertyDoc.mimeType = file.type;
        this.compliancePropertyDoc.size = file.size;
        this.compliancePropertyDoc.name = name;
        let reader = new FileReader();
        reader.onload = (e) => {
          let image = e.target.result;
          this.compliancePropertyDoc.docObject = image;
        //  this.form.extension=
        };
        reader.readAsDataURL(file);


      },
      async loadDocument() {

       await this.getComplianceList()
          .then(res => {
            res.forEach((value, index) => {
              this.documentList.push({ value: value.id, text: value.title });
            });
            //this.clients = res;

          })
          .catch(ex => {
            this.errorMessage = ex
            swal({
              title: 'Warning',
              text: this.errorMessage,
              type: 'warning',
              confirmButtonClass: 'btn btn-success btn-fill',
              buttonsStyling: false
            })          })
      },
      convert(str) {
        var date = new Date(str),
          mnth = ("0" + (date.getMonth() + 1)).slice(-2),
          day = ("0" + date.getDate()).slice(-2);
        return [date.getFullYear(), mnth, day].join("-");
      },

      onChange(event) {
        
        var myDate = new Date();
        var id = event.target.value;
        this.defalutValue = this.documentList.find(x => x.id == id).defaultRenewValue;
        console.log(this.compliancePropertyDoc.expiryDateFrom);
        
        myDate = this.convert(this.compliancePropertyDoc.expiryDateFrom);
        console.log(myDate);
        var year = new Date(myDate).getFullYear();
        var month = new Date(myDate).getMonth();
        var day = new Date(myDate).getDate();
        var date = new Date(year + this.defalutValue, month, day);
        this.compliancePropertyDoc.expiryDateTo = date;
      },
      aesDecript(txt) {
        var CryptoJS = require("crypto-js");

        var bytes = CryptoJS.AES.decrypt(txt, this.secret);
        var originalText = bytes.toString(CryptoJS.enc.Utf8);
        return originalText;
      }
    },
    computed: {
      ...mapFields(['complianceId']),

      ...mapState({
        list: state => state.compliancedocs.list,
        isLoading: state => state.compliancedocs.isLoading
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
      },
      docData() {
    
        if (this.listDoc.items != null) {
          let result = this.listDoc.items

          return result;
        }
      },
  
    },
    mounted: function () {
     // let id = this.$route.params.id;
      
      let str = this.$route.params.id;
      var dId = this.aesDecript(str);
      var id = parseInt(dId.slice(0, -1));
      this.propertyId = id;
      this.complianceDocList();
    //  this.loadDocument();
      this.complianceList();
      this.compliancePropertyDoc.expiryDateFrom = new Date();
    }

  }
</script>
<style>
  .modal-footer {
    padding: 15px;
    text-align: right;
    border-top: 0px solid #e5e5e5;
  }
</style>


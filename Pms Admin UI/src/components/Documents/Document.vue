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
        <!--<el-select class="select-default"
                 style="float:left"
                 v-model="pagination.perPage"
                 placeholder="Per page">
        <el-option class="select-default"
                   v-for="item in pagination.perPageOptions"
                   :key="item"
                   :label="item"
                   :value="item">
        </el-option>
      </el-select>-->
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
          

          <el-table-column class="success" label="Name" property="name"></el-table-column>

          <el-table-column :min-width="120" label="Category" property="categoryId"></el-table-column>
          <el-table-column class="success" label="Description" property="description"></el-table-column>

          <el-table-column label="Created By" property="addedBy"></el-table-column>

          <el-table-column label="Created Date" property="addedDate">
            <template slot-scope="props">
              {{ props.row.addedDate | formatDate }}
            </template>
          </el-table-column>

          <el-table-column :min-width="80"
                           fixed="right"
                           label="Actions">
            <template slot-scope="props">
              <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleView(props.$index, props.row)"><i class="ti-eye"></i></a>

              <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-cloud-down"></i></a>
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
          <div class="form-group">
            <div class="form-group">
              <label>Name</label>
              <input type="text" class="form-control" v-validate="modelValidations.name" name="name" v-model="form.name" />
              <small class="text-danger" v-show="name.invalid">
                {{ getError('name') }}
              </small>
            </div>
            <label>Category</label>
            <select class="form-control" v-model="form.categoryId" v-validate="modelValidations.categoryId" name="categoryId">
              <option value="">select</option>
              <option value="2">Provider Notes</option>
              <option value="1">General Note</option>

            </select>
            <small class="text-danger" v-show="categoryId.invalid">
              {{ getError('categoryId') }}
            </small>
          </div>
          <div class="form-group">
            <label>File</label>
            <input type="file" class="form-control" id="file" ref="file" v-on:change="handleFileUpload" />

          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea class="form-control" v-validate="modelValidations.description" name="description" v-model="form.description"></textarea>
            <small class="text-danger" v-show="description.invalid">
              {{ getError('note') }}
            </small>
          </div>
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
    </Modal>


    <Modal v-model="showModalPreview" title="Document Preview">
      <div class="row">
        <!--<div id="pdfPreview" class="modal-body" style="width:100%!important">
  </div>-->
        <iframe :src="ruta"  height="500px"></iframe>

      </div>
    </Modal>
  </div>
</template>
<script>
  import Vue from 'vue'
  import store from '@/store/index'
  import swal from 'sweetalert2'
  import VueModal from '@kouts/vue-modal'
  import '@kouts/vue-modal/dist/vue-modal.css'
  import { Table, TableColumn, Select, Option } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  import { mapFields } from 'vee-validate'

  import { mapState } from 'vuex'
  Vue.component('Modal', VueModal)
  import {addUpdateDocument, getDocumentById} from '@/helpers/document'

  export default {
    components: {
      PPagination,
      'Modal': VueModal

    },
    data() {
      return {
        ruta:'',
        showModal: false,
        showModalPreview: false,
        tableData: [],
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
        propsToSearch: ['note'],
        form: {
          name: '',
          file: '',
          description:'',
          categoryId: 0,
          extension: '',
          mimeType: '',
          docObject: '',
          size: 0,
          id: 0,
          parentId: 0,
          typeId: 0,
          clientId: 0,
        },
        fileType: false,
        modelValidations: {
          name: {
            required: true,
          },
          categoryId: {
            required: true,
          },
          description: {
            required: true
          }

        },


      }
    },
    methods: {
      addUpdateDocument: addUpdateDocument,
      getDocumentById: getDocumentById,

 
      async submit() {
        try {
          this.form.categoryId = parseInt(this.form.categoryId);
          this.form.clientId = store.state.login.user.clientId;
          if (this.fileType == false) {
             
          }

          const isValidForm = this.IsValid;
          if (isValidForm && this.fileType) {


            await this.addUpdateDocument(this.form)
              .then(res => {
                if (res != null || res != undefined)
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
                this.documentList();
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
      documentList() {

        this.$store.dispatch('document/fetchList', {
          auth: {
            proId: this.form.parentId
          },
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
      handleView(index, row) {
        if (row.id != null) {

          this.getDocumentById(row.id).then(res => {
            const fileName = "";
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
          root.$store.dispatch('document/delete', row.id)
          root.documentList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      addNew() {
        this.form.categoryId = "";
        this.form.name = "";
        this.showModal = true;

        //this.$router.push('/users/add')

      },
      handleFileUpload(e) {
     
        //debugger;
        var file = e.target.files[0];
        var fileExtension = file.name.split('.').pop();
        if (fileExtension == "pdf" || fileExtension == "png" || fileExtension == "jpg" || fileExtension == "jpeg") {
          this.form.extension = fileExtension;
          this.form.mimeType = file.type;
          this.form.size = file.size;
          let reader = new FileReader();
          reader.onload = (e) => {
            let image = e.target.result;
            console.log(image);
            this.form.docObject = image;
            //  this.form.extension=
          };
          reader.readAsDataURL(file);
          this.fileType = true;
        }
        else {
          swal({
            title: 'Warning',
            text: `please change the file formate`,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })
          this.fileType = false;

        }
      
    
      }
    },
    computed: {
      ...mapFields(['name', 'categoryId','description']),

      ...mapState({
        list: state => state.document.list,
        isLoading: state => state.document.isLoading,
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
/*      let id = this.$route.params.id;*/
    

      let str = this.$route.params.id;
      var dId = this.aesDecript1(str);
      var id = parseInt(dId.slice(0, -1));
      this.form.parentId = parseInt(id);

      this.form.typeId = 1;
      this.documentList();

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


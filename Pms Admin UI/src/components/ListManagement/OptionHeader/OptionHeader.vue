<style>
 /* .ui.fluid.dropdown > .dropdown.icon {
    float: right;
     margin-top: -7px !important; 
  }*/
  .modal-footer {
    padding: 15px;
    text-align: right;
    border-top: 0px solid #e5e5e5 !important;
  }
</style>

<template>
  <div class="row">



    <div class="col-md-12">
      <div class="card">
        <div class="card-header">
          <div class="row">
            <div class="col-lg-6">
              <label class="title headersize">Option Header</label>
            </div>
            <div class="col-lg-6">
              <button class="btn btn-primary btn-small" style="float:right" v-on:click="addNew">New Header</button>
            </div>
          </div>
          <hr />
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
                <el-table-column class="success" label="Title" property="title"></el-table-column>

                <el-table-column :min-width="120" label="Client" property="clientName"></el-table-column>


                <el-table-column :min-width="80"
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

        </div>
      </div>
    </div>


    <Modal v-model="showModal" title="Option Header">
      <div class="row">
        <div class="col-lg-12">
          <div class="form-group">
            <div class="form-group">
              <label>Title</label>
              <input type="text" class="form-control" v-validate="modelValidations.title" name="title" v-model="form.title" />
              <small class="text-danger" v-show="title.invalid">
                {{ getError('name') }}
              </small>
            </div>
            <div v-if="clientId == 0">
              <label>Client</label>
              <model-select :options="clients"
                            v-model="form.clientId"
                            placeholder="select client">
              </model-select>
            </div>
            <!--<select class="form-control" v-model="form.clientId" v-validate="modelValidations.clientId" name="clientId">
    <option value="">select</option>

  </select>-->
            <!--<small class="text-danger" v-show="clientId.invalid">-->
            <!--{{ getError('clientId') }}
  </small>-->
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
  </div>
       
         


   
</template>
<script>
  import Vue from 'vue'
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
  import {addUpdateHeader, getHeaderById} from '@/helpers/optionheader'
  import { getClients } from '@/helpers/client'
  import { ModelSelect } from 'vue-search-select'

  export default {
    components: {
      PPagination,
      'Modal': VueModal,
      ModelSelect
    },
    data() {
      return {
        ruta:'',
        showModal: false,
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
        propsToSearch: ['title'],
        form: {
          title: '',
          clientId:0,
          id: 0,
      
        },
        fileType: false,
        modelValidations: {
          title: {
            required: true,
          },
          clientId: {
            required: true,
          }
         
        },
        clients: [],

        clientId:''
      }
    },
    methods: {
      addUpdateHeader: addUpdateHeader,
      getHeaderById: getHeaderById,
      getClients: getClients,


      async submit() {
        try {
          //this.form.clientId = store.state.login.user.clientId;
      debugger

          const isValidForm = this.IsValid;
          if (isValidForm) {


            await this.addUpdateHeader(this.form)
              .then(res => {
                if (res != null || res != undefined)
                  this.showModal = false;
                this.optionheaderList();
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Header Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })
              })
              .catch(e => {
                this.errorMessage = e
                //swal({
                //  title: 'Warng',
                //  text: this.errorMessage,
                //  type: 'warning',
                //  confirmButtonClass: 'btn btn-success btn-fill',
                //  buttonsStyling: false
                //})
              })
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
      optionheaderList() { 

        this.$store.dispatch('optionheader/fetchList').catch(e => {
          this.errorMessage = e
          //swal({
          //  title: 'Warng',
          //  text: this.errorMessage,
          //  type: 'warning',
          //  confirmButtonClass: 'btn btn-success btn-fill',
          //  buttonsStyling: false
          //})
        })
      },

      handleEdit(index, row) {
        if (row.id != null) {
          this.form.clientId = 0;
          this.form.title = '';
          this.clientId = window.localStorage.getItem("clientId");
          if (this.clientId != 0) {
            this.form.clientId = parseInt(this.clientId);
          }
          this.getHeaderById(row.id).then(res => {
            this.form.clientId = res.clientId;
            this.form.title = res.title;
            this.form.id = res.id;
            this.showModal = true;
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
          root.$store.dispatch('optionheader/delete', row.id)
          root.documentList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      addNew() {
        this.form.clientId = "";
        this.form.title = "";
        this.clientId = window.localStorage.getItem("clientId");
        if (this.clientId != 0) {
          this.form.clientId = parseInt(this.clientId);
        }
        this.showModal = true;

        //this.$router.push('/users/add')

      },
    },
    computed: {
      ...mapFields(['title', 'clientId']),

      ...mapState({
        list: state => state.optionheader.list,
        isLoading: state => state.header.isLoading,
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
      this.clientId = window.localStorage.getItem("clientId");
      if (this.clientId != 0) {
        this.form.clientId = parseInt(this.clientId);
      }
      this.optionheaderList();
      let val = "";
      this.getClients(val)
        .then(res => {
          res.forEach((value, index) => {
            this.clients.push({ value: value.id, text: value.name });
          });
          //console.log(res);
          //this.clients = res;

        })
        .catch(ex => {
          this.errorMessage = ex
          //swal({
          //  title: 'Warng',
          //  text: this.errorMessage,
          //  type: 'warning',
          //  confirmButtonClass: 'btn btn-success btn-fill',
          //  buttonsStyling: false
          //})
        })
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


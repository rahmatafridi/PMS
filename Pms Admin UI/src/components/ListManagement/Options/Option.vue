<style>
  .ui.fluid.dropdown > .dropdown.icon {
    float: right;
    margin-top: 0px !important;
  }
</style>
<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">

        <div class="card-header">
          <div class="row">
            <div class="col-lg-3">
              <h4 class="title">List Management</h4>
            </div>
            <div class="col-lg-3" v-if="clientId==0">
              <label>Client</label>
              <model-select :options="clients"
                            v-model="form.clientId"
                            @input="getProiver"
                            placeholder="select client">
              </model-select>
              <small class="text-danger" v-if="clinterror">
                Please Select Client
              </small>
            </div>
            <div class="col-lg-3">
              <label>Option Header</label>
              <model-select :options="headers"
                            v-model="form.headerId"
                            @input="getOption"
                            placeholder="select header">
              </model-select>
              <small class="text-danger" v-if="optionerror">
                Please Select Option
              </small>

            </div>
          </div>
          <hr />
        </div>
        <div style="margin-left: 30px; margin-right:29px;">
          <div class="row">
            <div class="col-lg-3">
              <div class="form-group">
                <input class="form-control" type="text" v-validate="modelValidations.title" name="title" placeholder="Enter Title" v-model="form.title" />
                <small class="text-danger" v-show="title.invalid">
                  {{ getError('title') }}
                </small>
              </div>

            </div>
            <div class="col-lg-3">
              <div class="form-group">
                <input class="form-control" @keypress="isNumber($event)" type="text" v-validate="modelValidations.value" placeholder="Enter Value" v-model="form.value" name="value" />
                <small class="text-danger" v-show="value.invalid">
                  {{ getError('value') }}
                </small>
              </div>
            </div>
            <div class="col-lg-3">
              <div class="form-group">
                <input class="form-control" @keypress="isNumber($event)" type="text" v-model="form.sortOrder" v-validate="modelValidations.sortOrder" placeholder="Enter sortOrder" name="sortOrder" />
                <small class="text-danger" v-show="sortOrder.invalid">
                  {{ getError('sortOrder') }}
                </small>
              </div>
            </div>
            <div class="col-lg-3">
              <div>
                <button class="btn btn-primary" @click.prevent="validate">{{btnadd}}</button>
              </div>
            </div>
          </div>

          <div>
            <div class="card-content table-responsive table-full-width table">
              <el-table :data="options">
                <el-table-column class="success" label="Title" property="title"></el-table-column>

                <el-table-column  label="Value" property="value"></el-table-column>
                <el-table-column class="success" label="Sort Order" property="sortOrder"></el-table-column>


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


            <!--<table class="table table-bordered table-hover">
              <tbody>
                <tr v-for="item in options">
                  <td style="width:22%">{{item.title}}</td>
                  <td style="width:24%">{{item.value}}</td>
                  <td style="width:20%">{{item.value}}</td>
                  <td style="width:25%">
                    <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(item.id)"><i class="ti-pencil-alt"></i></a>
                    <a class="btn btn-simple btn-xs btn-danger btn-icon remove" @click="handleDelete(item.id)"><i class="ti-close"></i></a>
                  </td>

                </tr>
              </tbody>
            </table>-->
          </div>
        </div>
      </div>  
      </div>
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
  import {getHeaderListById } from '@/helpers/optionheader'
  import { getOptionListById, addUpdateOption, getOptionById, validateOption} from '@/helpers/option'
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
        clinterror: false,
        optionerror: false,

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
          headerId: 0,
          value: '',
          sortOrder:''

        },
        fileType: false,
        modelValidations: {
          headerId: {
            required: true,
          },
          clientId: {
            required: true,
          },
          title: {
            required:true,
          },
          value: {
            required:true,
          },
          sortOrder: {
            required:true
          }

        },
        clients: [],
        headers: [],
        options: [],
        btnadd: 'Add',
        clientId1:''
        
      }
    },
    methods: {
      getHeaderListById: getHeaderListById,
      getClients: getClients,
      getOptionListById: getOptionListById,
      addUpdateOption: addUpdateOption,
      getOptionById: getOptionById,
      validateOption: validateOption,
      async submit() {
        try {
          if (this.form.clientId == 0) {
            this.clinterror = true;
            //alert("test");
            return;
          }
          if (this.form.headerId ==0) {
            this.optionerror = true;
            return;
          }
          await this.validateOption(this.form.id,this.form.headerId, this.form.value, this.form.title)
            .then(res => {
            })
            .catch(e => {
              console.log(e);
              swal({
                title: `Warning`,
                text: e,
                buttonsStyling: false,
                confirmButtonClass: 'btn btn-info btn-fill'
              })
              this.IsValid = false;
           
              this.errorMessage = e
              swal({
                title: 'Warning',
                text: this.errorMessage,
                type: 'warning',
                confirmButtonClass: 'btn btn-success btn-fill',
                buttonsStyling: false
              })            })
          const isValidForm = this.IsValid;

          if (isValidForm) {
            this.form.sortOrder = parseInt(this.form.sortOrder);
     
            await this.addUpdateOption(this.form)
              .then(res => {
                console.log(res);
                if (res != null || res != undefined)
                  this.showModal = false;
                this.getOption();
                this.reset();
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Option Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })
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

      handleEdit(value, row) {
        if (row.id != null) {
          this.clientId1 = window.localStorage.getItem("clientId");
          if (this.clientId1 != 0) {
            this.form.clientId = parseInt(this.clientId1);
          }
          this.getOptionById(row.id).then(res => {

            //console.log(res);
            this.form.title = res.title;
            this.form.value = res.value;
            this.form.sortOrder = res.sortOrder;
            this.form.id = res.id;
            this.btnadd = 'Update';
            //this.form.clientId = res.clientId;
            //this.form.title = res.title;
            //this.form.id = res.id;
            //this.showModal = true;
          })
        }
      },

      handleDelete(value, row) {
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
          root.$store.dispatch('option/delete', row.id)
          root.getOption();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      getProiver() {
        this.clinterror = false;
        this.headers = [];
        this.getHeaderListById(this.form.clientId)
          .then(res => {

            //this.providers = res
            //conosle.log(this.providers);
            res.forEach((value, index) => {
              this.headers.push({ value: value.id, text: value.title });
            });
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
      },
      getOption() {
        this.optionerror = false;

        this.options = [];
        this.getOptionListById(this.form.headerId)
          .then(res => {

            //this.providers = res
            //conosle.log(this.providers);
            console.log(res);
            this.options = res;
            //res.forEach((value, index) => {
            //  this.headers.push({ value: value.id, text: value.title });
            //});
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
      reset() {
        this.form.value = '';
        this.form.title = '';
        this.form.sortOrder = '';
        this.form.id = 0;
        this.btnadd = 'Add';
      },
      isNumber: function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
          evt.preventDefault();;
        } else {
          return true;
        }
      }

    },
    computed: {
      ...mapFields(['title', 'clientId','title','value','sortOrder']),

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
      debugger
      this.clientId1 = window.localStorage.getItem("clientId");
      if (this.clientId1 != 0) {


        this.form.clientId = parseInt(this.clientId1);
        this.getProiver();
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
          swal({
            title: 'Warning',
            text: this.errorMessage,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })        })
    }

  }
</script>

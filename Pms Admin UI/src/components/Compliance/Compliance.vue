<template>

  <div class="row">



    <div class="col-md-12">
      <div class="card">
        <div class="card-header">
          <div class="row">
            <div class="col-lg-6">
              <label class="title headersize">Compliance</label>
            </div>
            <div class="col-lg-6">
              <button class="btn btn-primary btn-small" style="float:right" v-on:click="addNew">New Comliance</button>
            </div>
          </div>
          <hr />
        </div>
        <div class="card-content row">
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
              <el-table-column :min-width="120" label="SortOrder" property="sortOrder"></el-table-column>


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
          <div class="col-sm-6 pagination-info paggingcss" >
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
        <Modal v-model="showModal" title="Compliance">
          <div class="row">
            <div class="col-lg-12">

              <div class="form-group">
                <label>Title</label>
                <input class="form-control" v-validate="modelValidations.title" name="title" v-model="form.title" />
                <small class="text-danger" v-show="title.invalid">
                  {{ getError('title') }}
                </small>
              </div>

              <div class="form-group">
                <label>Sortorder</label>
                <input class="form-control" v-validate="modelValidations.sortOrder" @keypress="isNumber($event)" name="sortOrder"  type="text" v-model="form.sortOrder" />
                <small class="text-danger" v-show="sortOrder.invalid">
                  {{ getError('sortOrder') }}
                </small>
              </div>

              <div class="form-group">
                <label>Defult Renew Value</label>
                <input class="form-control"  v-validate="modelValidations.defaultRenewValue" @keypress="isNumber($event)" name="defaultRenewValue" type="text" v-model="form.defaultRenewValue" />
                <small class="text-danger" v-show="defaultRenewValue.invalid">
                  {{ getError('defaultRenewValue') }}
                </small>
              </div>
              <div class="form-group">
                <p-checkbox :checked="form.isRequired" v-model="form.isRequired">IsRequired</p-checkbox>


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
  import { addUpdateCompliance, getComplianceById} from '@/helpers/compliance'

  export default {
    props: ['propertyId'],

    components: {
      PPagination,
      'Modal': VueModal

    },
    data() {
      return {
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
        searchQuery: '',
        propsToSearch: ['title'],
        form: {
          title: '',
          isDefault: true,
          sortOrder: '',
          isRequired: false,
          defaultRenewValue: 1,
          defaulRenewType: 1,
          isVisibleToProvider: true,
          id: 0,
         
        },
        modelValidations: {
          title: {
            required: true,
          },
          defaultRenewValue: {
            required:true,
          },
          sortOrder: {
            required:true
          }
        },


      }
    },
    methods: {
      addUpdateCompliance: addUpdateCompliance,
      getComplianceById: getComplianceById,
      async submit() {
        try {
          debugger
          const isValidForm = this.IsValid;
          if (isValidForm) {
            this.form.defaulRenewType = parseInt(this.form.defaulRenewType);
            this.form.defaultRenewValue = parseInt(this.form.defaultRenewValue);
            this.form.sortOrder = parseInt(this.form.sortOrder);

            await this.addUpdateCompliance(this.form)
              .then(res => {
                if (res != null || res != undefined)
                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Added</b> </br> Compliance Added Successfully</span>`
                      },
                      icon: 'alert-with-icon',
                      horizontalAlign: 'right',
                      verticalAlign: 'top',
                      type: 'info'
                    })
                  this.showModal = false;
                this.complianceList();
              })
              .catch(e => {
                this.errorMessage = e
                this.color = 'error', this.direction = 'bottom right', this.snackbar = true
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
      complianceList() {

        this.$store.dispatch('compliance/fetchList').catch(e => {
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
          this.getComplianceById(row.id).then(res => {
            console.log(res);
            this.form = res;
            this.showModal = true;
          })
        }
        //this.$router.push(`/users/edit/${row.id}`)

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
          root.$store.dispatch('compliance/delete', row.id)
          root.noteList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      addNew() {
        this.form.isRequired = false;
        this.form.sortOrder = "";
        this.form.title = "";
        this.form.id = 0;
        this.showModal = true;

        //this.$router.push('/users/add')

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
      ...mapFields(['title', 'defaultRenewValue','sortOrder']),

      ...mapState({
        list: state => state.compliance.list,
        isLoading: state => state.compliance.isLoading,
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
    
     // console.log("Note", id);
      this.complianceList();

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


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
          <el-table-column class="success" label="Note" property="notes"></el-table-column>
          <el-table-column :min-width="120" label="Category" property="noteCategoryId">

          </el-table-column>
          <el-table-column label="Created By" property="addedBy"></el-table-column>

          <el-table-column label="Created Date" property="addedDate">
            <template slot-scope="props">
              {{ props.row.addedDate | formatDate }}
            </template>

          </el-table-column>

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
    <Modal v-model="showModal" title="Note">
      <div class="row">
        <div class="col-lg-12">
          <div class="form-group">
            <label>Category</label>
            <select class="form-control" v-model="form.noteCategoryId" v-validate="modelValidations.noteCategoryId" name="noteCategoryId">
              <option value="">select</option>
              <option value="2">Provider Notes</option>
              <option value="1">General Note</option>

            </select>
            <small class="text-danger" v-show="noteCategoryId.invalid">
              {{ getError('noteCategoryId') }}
            </small>
          </div>
          <div class="form-group">
            <label>Notes</label>
            <textarea class="form-control" v-validate="modelValidations.notes" name="notes" v-model="form.notes"></textarea>
            <small class="text-danger" v-show="notes.invalid">
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
  import {addUpdateNote, getNoteById} from '@/helpers/note'

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
        propsToSearch: ['note'],
        form: {
          notes: '',
          noteCategoryId: 0,
          parentId: 0,
          typeId:0,
        },
        modelValidations: {
          notes: {
            required: true,
          },
          noteCategoryId: {
            required: true,
          }
 
        },


      }
    },
    methods: {
      addUpdateNote: addUpdateNote,
      getNoteById: getNoteById,
      async submit() {
        try {
        debugger
          const isValidForm = this.IsValid;
          if (isValidForm) {
            this.form.noteCategoryId = parseInt(this.form.noteCategoryId);
            await this.addUpdateNote(this.form)
              .then(res => {
                if (res != null || res != undefined)
                  this.showModal = false;
                this.noteList();
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Note Added Successfully</span>`
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
                //  title: 'Worng',
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
      noteList() {

        this.$store.dispatch('note/fetchList', {
          auth: {
            proId: this.form.parentId
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
      handleEdit(index, row) {
        if (row.id != null) {
          this.getNoteById(row.id,1).then(res => {
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
          root.$store.dispatch('users/delete', row.id)
          root.noteList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      addNew() {
        this.form.noteCategoryId = "";
        this.form.notes = "";
        this.showModal = true;

        //this.$router.push('/users/add')

      },
     
    },
    computed: {
      ...mapFields(['notes', 'noteCategoryId']),

      ...mapState({
        list: state => state.note.list,
        isLoading: state => state.note.isLoading,
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
      let str = this.$route.params.id;
      var dId = this.aesDecript1(str);
      var id = parseInt(dId.slice(0, -1));
      this.form.parentId = parseInt(id);
      this.form.typeId = 1;
     // console.log("Note", id);
      this.noteList();

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


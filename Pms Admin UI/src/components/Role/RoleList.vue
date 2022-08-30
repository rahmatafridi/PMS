<template>
  <div class="row">



    <div class="col-md-12">
      <div class="card">
        <div class="card-header">
          <div class="row">
            <div class="col-lg-6">
              <label class="title headersize">Roles</label>
            </div>
            <div class="col-lg-6">
              <button class="btn btn-primary btn-small" style="float:right" v-on:click="addNew">New Role</button>
            </div>
          </div>
          <hr />
        </div>
        <div class="card-content row">
          <div class="col-sm-6">
          </div>
          <div class="col-sm-6">
            <div class="pull-right">
              <label>
                <input type="search" class="form-control input-sm" placeholder="Search records" v-model="searchQuery" aria-controls="datatables">
              </label>
            </div>
          </div>

          <div class="card-content table-responsive table-full-width table">
            <el-table :data="queriedData">
              <el-table-column class="success" label="Name" property="name"></el-table-column>
              <el-table-column :min-width="120" label="Description" property="description"></el-table-column>
              <el-table-column class="danger" label="Client Name" property="clientName"></el-table-column>

              <el-table-column :min-width="60"
                               fixed="right"
                               label="Actions">
                <template slot-scope="props">
                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handlePermission(props.$index, props.row)"><i class="ti-eye" title="Permission"></i></a>

                  <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-pencil-alt" title="Edit"></i></a>
                  <a class="btn btn-simple btn-xs btn-danger btn-icon remove" @click="handleDelete(props.$index, props.row)"><i class="ti-close" title="Delete"></i></a>
                </template>
              </el-table-column>
            </el-table>

          </div>
          <div class="col-sm-3 pagination-info paggingcss">
            <p class="category">Showing {{from + 1}} to {{to}} of {{total}} entries</p>
          </div>
          <div class="col-lg-7 mt-2" style="margin-top:17px;">
            <el-select class="select-default"
                       v-model="pagination.perPage"
                       style="float:right;"
                       placeholder="Per page">
              <el-option class="select-default"
                         v-for="item in pagination.perPageOptions"
                         :key="item"
                         :label="item"
                         :value="item">
              </el-option>
            </el-select>
          </div>
          <div class="col-sm-2">
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


  import { Table, TableColumn } from 'element-ui'
  Vue.use(Table)
  Vue.use(TableColumn)
  import { mapState } from 'vuex'
  import PPagination from 'src/components/UIComponents/Pagination.vue'

  export default {
    components: {
      PPagination
    },
    data() {
      return {
        tableData: [],
        options: {},
        client: null,
        secret: "123#$%",
        errorMessage: null,
        pagination: {
          perPage: 5,
          currentPage: 1,
          perPageOptions: [5, 10, 25, 50],
          total: 0
        },
        searchQuery: '',
        propsToSearch: ['titleNo', 'numberOfRooms', 'address1'],

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

        this.$store.dispatch('role/fetch').catch(e => {
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
        var newId = this.aesEncrypt(row.id + 'a');
        this.$router.push(`/role/edit/${newId}`)

      },
      handlePermission(index, row) {
        this.$router.push(`/permission/addEdit/${row.id}`)

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
          root.$store.dispatch('role/delete', row.id)
          root.fetchList();
        })
        //let indexToDelete = this.tableData.findIndex((tableRow) => tableRow.id === row.id)
        //if (indexToDelete >= 0) {
        //  this.tableData.splice(indexToDelete, 1)
        //}
      },
      addNew() {
        this.$router.push('/role/add')

      },
      aesEncrypt(txt) {
        var CryptoJS = require("crypto-js");
        var ciphertext = CryptoJS.AES.encrypt(txt, this.secret).toString();
        return ciphertext.toString()
      },
    },
    computed: {
      ...mapState({
        list: state => state.role.list,
        isLoading: state => state.role.isLoading,
      }),
      parsedDirection() {
        return this.direction.split(' ')
      },
      pagedData() {
        return this.list.items.slice(this.from, this.to)
      },

      queriedData() {
        if (!this.searchQuery) {
          this.pagination.total = this.list.items.length
          return this.pagedData
        }
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
        this.pagination.total = this.list.items.length
        return this.list.items.length
      }
    },
    mounted: function () {
       this.fetchList();

    }

  }

</script>
<style>
</style>

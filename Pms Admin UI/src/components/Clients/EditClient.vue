<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Edit Client ({{client.name}})
            </h4>
          </div>
          <hr />

        </form>
        <vue-tabs class="card-content">
          <v-tab title="Client Detail">
            <div class="row">
              <input type="hidden" v-model="client.id" />
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Name</label>
                  <input type="text" v-validate="modelValidations.name" name="name" placeholder="Enter Name" v-model="client.name" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="name.invalid">
                    {{ getError('name') }}
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Email</label>
                  <input type="email" v-validate="modelValidations.email" @keyup="validateEmail" name="email" v-model="client.email" placeholder="Enter Email" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="email.invalid">
                    {{ getError('email') }}
                  </small>
                  
                  
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Mobile</label>
                  <input type="text" v-validate="modelValidations.mobile" @keyup="regexPhoneNumber" name="mobile" v-model="client.mobile" placeholder="Enter Mobile" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="mobile.invalid">
                    {{ getError('mobile') }}
                  </small>
                  <br />
                  <small class="text-danger tabEditValidmsg" v-if="!phoneValid">
                    Enter Valid Mobile Number
                  </small>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Address</label>
                  <input type="text" v-validate="modelValidations.address1" name="address1" placeholder="Enter Address" v-model="client.address1" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="address1.invalid">
                    {{ getError('address1') }}
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Area</label>
                  <input type="text" v-model="client.address2" placeholder="Enter Area" class="form-control">
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Town/City</label>
                  <input type="text" v-validate="modelValidations.city" name="city" v-model="client.city" placeholder="Enter Town/City" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="address1.invalid">
                    {{ getError('city') }}
                  </small>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Postcode</label>
                  <input type="text" v-validate="modelValidations.postCode" @keyup="postCodeCheck" name="postCode" placeholder="Enter Postcode" v-model="client.postCode" class="form-control">
                  <small class="text-danger tabEditValidmsg" v-show="postCode.invalid">
                    {{ getError('postCode') }}
                  </small>
                  <br />
                  <small class="text-danger tabEditValidmsg" v-if="!postCodeValid">
                    Enter Valid Postcode
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">County</label>
                  <input type="text" v-model="client.county" placeholder="Enter County" class="form-control">
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label class="lbltab">Website</label>
                  <input type="text" v-model="client.website" placeholder="Enter Website" class="form-control">
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-lg-6">

              </div>
              <div class="col-lg-6">
                <button type="button" style="float:right" @click.prevent="validate" class="btn btn-fill btn-info">Submit</button>

              </div>
            </div>
          </v-tab>
          <v-tab title="Client User">
            <div class="row">
              <div class="col-lg-6">

              </div>
              <div class="col-lg-6">
                <button style="float:right;" type="button" class="btn btn-fill btn-info " v-on:click="AddUserClick">Add User</button>
              </div>
            </div>
            <br />

            <div class="card-content row">
              <div class="col-sm-6">
                <el-select class="select-default"
                           style="float:left"
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
                <div class="pull-right">
                  <label>
                    
                    <input type="text" class="form-control input-sm" placeholder="Search records" v-model="searchQuery">
                  </label>
                </div>
              </div>

              <div class="card-content table-responsive table-full-width table">
                <el-table :data="queriedData">
                  <el-table-column class="success" label="Display Name" property="displayName"></el-table-column>
                  <el-table-column :min-width="120" label="Email" property="email"></el-table-column>
                  <el-table-column label="Status" property="IsActive">

                    <template slot-scope="props">
                      <span v-if="props.row.isActive == true">Active</span>
                      <span v-if="props.row.isActive == false">InActive</span>

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
              <div class="col-sm-6 pagination-info">
                <p style="float:left" class="category">Showing {{from + 1}} to {{to}} of {{total}} entries</p>
              </div>
              <div class="col-sm-6">
                <p-pagination class="pull-right"
                              v-model="pagination.currentPage"
                              :per-page="pagination.perPage"
                              :total="pagination.total">
                </p-pagination>
              </div>
            </div>

          </v-tab>
        </vue-tabs>


      </div> <!-- end card -->
    </div> <!--  end col-md-6  -->
    <!-- Add User -->
    <Modal v-model="addUserModel" title="Add User">
      <div class="row">
        <div class="col-lg-12">
          <div class="form-group">

            <label>Display Name</label>
            <input type="text" v-validate="modelUser.displayName" name="displayName" class="form-control" v-model="user.displayName" />
            <small class="text-danger" v-show="displayName.invalid">
              Display Name is Required
            </small>
          </div>
          <div class="form-group">
            <label>Email</label>
            <input type="text" v-validate="modelUser.email" name="email" class="form-control" v-model="user.email" />
            <small class="text-danger" v-show="email.invalid">
              Email  is Required
            </small>
          </div>
          <div class="form-group">
            <label>Roles</label>

            <multi-select :options="roles"
                          :selected-options="items"
                          placeholder="select role"
                          @select="onSelect">
            </multi-select>
            <small class="text-danger" v-if="roleerror">
              Please Select Role
            </small>

          </div>
          <div class="form-group">
            <label>Password</label>
            <input type="password" v-validate="modelUser.password" name="password" class="form-control" v-model="user.password" />
            <small class="text-danger" v-show="password.invalid">
              Password is Required
            </small>
          </div>


        </div>
        <div class="row modal-footer">
          <div class="col-sm-12">
            <div class="float-right">
              <button class="btn btn-info" type="button" v-on:click="userSubmit">Submit</button>
              <button class="btn btn-secondary ml-2" type="button" @click="onCancel">Cancel</button>
            </div>
          </div>
        </div>
      </div>
    </Modal>


  </div>
</template>
<script>
  import { mapFields } from 'vee-validate'
  import { mapState } from 'vuex'
  import swal from 'sweetalert2'

  import { getUserByClientId, validateUserName, addUpdateUser} from '@/helpers/user'
  import { addUpdateClient, validateClientEmail, getClientById } from '@/helpers/client'
  import { fetchRolesByClientId } from '@/helpers/role'
  import { Table, TableColumn, Select, Option } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  import { ModelSelect, MultiSelect } from 'vue-search-select'

  import Vue from 'vue'
  import VueTabs from 'vue-nav-tabs'
  Vue.use(VueTabs)
  export default {
    components: {
      PPagination,
      ModelSelect,
      MultiSelect
    },
    data() {
      return {
        isLoading: false,
        roleerror:false,
        errorMessage: null,
        addUserModel:false,
        IsValid: false,
        clientId: 0,
        secret: "123#$%",
        client: {
          id: 0,
          name: '',
          email: '',
          mobile: '',
          address1: '',
          address2:'',
          city: '',
          postCode: '',
          county: '',
          website: '',
        },
        user: {
          clientId: 0,
          id: 0,
          userName: '',
          email: '',
          displayName: '',
          mobile: '',
          password: null,
          roleIds: [],
          isActive: true
        },
        isValidEmail: true,
        isValidUserName:true,
        modelValidations: {
          email: {
            required: true,
            email: true
          },
          name: {
            required: true,

          },
          mobile: {
            required: true,

          },
          address1: {
            required: true,

          },
          postCode: {
            required: true,

          },
          city: {
            required: true,

          }
        },
        modelUser:{
          email: {
            required: true,
            email:true

          },
          displayName: {
            required:true
          },
          password: {
            required:true
          },
   
          
      },
        userList: [],
        pagination: {
          perPage: 5,
          currentPage: 1,
          perPageOptions: [5, 10, 25, 50],
          total: 0
        },
        searchQuery: '',
        propsToSearch: ['name', 'email'],
        roles: [],
        roleadd: [],
        providers: [],
        items: [],
        lastSelectItem: {},
        phoneValid: true,
        postCodeValid: true,
      }
    },
    mounted() {
      
      let id = this.$route.params.id;
      var dId = this.aesDecript(id);
      var str = parseInt(dId.slice(0, -1));
      this.getClientById(str).then(res => {
        this.clientId = str;
        if (res) this.client = res
        this.fetchList(str);

        //this.getUserByClientId(id).then(res1 => {

        //  console.log(res1);
        //  this.userList = res1.items;
        //});
      })
    },
    methods: {
      addUpdateClient: addUpdateClient,
      validateClientEmail: validateClientEmail,
      getClientById: getClientById,
      getUserByClientId: getUserByClientId,
      validateUserName: validateUserName,
      addUpdateUser: addUpdateUser,
      fetchRolesByClientId: fetchRolesByClientId,
      async submit() {
        try {

          if (this.isValidEmail == false) {
            swal({
              title: 'Warning',
              text: `Email is already in use.`,
              type: 'warning',
              confirmButtonClass: 'btn btn-success btn-fill',
              buttonsStyling: false
            })
            return;
          }

          const isValidForm = this.IsValid;

          if (isValidForm) {

           
            await this.addUpdateClient(this.client)
              .then(res => {
                if (res != null || res != undefined)
                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Updated</b> </br> Client Updated Successfully</span>`
                      },
                      icon: 'alert-with-icon',
                      horizontalAlign: 'right',
                      verticalAlign: 'top',
                      type: 'info'
                    })

                  this.$router.push('/clients')
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
        }
        finally {
          this.isSending = false
        }
      },
      onCancel() {
        this.$router.push('/clients')
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
      async userSubmit() {
        try {

          //if (this.u == false) {
          //  swal({
          //    title: 'Worng',
          //    text: `UserName Or Email is already in use.`,
          //    type: 'warning',
          //    confirmButtonClass: 'btn btn-success btn-fill',
          //    buttonsStyling: false
          //  })
          //  return;
          //}
          debugger
          //const isValidForm = this.IsValid;
          this.user.clientId = parseInt(this.clientId);
          this.user.userName = this.user.email;

          await this.addUpdateUser(this.user)
            .then(res => {
              if (res != null || res != undefined)
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> User Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })
              this.fetchList(this.clientId);
              this.addUserModel = false;
              /*this.$router.push('/users')*/
            })
            .catch(e => {
              this.errorMessage = e
              swal({
                title: 'Warng',
                text: this.errorMessage,
                type: 'warning',
                confirmButtonClass: 'btn btn-success btn-fill',
                buttonsStyling: false
              })
            })
         
        }
        finally {
          this.isSending = false
        }
      },
      fetchList(id) {

        this.$store.dispatch('user/fetchclientuser', {
          id:id
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
      validateEmail(event) {
        const value = event.target.value
        this.validateClientEmail(this.client.id, value).then(res => {
          if (res == true) {
            this.isValidEmail = true;
          }
          else {
            this.isValidEmail == false;
          }
          console.log(res);
        }).catch(e => {
          this.isValidEmail == false;

          this.errorMessage = e
          swal({
            title: 'Warning',
            text: this.errorMessage,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })
        })
      },
      validateUser(event) {
        const value = event.target.value
        this.validateUserName(0, value).then(res => {
          if (res == true) {
            this.isValidUserName = true;
          }
          else {
            this.isValidUserName == false;
          }
          console.log(res);
        }).catch(e => {
          this.isValidUserName == false;

          this.errorMessage = e
          swal({
            title: 'Warning',
            text: this.errorMessage,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })
        })
      },

      AddUserClick() {
        this.addUserModel = true;
        this.getRole();
      },
            getRole() {
        this.roles = [];
        this.items = [];
              this.clinterror = false;
              this.fetchRolesByClientId(this.clientId)
          .then(res => {
            //this.providers = res
            // console.log(res);
            res.forEach((value, index) => {
              this.roles.push({ value: value.id, text: value.name });
            });
          })
          .catch(ex => {
            this.errorMessage = ex
            swal({
              title: 'Warning',
              text: this.errorMessage,
              type: 'warning',
              confirmButtonClass: 'btn btn-success btn-fill',
              buttonsStyling: false
            })
          })
      },
      onSelect(items, lastSelectItem) {
        this.user.roleIds = [];
        this.items = items
        items.forEach((value, index) => {
          this.user.roleIds.push(value.value);
        });
        if (this.user.roleIds != '[]') {
          this.roleerror = false;
        }
        console.log(this.user.roleIds);
        //this.user.roleIds.push(lastSelectItem.value )
        //this.user.roleIds = lastSelectItem
        this.lastSelectItem = lastSelectItem
      },
      handleEdit(index, row) {
        var newId = this.aesEncrypt1(row.id + 'a');
        this.$router.push(`/users/edit/${newId}`)

      },
      regexPhoneNumber(event) {
        const regexPhoneNumber = /^((\+)44|0)[1-9](\d{2}){4}$/;
        const value = event.target.value

        if (value.match(regexPhoneNumber)) {
          this.IsValid = true;
          this.phoneValid = true;
          return true;
        } else {
          this.IsValid = false;
          this.phoneValid = false;

          return false;
        }
      },
      postCodeCheck(event) {
        const regexp = /^[A-Z]{1,2}[0-9RCHNQ][0-9A-Z]?\s?[0-9][ABD-HJLNP-UW-Z]{2}$|^[A-Z]{2}-?[0-9]{4}$/;
        const value = event.target.value

        if (regexp.test(value)) {
          this.IsValid = true;
          this.postCodeValid = true;
          return true;
        }
        else {

          this.IsValid = false;
          this.postCodeValid = false;
          return false;
        }

      },
      aesDecript(txt) {
        var CryptoJS = require("crypto-js");

        var bytes = CryptoJS.AES.decrypt(txt, this.secret);
        var originalText = bytes.toString(CryptoJS.enc.Utf8);
        return originalText;
      }
    },
    computed: {
      ...mapState({
        list: state => state.user.list,
      }),
      ...mapFields(['email', 'address1', 'name', 'postCode', 'city', 'mobile', 'displayName','password']),
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

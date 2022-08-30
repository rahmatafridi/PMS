<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add User
            </h4>
          </div>
          <hr />
          <div class="card-content">

            <div class="card-content" >

              <h3>Client Area</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4" v-if="clientId ==0">
                  <div class="form-group">
                    <label>Client</label>
                    <model-select :options="clients"
                                  v-model="user.clientId"
                                  @input="getRole"
                                  required="true"
                                  placeholder="select client">
                    </model-select>
                    <small class="text-danger" v-if="clinterror">
                      Please Select Client
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
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

                </div>

              </div>
            </div>
            <div class="card-content">

              <h3>User Detail</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Display Name</label>
                    <input type="text" v-validate="modelValidations.displayName" name="displayName" placeholder="Enter Display Name" v-model="user.displayName" class="form-control">
                    <small class="text-danger" v-show="displayName.invalid">
                      {{ getError('displayName') }}
                    </small>
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Email</label>
                    <input type="email" v-validate="modelValidations.email" name="email" v-model.number="user.email" placeholder="Enter Email" class="form-control">
                    <small class="text-danger" v-show="email.invalid">
                      {{ getError('email') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>UserName</label>
                    <input type="text" v-validate="modelValidations.userName" name="address1" v-model="user.userName" placeholder="Enter UserName" class="form-control">
                    <small class="text-danger" v-show="userName.invalid">
                      {{ getError('userName') }}
                    </small>
                  </div>
                </div>

              </div>
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Password</label>
                    <input type="password" v-validate="modelValidations.password" name="password" placeholder="Enter Password" v-model="user.password" class="form-control">
                    <small class="text-danger" v-show="password.invalid">
                      {{ getError('password') }}
                    </small>
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Mobile</label>
                    <input type="text" v-validate="modelValidations.mobile" name="mobile" v-model="user.mobile" placeholder="Enter Mobile" class="form-control">
                    <small class="text-danger" v-show="mobile.invalid">
                      {{ getError('mobile') }}
                    </small>
                  </div>
                </div>
                <div class="col-lg-4">
                  <div class="form-group" style="margin-top:30px;">
                    <p-checkbox :checked="user.isActive" v-model="user.isActive">IsActive</p-checkbox>

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
            </div>
            </div>
</form>
      </div> <!-- end card -->
    </div> <!--  end col-md-6  -->

  </div>
</template>
<script>
  //import { mdiEye, mdiEyeOff, mdiLockReset } from '@mdi/js'
  import { mapFields } from 'vee-validate'
  import swal from 'sweetalert2'

  import {
    addUpdateUser, getUserById, validateUserName, validateUserEmail
    , getRolesByUserId
  } from '@/helpers/user'
  import { getClients } from '@/helpers/client'
  import { fetchRolesByClientId } from '@/helpers/role'
  import { ModelSelect, MultiSelect  } from 'vue-search-select'
  export default {
    components: {
      ModelSelect,
      MultiSelect

    },
    data() {
      return {
        isLoading: false,
        IsValid:false,
        errorMessage: null,
        clinterror: false,
        roleerror: false,
        isActive:false,
        user : {
          clientId:0,
          id: 0,
          userName: '',
          email: '',
          displayName: '',
          mobile: '',
          password: null,
          roleIds: [],
          isActive:false
        },
        modelValidations: {
          userName: {
            required:true,
          },
          email: {
            required: true,
            email: true
          },
          displayName: {
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
          password: {
            required:true
          }
        },
        roles: [],
        roleadd:[],
        clients: [],
        providers: [],
        items: [],
        lastSelectItem: {},
        clientId:''

      }
    },
    mounted() {
      let val = "";
      this.clientId = window.localStorage.getItem("clientId");
      if (this.clientId == 0) {

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
            })
          })
      }
      if (this.clientId != 0) {
        this.user.clientId = parseInt(this.clientId);
        this.getRole();
      }
    },
    methods: {
      addUpdateUser: addUpdateUser,
      getUserById: getUserById,
      validateUserName: validateUserName,
      validateUserEmail: validateUserEmail,
      getClients: getClients,
      fetchRolesByClientId: fetchRolesByClientId,
      getRolesByUserId: getRolesByUserId,

      async submit() {
        try {
          if (this.user.clientId == 0) {
            this.clinterror = true;
            //alert("test");
            return;
          }
          if (this.user.roleIds.length <= 0) {
            this.roleerror = true;
            return;
          }
          //if (this.isActive == true) {
          //  this.user.isActive = 1;
          //}
          //if (this.isActive == false) {
          //  this.user.isActive = 0;
          //}
          const isValidForm = this.IsValid;
          if (isValidForm) {

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

                  this.$router.push('/users')
              })
              .catch(e => {

                this.errorMessage = e
                swal({
                  title: 'Warning',
                  text: this.errorMessage,
                  type: 'warning',
                  confirmButtonClass: 'btn btn-success btn-fill',
                  buttonsStyling: false
                })
              })
          }
        } finally {
          //
        }
      },
      onCancel() {
        this.$router.push('/users')
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

      getRole() {
        this.roles = [];
        this.items = [];
        this.clinterror = false;
        this.fetchRolesByClientId(this.user.clientId)
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
    },
    computed: {
      ...mapFields(['email', 'userName', 'displayName', 'password','mobile']),
      //...mapState({
      //  list: state => state.client.list,
      //}),


    },

  }</script>

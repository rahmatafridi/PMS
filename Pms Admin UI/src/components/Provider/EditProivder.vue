<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="col-lg-12 col-md-12">
            <div class="card">
              <div class="card-header">
                <h4 class="card-title">Provider Edit</h4>
              </div>
              <vue-tabs class="card-content">
                <v-tab title="Detail">
                  <div class="row" v-if="clientId ==0">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label style="margin-right:85%;">Client</label>

                        <model-select :options="clients"
                                      v-model="provider.clientId"
                                      @input="getRole"
                                      placeholder="select client">
                        </model-select>
                        <small class="text-danger" v-if="clinterror">
                          Please Select Client
                        </small>

                      </div>

                    </div>

                  </div>
                  <hr />
                  <div class="row">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Name</label>
                        <input type="text" v-validate="modelValidations.name" name="name" placeholder="Enter Name" v-model="provider.name" class="form-control">
                        <small class="text-danger" v-show="name.invalid">
                          {{ getError('name') }}
                        </small>
                      </div>

                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Email</label>
                        <input type="email" v-validate="modelValidations.email" name="email" v-model="provider.email" placeholder="Enter Email" class="form-control">
                        <small class="text-danger" v-show="email.invalid">
                          {{ getError('email') }}
                        </small>
                      </div>

                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Mobile</label>
                        <input type="text" v-validate="modelValidations.mobile" @keyup="regexPhoneNumber" name="mobile" v-model="provider.mobile" placeholder="Enter Mobile" class="form-control">
                        <small class="text-danger" v-show="mobile.invalid">
                          {{ getError('mobile') }}
                        </small>
                        <br />
                        <small class="text-danger" v-if="!phoneValid">
                          Enter Valid Mobile Number
                        </small>
                      </div>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Address</label>
                        <input type="text" v-validate="modelValidations.address1" name="address1" placeholder="Enter Address" v-model="provider.address1" class="form-control">
                        <small class="text-danger" v-show="address1.invalid">
                          {{ getError('address1') }}
                        </small>
                      </div>
                    </div>
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Town/City</label>
                        <input type="text" v-validate="modelValidations.city" name="city" v-model="provider.city" placeholder="Enter Town/City" class="form-control">
                        <small class="text-danger" v-show="address1.invalid">
                          {{ getError('city') }}
                        </small>
                      </div>
                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Postcode</label>
                        <input type="text" v-validate="modelValidations.postCode" @keyup="postCodeCheck" name="postCode" placeholder="Enter Postcode" v-model="provider.postcode" class="form-control">
                        <small class="text-danger" v-show="postCode.invalid">
                          {{ getError('postCode') }}
                        </small>
                        <br />
                        <small class="text-danger" v-if="!postCodeValid">
                          Enter Valid Postcode
                        </small>
                      </div>

                    </div>
                  </div>
                  <div class="row">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">County</label>
                        <input type="text" v-model="provider.county" placeholder="Enter County" class="form-control">
                      </div>

                    </div>
                    <div class="col-lg-4">
                      <div class="form-group" style="margin-top:30px; float:left;">
                        <p-checkbox :checked="user.isActive" v-model="user.isActive">IsActive</p-checkbox>

                      </div>
                    </div>
                  </div>
                </v-tab>
                <v-tab title="User Detail">
                  <div class="row">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label style="margin-right:85%;" >Roles</label>

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
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Display Name</label>
                        <input type="text" v-validate="modelValidations.displayName" name="displayName" placeholder="Enter Display Name" v-model="user.displayName" class="form-control">
                        <small class="text-danger" v-show="displayName.invalid">
                          {{ getError('displayName') }}
                        </small>
                      </div>

                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Email</label>
                        <input type="email" v-validate="modelValidations.email" name="email" v-model.number="user.email" placeholder="Enter Email" class="form-control">
                        <small class="text-danger" v-show="useremail.invalid">
                          {{ getError('email') }}
                        </small>
                      </div>

                    </div>

                  </div>
                  <div class="row">
                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">UserName</label>
                        <input type="text" v-validate="modelValidations.userName" name="address1" v-model="user.userName" placeholder="Enter UserName" class="form-control">
                        <small class="text-danger" v-show="userName.invalid">
                          {{ getError('userName') }}
                        </small>
                      </div>
                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Password</label>
                        <input type="password" v-validate="modelValidations.password" name="password" placeholder="Enter Password" v-model="user.password" class="form-control">
                        <small class="text-danger" v-show="password.invalid">
                          {{ getError('password') }}
                        </small>
                      </div>

                    </div>

                    <div class="col-lg-4">
                      <div class="form-group">
                        <label class="lbltab">Mobile</label>
                        <input type="text" v-validate="modelValidations.mobile" name="mobile" @keyup="regexPhoneNumber1" v-model="user.mobile" placeholder="Enter Mobile" class="form-control">
                        <small class="text-danger" v-show="mobile.invalid">
                          {{ getError('mobile') }}
                        </small>
                        <br />
                        <small class="text-danger" v-if="!phoneValid1">
                          Enter Valid Mobile Number
                        </small>
                      </div>
                    </div>

                  </div>

                </v-tab>
                <div class="row">
                  <div class="col-lg-6">

                  </div>
                  <div class="col-lg-6">
                    <button type="button" style="float:right" @click.prevent="validate" class="btn btn-fill btn-info">Submit</button>

                  </div>
                </div>

              </vue-tabs>
            </div>
          </div>

        </form>
      </div> <!-- end card -->
    </div> <!--  end col-md-6  -->

  </div>
</template>
<script>
  import { mapFields } from 'vee-validate'
  import { mapState } from 'vuex'
  import { addUpdateProvider, getProviderById, validateProviderEmail, getProviderUserById, addProviderUser}
    from '@/helpers/provider'
  import { getClients } from '@/helpers/client'
  import { addUpdateUser, getUserById, getRolesByUserId } from '@/helpers/user'
  import { fetchRolesByClientId, getRoleById } from '@/helpers/role'
  import swal from 'sweetalert2'

  import { ModelSelect, MultiSelect } from 'vue-search-select'
  import Vue from 'vue'
  import VueTabs from 'vue-nav-tabs'
  Vue.use(VueTabs)
  export default {
    components: {
      ModelSelect,
      MultiSelect

    },
    data() {
      return {
        isLoading: false,
        IsValid: false,
        clinterror: false,
        roleerror: false,
        phoneValid1: true,
        phoneValid: true,
        postCodeValid: true,
        errorMessage: null,

        provider: {
          name: '',
          email: '',
          mobile: '',
          address1: '',
          clientId: 0,
          city: '',
          postCode: '',
          county: '',
          website: '',
          id: 0,
          isActive:false,


        },
        user: {
          userName: '',
          email: '',
          displayName: '',
          mobile: '',
          password: null,
          roleIds: [],
          id: 0,
          clientId: 0,
          isActive: false
        },
        providerUser: {
          providerId: '',
          userId: '',
          id: 0
        },

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

          },
          user: {
            email: {
              required: true
            },
            userName: {
              required: true
            },
            password: {
              required: true
            },
            displayName: {
              required: true
            }
          }
        },
        roles: [],

        clients: [],
        items: [],
        lastSelectItem: {},
        userId:0,
        clientId:''

      }
    },
    mounted() {

    },
    methods: {
      addUpdateProvider: addUpdateProvider,
      getProviderById: getProviderById,
      validateProviderEmail: validateProviderEmail,
      getClients: getClients,
      getProviderUserById: getProviderUserById,
      addUpdateUser: addUpdateUser,
      getUserById: getUserById,
      getRolesByUserId: getRolesByUserId,
      fetchRolesByClientId: fetchRolesByClientId,
      addProviderUser: addProviderUser,
      getRoleById: getRoleById,


      async submit() {
        try {
          if (this.user.clientId == 0) {
            this.clinterror = true;
            //alert("test");
            return;
          }
          const isValidForm = this.IsValid;
          if (isValidForm) {
            this.provider.clientId = this.clientId
            this.user.clientId = this.clientId;
            await this.addUpdateProvider(this.provider)
              .then(res => {
                if (res != null || res != undefined) {
                  this.addUpdateUser(this.user).then(res1 => {
                    if (res1 != null || res1 != undefined) {
                      this.$notify(
                        {
                          component: {
                            template: `<span> <b>Updated</b> </br> Provider Updated Successfully</span>`
                          },
                          icon: 'alert-with-icon',
                          horizontalAlign: 'right',
                          verticalAlign: 'top',
                          type: 'info'
                        })


                      this.$router.push('/providers')
                      //this.providerUser.userId = res1.id;
                      //this.providerUser.providerId = res.id;
                      //this.providerUser.id = 0;
                      //this.addProviderUser(this.providerUser).then(res2 => {
                      //  this.$router.push('/providers')
                      //})
                    }
                  });
                  // this.$router.push('/providers')
                }
              })
              .catch(e => {
                this.errorMessage = e;
                swal({
                  title: 'Warning',
                  text: this.errorMessage,
                  type: 'warning',
                  confirmButtonClass: 'btn btn-success btn-fill',
                  buttonsStyling: false
                })
              })
          }

        }
        finally {
          this.isSending = false
        }
      },
      onCancel() {
        this.$router.push('/proivders')
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
      fetchList() {

        this.$store.dispatch('client/fetchList').catch(e => {
          this.errorMessage = e
          swal({
            title: 'Warning',
            text: this.errorMessage,
            type: 'warning',
            confirmButtonClass: 'btn btn-success btn-fill',
            buttonsStyling: false
          })        })
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
      getRole() {


        this.fetchRolesByClientId(this.provider.clientId)
          .then(res => {
            //this.providers = res
            console.log(res);
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
      regexPhoneNumber1(event) {
        const regexPhoneNumber = /^((\+)44|0)[1-9](\d{2}){4}$/;
        const value = event.target.value

        if (value.match(regexPhoneNumber)) {
          this.IsValid = true;
          this.phoneValid1 = true;
          return true;
        } else {
          this.IsValid = false;
          this.phoneValid1 = false;

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

    },
    computed: {
      ...mapFields(['displayName', 'email', 'address1', 'name', 'postCode', 'city', 'mobile', 'useremail', 'userName', 'password']),
      ...mapState({
        list: state => state.client.list
      }),

      //selectClients() {
      //  return this.clients
      //},


    },
    mounted: function () {
      let id = this.$route.params.id;
      this.clientId = window.localStorage.getItem("clientId");
      var dId = this.aesDecript1(id);
      var str = parseInt(dId.slice(0, -1));
      if (str != null) {
        if (this.clientId == 0) {
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
              })
            })
        }
        if (this.clientId != 0) {
          this.provider.clientId = this.clientId;
        }
        this.getProviderById(str).then(res => {
          if (res) {
            this.provider = res;
            
            this.getProviderUserById(this.provider.id).then(res => {
            
              this.userId = res.userId;
              this.providerUser = res;
              if (this.userId != 0) {
                this.getUserById(this.userId).then(res1 => {
                  this.user = res1;
                })

                this.getRolesByUserId(this.userId).then(res2 => {
                  this.user.roleIds = res2
                  console.log(this.user.roleIds);
                  this.user.roleIds.forEach((value, index) => {
                    this.getRoleById(value).then(res3 => {
                      this.items.push({ value: res3.id, text: res3.name });

                    })
                  })
                })
              }

            });
          }
        })
      }

    }
  }</script>

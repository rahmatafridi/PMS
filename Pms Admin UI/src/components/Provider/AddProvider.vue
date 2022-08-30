<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add Provider 
            </h4>
          </div>
          <hr />
          <div class="card-content">
            <div class="card-content" v-if="clientId ==0">

              <h3>Client Area</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4" >
                  <div class="form-group">
                    <label>Client</label>

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
            </div>
            <div class="card-content">

              <h3>Provider Detail</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Name</label>
                    <input type="text" v-validate="modelValidations.name" name="name" placeholder="Enter Name" v-model="provider.name" class="form-control">
                    <small class="text-danger" v-show="name.invalid">
                      {{ getError('name') }}
                    </small>
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Email</label>
                    <input type="email" v-validate="modelValidations.email" name="email" v-model="provider.email" placeholder="Enter Email" class="form-control">
                    <small class="text-danger" v-show="email.invalid">
                      {{ getError('email') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Mobile</label>
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
                    <label>Address</label>
                    <input type="text" v-validate="modelValidations.address1" name="address1" placeholder="Enter Address" v-model="provider.address1" class="form-control">
                    <small class="text-danger" v-show="address1.invalid">
                      {{ getError('address1') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Area</label>
                    <input type="text" v-model="provider.area" placeholder="Enter Area" class="form-control">
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Town/City</label>
                    <input type="text" v-validate="modelValidations.city" name="city" v-model="provider.city" placeholder="Enter Town/City" class="form-control">
                    <small class="text-danger" v-show="address1.invalid">
                      {{ getError('city') }}
                    </small>
                  </div>
                </div>
              </div>
              <div class="row">

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Postcode</label>
                    <input type="text" v-validate="modelValidations.postCode"  @keyup="postCodeCheck" name="postCode" placeholder="Enter Postcode" v-model="provider.postcode" class="form-control">
                    <small class="text-danger" v-show="postCode.invalid">
                      {{ getError('postCode') }}
                    </small>
                    <br />
                    <small class="text-danger" v-if="!postCodeValid">
                      Enter Valid Postcode
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>County</label>
                    <input type="text" v-model="provider.county" placeholder="Enter County" class="form-control">
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group" style="margin-top:30px;">
                    <p-checkbox :checked="user.isActive" v-model="user.isActive">IsActive</p-checkbox>

                  </div>
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
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Display Name</label>
                  <input type="text" v-validate="modelValidations.user.displayName" name="displayName" placeholder="Enter Display Name" v-model="user.displayName" class="form-control">
                  <small class="text-danger" v-show="displayName.invalid">
                    {{ getError('displayName') }}
                  </small>
                </div>

              </div>

              <div class="col-lg-4">
                <div class="form-group">
                  <label>Email</label>
                  <input type="email" v-validate="modelValidations.user.email" name="email" v-model.number="user.email" placeholder="Enter Email" class="form-control">
                  <small class="text-danger" v-show="email.invalid">
                    {{ getError('email') }}
                  </small>
                </div>

              </div>

            </div>
            <div class="row">
              <div class="col-lg-4">
                <div class="form-group">
                  <label>UserName</label>
                  <input type="text" v-validate="modelValidations.user.userName" name="address1" v-model="user.userName" placeholder="Enter UserName" class="form-control">
                  <small class="text-danger" v-show="userName.invalid">
                    {{ getError('userName') }}
                  </small>
                </div>
              </div>

              <div class="col-lg-4">
                <div class="form-group">
                  <label>Password</label>
                  <input type="password" v-validate="modelValidations.user.password" name="password" placeholder="Enter Password" v-model="user.password" class="form-control">
                  <small class="text-danger" v-show="password.invalid">
                    {{ getError('password') }}
                  </small>
                </div>

              </div>

              <div class="col-lg-4">
                <div class="form-group">
                  <label>Mobile</label>
                  <input type="text" v-validate="modelValidations.user.mobile"  @keyup="regexPhoneNumber1" name="mobile" v-model="user.mobile" placeholder="Enter Mobile" class="form-control">
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



            <div class="row">
              <div class="col-lg-6">

              </div>
              <div class="col-lg-6">
                <button type="button" style="float:right" @click.prevent="validate" class="btn btn-fill btn-info">Submit</button>

              </div>
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
  import swal from 'sweetalert2'

  import { addUpdateProvider, getProviderById, validateProviderEmail, addProviderUser }
    from '@/helpers/provider'
  import { getClients } from '@/helpers/client'
  import { ModelSelect, MultiSelect } from 'vue-search-select'
  import { fetchRolesByClientId, getRoleById } from '@/helpers/role'
  import { addUpdateUser } from '@/helpers/user'
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
          clientId:0,
          city: '',
          postCode: '',
          county: '',
          website: '',
          
         
        },
        user: {
          userName: '',
          email: '',
          displayName: '',
          mobile: '',
          password: null,
          roleIds: [],
          id: 0,
          clientId:0
        },
        providerUser: {
          providerId: '',
          userId: '',
          id:0
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
        clientId:''

      }
    },
    mounted() {
    
      this.clientId = window.localStorage.getItem("clientId");
      if (this.clientId == 0) {
        var val = "";
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
        this.provider.clientId = parseInt(this.clientId);
        this.getRole();
      }
    //  this.getRole();
    },
    methods: {
      addUpdateProvider: addUpdateProvider,
      getProviderById: getProviderById,
      validateProviderEmail: validateProviderEmail,
      getClients: getClients,
      fetchRolesByClientId: fetchRolesByClientId,
      getRoleById: getRoleById,
      addUpdateUser: addUpdateUser,
      addProviderUser: addProviderUser,
      async submit() {
        try {
          if (this.provider.clientId == 0) {
            this.clinterror = true;
            //alert("test");
            return;
          }
          const isValidForm = this.IsValid;
          if (isValidForm) {
            /* this.provider.clientId = 3;*/
            this.user.clientId = this.provider.clientId;
            await this.addUpdateProvider(this.provider)
              .then(res => {
                if (res != null || res != undefined) {
                   this.addUpdateUser(this.user).then(res1 => {
                     if (res1 != null || res1 != undefined) {
                      this.providerUser.userId = res1.id;
                       this.providerUser.providerId = res.id;
                       this.providerUser.id = 0;
                       this.addProviderUser(this.providerUser).then(res2 => {

                         this.$notify(
                           {
                             component: {
                               template: `<span> <b>Added</b> </br> Provider Added Successfully</span>`
                             },
                             icon: 'alert-with-icon',
                             horizontalAlign: 'right',
                             verticalAlign: 'top',
                             type: 'info'
                           })

                        this.$router.push('/providers')
                      })
                    }
                  });
                  console.log(res);
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
      onSelect(items, lastSelectItem) {
        this.user.roleIds = [];
        this.items = items
        items.forEach((value, index) => {
          this.user.roleIds.push(value.value);
        });
        if (this.user.roleIds != '[]') {
          this.roleerror = false;
        }
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
            })          })
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
      ...mapFields(['displayName','email', 'address1', 'name', 'postCode', 'city', 'mobile', 'useremail','userName','password']),
      ...mapState({
      //  clientId: state => state.loign.clientId
      }),


    },

  }
</script>

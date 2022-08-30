<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form >
          <div class="card-header">
            <h4 class="card-title">
              Add Client
            </h4>
          </div>
          <hr />
          <div class="card-content">
            <div class="row">
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Name</label>
                  <input type="text" v-validate="modelValidations.name" name="name" placeholder="Enter Name" v-model="client.name" class="form-control">
                  <small class="text-danger" v-show="name.invalid">
                    {{ getError('name') }}
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Email</label>
                  <input type="email" v-validate="modelValidations.email" @keyup="validateEmail" name="email" v-model="client.email" placeholder="Enter Email" class="form-control">
                  <small class="text-danger" v-show="email.invalid">
                    {{ getError('email') }}
                  </small>

                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Mobile</label>
                  <input type="text" v-validate="modelValidations.mobile" @keyup="regexPhoneNumber" name="mobile" v-model="client.mobile" placeholder="+44777777776" class="form-control">
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
                  <input type="text" v-validate="modelValidations.address1" name="address1" placeholder="Enter Address" v-model="client.address1" class="form-control">
                  <small class="text-danger" v-show="address1.invalid">
                    {{ getError('address1') }}
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Area</label>
                  <input type="text" v-model="client.address2" placeholder="Enter Area" class="form-control">
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Town/City</label>
                  <input type="text" v-validate="modelValidations.city" name="city" v-model="client.city" placeholder="Enter Town/City" class="form-control">
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
                  <input type="text" v-validate="modelValidations.postCode" @keyup="postCodeCheck" name="postCode" placeholder="Enter Postcode" v-model="client.postCode" class="form-control">
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
                  <input type="text" v-model="client.county" placeholder="Enter County" class="form-control">
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Website</label>
                  <input type="text" v-model="client.website" placeholder="Enter Website" class="form-control">
                </div>
              </div>
            </div>
            <div class="card-content">
              <h3>Default User</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Display Name</label>
                    <input type="text" v-validate="modelValidations.displayName"  name="displayName" placeholder="Enter Dispaly Name" v-model="client.dispalyName" class="form-control">
                    <small class="text-danger" v-show="displayName.invalid">
                     Display Name Required
                    </small>
                  

                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Email</label>
                    <input type="email" v-validate="modelValidations.useremail" name="useremail" v-model="client.useremail" placeholder="Enter Email" class="form-control">
                    <small class="text-danger" v-show="useremail.invalid">
                      User Email field must be a valid email
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Password</label>
                    <input type="password" v-model="client.password" placeholder="Password" class="form-control">
                  </div>
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
  import swal from 'sweetalert2'

  import { addUpdateClient, validateClientEmail, getClientById } from '@/helpers/client'
  export default {
    data() {
      return {
        isLoading: false,
        IsValid:false,
        errorMessage: null,
        client: {
          name: '',
          email: '',
          mobile: '',
          address1: '',
          address2:'',
          city: '',
          postCode: '',
          county: '',
          website: '',
          useremail: '',
          displayName: '',
          password:''
        },
        isValidEmail: true,
        phoneValid: true,
        postCodeValid:true,
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
          displayName: {
            required:true
          },
          useremail: {
            required:true
          }
        },
        user: {
          displayName: '',
          useremail: '',
          password:''
        }

      }
    },
    mounted() {
     
    },
    methods: {
      addUpdateClient: addUpdateClient,
      validateClientEmail: validateClientEmail,
      getClientById: getClientById,

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
                        template: `<span> <b>Added</b> </br> Client Added Successfully</span>`
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
                })
              })
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
      validateEmail(event) {
        const value = event.target.value
        this.validateClientEmail(0, value).then(res => {
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
          //swal({
          //  title: 'Warng',
          //  text: this.errorMessage,
          //  type: 'warning',
          //  confirmButtonClass: 'btn btn-success btn-fill',
          //  buttonsStyling: false
          //})
        })
      }
    },
    computed: {
      ...mapFields(['email', 'address1', 'name', 'postCode', 'city', 'mobile', 'useremail','displayName'])

    },

  }
</script>

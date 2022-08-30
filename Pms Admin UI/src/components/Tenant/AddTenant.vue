<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add Tenant
            </h4>
          </div>
          <hr />
          <div class="card-content">
            <div class="card-content">

              <h3>Client Area</h3>
              <hr />
              <div class="row">
                <div class="col-lg-4" v-if="clientId == 0">
                  <div class="form-group">
                    <label>Client</label>
                    <model-select :options="clients"
                                  v-model="tenant.clientId"
                                  @input="getProiver"
                                  placeholder="select client">
                    </model-select>
                    <!--<small class="text-danger" v-show="clientId.invalid">
            {{ getError('name') }}
          </small>-->
                    <!--<select class="form-control" v-model="property.clientId" v-validate="modelValidations.clientId" name="clientId">
            <option v-for="item in list.items" :value="item.id">
              {{item.name}}
            </option>
          </select>-->
                    <!--<input type="text" v-validate="modelValidations.clientId" name="clientId" placeholder="Enter Name" v-model="role.clientId" class="form-control">-->
                    <!--<small class="text-danger" v-show="client.invalid">
            {{ getError('name') }}
          </small>-->
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Provider</label>
                    <model-select :options="providers"
                                  v-model="tenant.providerId"
                                  placeholder="select provider">
                    </model-select>

                  </div>

                </div>

              </div>



            </div>
            <div class="card-content">
              <h3>
                Tenant Information
              </h3>
              <hr />
              <div class="row">

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>First Name</label>
                    <input type="text" v-validate="modelValidations.firstName" name="firstName" placeholder="Enter First Name" v-model="tenant.firstName" class="form-control">
                    <small class="text-danger" v-show="firstName.invalid">
                      {{ getError('firstName') }}
                    </small>
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Last Name</label>
                    <input type="text" v-validate="modelValidations.lastName" name="lastName" v-model.number="tenant.lastName" placeholder="Number Of Last Name" class="form-control">
                    <small class="text-danger" v-show="lastName.invalid">
                      {{ getError('lastName') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>DOB</label>
                    <div class="form-group">
                      <el-date-picker v-model="tenant.dob" type="date" placeholder="DoB"
                                      :picker-options="pickerOptions1" format="dd/MM/yyyy">
                      </el-date-picker>
                    </div>
                    <small class="text-danger" v-show="dob.invalid">
                      {{ getError('dob') }}
                    </small>
                  </div>
                </div>




              </div>
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Gender</label>
                    <select class="form-control" v-validate="modelValidations.gender" name="gender" v-model="tenant.gender">
                      <option v-for="item in genderList" :value="item.id">
                        {{item.title}}
                      </option>
                      
                    </select>
                    <!--<input type="text" v-validate="modelValidations.address2" name="address1" placeholder="Enter Area" v-model="property.address2" class="form-control">-->
                    <small class="text-danger" v-show="gender.invalid">
                      {{ getError('gender') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Ethnicity</label>
                    <select class="form-control" v-validate="modelValidations.ethnicity" name="ethnicity" v-model="tenant.ethnicity">
                      <option v-for="item in ethnicityList" :value="item.id">
                        {{item.title}}
                      </option>
                    </select>
                    <!--<input type="text" v-validate="modelValidations.ethnicity" name="ethnicity" placeholder="Enter Ethnicity" v-model="property.ethnicity" class="form-control">-->
                    <small class="text-danger" v-show="ethnicity.invalid">
                      {{ getError('ethnicity') }}
                    </small>
                  </div>
                </div>

              </div>

            </div>

            <div class="card-content">
              <h3>
                Contact Information
              </h3>
              <hr />
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Mobile</label>

                    <input type="text" v-validate="modelValidations.mobile" @keyup="regexPhoneNumber" name="mobile" placeholder="Enter Ethnicity" v-model="tenant.mobile" class="form-control">
                    <small class="text-danger" v-show="mobile.invalid">
                      {{ getError('mobile') }}
                    </small>
                    <br />
                    <small class="text-danger" v-if="!phoneValid">
                      Enter Valid Mobile Number
                    </small>
                  </div>
                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Email</label>

                    <input type="text" v-validate="modelValidations.email" name="email" placeholder="Enter Ethnicity" v-model="tenant.email" class="form-control">
                    <small class="text-danger" v-show="email.invalid">
                      {{ getError('email') }}
                    </small>
                  </div>
                </div>
              </div>
            </div>
            <div class="card-content">
              <h3>
                Tenancy Information
              </h3>
              <hr />
              <div class="row">
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>NiNumber</label>

                    <input type="text" v-validate="modelValidations.niNumber" name="niNumber" placeholder="Enter NiNumber" v-model="tenant.niNumber" class="form-control">
                    <small class="text-danger" v-show="niNumber.invalid">
                      {{ getError('mobile') }}
                    </small>
                  </div>
                </div>
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>Claim Number</label>

                    <input type="text" v-validate="modelValidations.claimNumber" name="claimNumber" placeholder="Enter Ethnicity" v-model="tenant.claimNumber" class="form-control">
                    <small class="text-danger" v-show="claimNumber.invalid">
                      {{ getError('claimNumber') }}
                    </small>
                  </div>
                </div>

                <div class="col-lg-3">
                  <div class="form-group">
                    <label>Referral Method</label>
                    <select class="form-control" v-model="tenant.referralMethod">
                      <option v-for="item in referralList" :value="item.id">
                        {{item.title}}
                      </option>
                    </select>
                    <!--<input type="text" v-validate="modelValidations.address2" name="address1" placeholder="Enter Area" v-model="property.address2" class="form-control">-->
                    <small class="text-danger" v-show="referralMethod.invalid">
                      {{ getError('referralMethod') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>Local Authority</label>
                    <select class="form-control" v-model="tenant.localAuthority">
                      <option v-for="item in localAuthority" :value="item.id">
                        {{item.title}}
                      </option>
                    </select>
                    <!--<input type="text" v-validate="modelValidations.ethnicity" name="ethnicity" placeholder="Enter Ethnicity" v-model="property.ethnicity" class="form-control">-->
                    <small class="text-danger" v-show="localAuthority.invalid">
                      {{ getError('localAuthority') }}
                    </small>
                  </div>
                </div>
              </div>
            </div>
            <div class="card-content">
              <h3>
                Key Dates
              </h3>
              <hr />
              <div class="row">
                <div class="col-lg-3">
                  <label>
                    Pre Acceptance Inspection Date
                  </label>
                  <div class="form-group">
                    <el-date-picker v-model="tenant.datePreAcceptanceInspection"
                                    type="date" placeholder="Pre Acceptance Inspection"
                                    format="dd/MM/yyyy"
                                    :picker-options="pickerOptions1">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>
                    Support Plan Review Date
                  </label>
                  <div>
                    <el-date-picker v-model="tenant.dateSupportPlanReview" type="date" placeholder=" Support Plan Review"
                                    format="dd/MM/yyyy" :picker-options="pickerOptions1">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>
                    Support Plan Completed Date
                  </label>
                  <div class="form-group">
                    <el-date-picker v-model="tenant.dateSupportPlanCompleted" type="date" placeholder="Support Plan Completed"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>
                    Risk Assessment Review Date
                  </label>
                  <div class="form-group">
                    <el-date-picker v-model="tenant.dateRiskAssessmentReview" type="date" placeholder="Risk Assessment Review"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>

              </div>
              <div class="row">
                <div class="col-lg-3">
                  <label>
                    Risk Assessment Completed Date
                  </label>
                  <div class="form-group">
                    <el-date-picker v-model="tenant.dateRiskAssessmentCompleted" type="date" placeholder="Risk Assessment Completed"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
              </div>
            </div>
            <div class="card-content">
              <h3>
                Existing/Previous Address
              </h3>
              <hr />
              <div class="row">
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>Address</label>

                    <input type="text" v-validate="modelValidations.address1" name="address1" placeholder="Enter Address1" v-model="tenant.address1" class="form-control">
                    <small class="text-danger" v-show="address1.invalid">
                      {{ getError('mobile') }}
                    </small>
                  </div>
                </div>
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>Area</label>

                    <input type="text" v-validate="modelValidations.address2" name="address2" placeholder="Enter Area" v-model="tenant.email" class="form-control">
                    <small class="text-danger" v-show="address2.invalid">
                      area Required
                    </small>
                  </div>
                </div>
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>City</label>

                    <input type="text" v-validate="modelValidations.city" name="city" placeholder="Enter City" v-model="tenant.city" class="form-control">
                    <small class="text-danger" v-show="city.invalid">
                      {{ getError('city') }}
                    </small>
                  </div>
                </div>
                <div class="col-lg-3">
                  <div class="form-group">
                    <label>PostCode</label>

                    <input type="text" v-validate="modelValidations.postCode" name="postCode" placeholder="Enter postCode" v-model="tenant.postCode" class="form-control">
                    <small class="text-danger" v-show="postCode.invalid">
                      {{ getError('postCode') }}
                    </small>
                  </div>
                </div>


              </div>
            </div>
            <div class="card-content">
              <h3>
                Personal Notes

              </h3>
              <hr />
              <div class="row">
                <div class="col-lg-12">
                  <div class="form-group">
                    <label>
                       Notes
                    </label>
                    <textarea  name="notes" placeholder="Enter Note" v-model="form.notes" class="form-control" ></textarea>
                    <!--<small class="text-danger" v-show="address1.invalid">
                      {{ getError('mobile') }}
                    </small>-->
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
  import { addUpdateTenant, getTenantById }
    from '@/helpers/tenant'
  import { getClients } from '@/helpers/client'
  import { getProviders } from '@/helpers/provider'
  import { getOptionByHeader } from '@/helpers/option'
 import { addUpdateNote } from '@/helpers/note'
  import swal from 'sweetalert2'

  import { DatePicker} from 'element-ui'
  import { ModelSelect } from 'vue-search-select'

  export default {
    components: {
      [DatePicker.name]: DatePicker,
      ModelSelect

    },
    data() {
      return {
        isLoading: false,
        IsValid:false,
        errorMessage: null,
        phoneValid: true,

        tenant: {
          id: 0,
          clientId: 0,
          providerId: 0,
    
          city: '',
          postCode: '',
          county: '',
          firstName: '',
          lastName: '',
          gender: 0,
          ethnicity:0,
          claimNumber: '',
          referralMethod: 0,
          localAuthority: 0,
          dateSupportPlanCompleted: '',
          dateRiskAssessmentCompleted: '',
          datePreAcceptanceInspection: '',
          dateRiskAssessmentReview: '',
          dateSupportPlanReview: '',
          niNumber: '',
          dob: '',
          email: '',
          mobile: '',
          address1: '',
          address2: '',
          address3: '',
       
        },
        modelValidations: {
          clinetId: {
            required:true,
          },
          email: {
            required: true,
            email: true
          },
          providerId: {
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
          firstName: {
            required: true,

          },
          lastName: {
            required: true,

          },
          address2: {
            required:true,
          },
          gender: {
            required: true,
          },
          ethnicity: {
            required: true,
          },
          claimNumber: {
            required: true,
          },
          referralMethod: {
            required: true,
          },
          localAuthority: {
            required: true,
          },
          dateSupportPlanCompleted: {
            required: true,
          },
          dateRiskAssessmentCompleted: {
            required: true,
          },
          datePreAcceptanceInspection: {
            required: true,
          },
          dateRiskAssessmentReview: {
            required: true,
          },
          gendateSupportPlanReviewder: {
            required: true,
          },
          niNumber: {
            required: true,
          },
          dob: {
            required: true,
          },
         
        },
        pickerOptions1: {
          shortcuts: [{
            text: 'Today',
            onClick(picker) {
              picker.$emit('pick', new Date())
            }
          },
          {
            text: 'Yesterday',
            onClick(picker) {
              const date = new Date()
              date.setTime(date.getTime() - 3600 * 1000 * 24)
              picker.$emit('pick', date)
            }
          },
          {
            text: 'A week ago',
            onClick(picker) {
              const date = new Date()
              date.setTime(date.getTime() - 3600 * 1000 * 24 * 7)
              picker.$emit('pick', date)
            }
          }]
        },
        clients: [],
        providers: [],
        genderList: [],
        ethnicityList: [],
        referralList: [],
        localAuthority: [],
        form: {
          notes: '',
          noteCategoryId: 0,
          parentId: 0,
          typeId: 0,
        },
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
        this.tenant.clientId = parseInt(this.clientId);
        this.getProiver();
      }
      this.loadOptions();
      this.loadEtitcity();
      this.referrall();
      this.localauthorityy();
    },
    methods: {
      addUpdateTenant: addUpdateTenant,
      getTenantById: getTenantById,
      getClients: getClients,
      getProviders: getProviders,
      getOptionByHeader: getOptionByHeader,
      addUpdateNote: addUpdateNote,

      async submit() {
        try {
          this.tenant.gender = parseInt(this.tenant.gender);
          this.tenant.ethnicity = parseInt(this.tenant.ethnicity);
          const isValidForm = this.IsValid;
          if (isValidForm) {
            await this.addUpdateTenant(this.tenant)
              .then(res => {
                if (this.form.notes != '') {
                  this.form.parentId = res.id;
                  this.form.typeId = 3;
                  this.form.noteCategoryId = 3;
                   this.addUpdateNote(this.form).then(te => {


                  })
                }
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Tenant Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })

                  this.$router.push('/tenants')
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
        this.$router.push('/tenants')
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

      getProiver() {
        this.providers = [];
        this.getProviders("",this.tenant.clientId)
          .then(res => {
            //this.providers = res
            //conosle.log(this.providers);
            res.forEach((value, index) => {
              this.providers.push({ value: value.id, text: value.name });
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
      loadOptions() {
        this.getOptionByHeader("Gender")
          .then(res => {
            //res.forEach((value, index) => {
            //  this.clients.push({ value: value.id, text: value.name });
            //});
            this.genderList = res;
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
            })          })

       
      },
      loadEtitcity() {
        this.getOptionByHeader("Ethnicity")
          .then(res => {
            //res.forEach((value, index) => {
            //  this.clients.push({ value: value.id, text: value.name });
            //});
            this.ethnicityList = res;
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
            })          })
      },
      referrall() {
        this.getOptionByHeader("ReferralMethods")
          .then(res => {
            //res.forEach((value, index) => {
            //  this.clients.push({ value: value.id, text: value.name });
            //});
            this.referralList = res;
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
            })          })
      },
      localauthorityy() {
        this.getOptionByHeader("LocalAuthority")
          .then(res => {
            //res.forEach((value, index) => {
            //  this.clients.push({ value: value.id, text: value.name });
            //});
            this.localAuthority = res;
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


    },
    computed: {
      ...mapFields(['email', 'address1', 'dob', 'dateRiskAssessmentReview', 'datePreAcceptanceInspection', 'dateRiskAssessmentCompleted', 'dateSupportPlanCompleted', 'localAuthority', 'referralMethod', 'claimNumber', 'ethnicity', 'gender', 'lastName', 'firstName', 'postCode', 'city', 'mobile', 'niNumber', 'address2','dateSupportPlanReview']),
      //...mapState({
      //  list: state => state.client.list,
      //}),


    },

  }</script>

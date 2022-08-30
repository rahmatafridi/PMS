<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add Property
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
                                  v-model="property.clientId"
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
                                  v-model="property.providerId"
                                  placeholder="select provider">
                    </model-select>

                  </div>

                </div>

              </div>



            </div>
            <div class="card-content">
              <h3>Property Details</h3>
              <hr />
              <div class="row">

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Title No</label>
                    <input type="text" v-validate="modelValidations.titleNo" name="titleNo" placeholder="Enter Title No" v-model="property.titleNo" class="form-control">
                    <small class="text-danger" v-show="titleNo.invalid">
                      {{ getError('titleNo') }}
                    </small>
                  </div>

                </div>

                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Number Of Rooms</label>
                    <input type="text" v-validate="modelValidations.numberOfRooms" v-on:keypress="NumbersOnly" name="numberOfRooms" v-model.number="property.numberOfRooms" placeholder="Number Of Rooms" class="form-control">
                    <small class="text-danger" v-show="numberOfRooms.invalid">
                      {{ getError('numberOfRooms') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Address</label>
                    <input type="text" v-validate="modelValidations.address1" name="address1" v-model="property.address1" placeholder="Enter Address" class="form-control">
                    <small class="text-danger" v-show="address1.invalid">
                      {{ getError('address1') }}
                    </small>
                  </div>
                </div>




              </div>
              <div class="row">
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Area</label>
                    <input type="text" v-validate="modelValidations.address2" name="address1" placeholder="Enter Area" v-model="property.address2" class="form-control">
                    <small class="text-danger" v-show="address2.invalid">
                      {{ getError('address2') }}
                    </small>
                  </div>

                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Town/City</label>
                    <input type="text" v-validate="modelValidations.city" name="city" v-model="property.city" placeholder="Enter Town/City" class="form-control">
                    <small class="text-danger" v-show="city.invalid">
                      {{ getError('city') }}
                    </small>
                  </div>
                </div>
                <div class="col-lg-4">
                  <div class="form-group">
                    <label>Postcode</label>
                    <input type="text" v-validate="modelValidations.postCode" @keyup="postCodeCheck" name="postCode" placeholder="Enter Postcode" v-model="property.postcode" class="form-control">
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
                    <label>County</label>
                    <input type="text" v-model="property.county" placeholder="Enter County" class="form-control">
                  </div>

                </div>

              </div>
            </div>
            <div class="card-content">
              <h3>Property Dates</h3>
              <hr />
              <div class="row">
                <div class="col-lg-3">
                  <label>SLA Start</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateSlaStart"
                                    type="date" placeholder="Select SLA Start"
                                    format="dd/MM/yyyy"
                                    :picker-options="pickerOptions1">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>SLA End</label>
                  <div>
                    <el-date-picker v-model="property.dateSlaEnd" type="date" placeholder="Select SLA End"
                                    format="dd/MM/yyyy" :picker-options="pickerOptions1">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>Lease Start</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateLeaseStart" type="date" placeholder="Select Lease Start"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>Lease End</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateLeaseEnd" type="date" placeholder="Select Lease End"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>

              </div>
              <div class="row">
                <div class="col-lg-3">
                  <label>Pre Acceptance Inspection</label>
                  <div>
                    <el-date-picker v-model="property.datePreAcceptInsp" type="date" placeholder="Select Pre Acceptance Inspection"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>Inspection Date</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateInspection" type="date" placeholder="Select Inspection Date"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>Exempt Date</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateExempt" type="date" placeholder="Select Exempt Date"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
                <div class="col-lg-3">
                  <label>Quarterly Audit Date</label>
                  <div>
                    <el-date-picker v-model="property.dateQuarterlyAudit" type="date" placeholder="Select Quarterly Audit Date"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
                  </div>
                </div>
              </div>
              <div class="row">
             
                <div class="col-lg-3">
                  <label>Quarterly Inspection</label>
                  <div class="form-group">
                    <el-date-picker v-model="property.dateQuarterlyInsp" type="date" placeholder="Select Quarterly Insp"
                                    :picker-options="pickerOptions1" format="dd/MM/yyyy">
                    </el-date-picker>
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
  import { addUpdateProperty, getPropertyById }
    from '@/helpers/property'
  import { getClients } from '@/helpers/client'
  import { getProviders } from '@/helpers/provider'
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
        property: {
          id: 0,
          clientId: 0,
          providerId: 0,
          address1: '',
          address2: '',
          address3: '',
          city: '',
          postCode: '',
          county: '',
          titleNo: '',
          numberOfRooms: null,
          dateSlaStart: null,
          dateSlaEnd: null,
          dateLeaseStart: null,
          dateLeaseEnd: null,
          datePreAcceptInsp: null,
          dateInspection: null,
          dateExempt: null,
          dateQuarterlyAudit: null,
          dateQuarterlyInsp: null,
        },
        modelValidations: {
          clinetId: {
            required:true,
          },
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
          titleNo: {
            required: true,

          },
          numberOfRooms: {
            required:true,
          },
          address2: {
            required:true
          }
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
        clientId: '',
        postCodeValid: true,

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
        this.property.clientId = parseInt(this.clientId);
        this.getProiver();
      }

    },
    methods: {
      addUpdateProperty: addUpdateProperty,
      getPropertyById: getPropertyById,
      getClients: getClients,
      getProviders: getProviders,
      async submit() {
        try {
          const isValidForm = this.IsValid;
          if (isValidForm) {
            await this.addUpdateProperty(this.property)
              .then(res => {
                if (res != null || res != undefined)
                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Added</b> </br> Property Added Successfully</span>`
                      },
                      icon: 'alert-with-icon',
                      horizontalAlign: 'right',
                      verticalAlign: 'top',
                      type: 'info'
                    })
                  this.$router.push('/property')
              })
              .catch(e => {
                this.errorMessage = e
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

      getProiver() {
        this.providers = [];
        this.getProviders("",this.property.clientId)
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
      NumbersOnly(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
          evt.preventDefault();;
        } else {
          return true;
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
      ...mapFields(['email', 'address1', 'titleNo', 'postCode', 'city', 'mobile', 'numberOfRooms','address2']),
      //...mapState({
      //  list: state => state.client.list,
      //}),


    },

  }</script>

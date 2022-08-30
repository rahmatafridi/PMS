<template>
  <div>
      <div class="col-md-12">
        <div class="card">
          <form>
            <div class="card">
              <div class="card-header">
                <h4 class="card-title">
                  Edit Tenant
                </h4>
              </div>
              <vue-tabs class="card-content">
                <v-tab title="Tenant Detail">
                  <div class="col-lg-4 col-md-12">
                    <div class="card">
                      <div class="card-header">
                        <div class="row">
                          <div class="col-lg-8">
                            <h4 class="card-title">
                              Client Area
                            </h4>
                          </div>
                          <div class="col-lg-4">
                            <span style="float:right"><a class="ti-marker-alt" v-on:click="clientEdit"></a></span>
                          </div>
                        </div>

                      </div>
                      <hr />
                      <div class="card-content">


                        <div class="row" v-if="clientId==0">
                          <div class="col-lg-6">
                            <label>Client</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{clientName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Provider</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{providerName}}</label>
                          </div>
                        </div>

                      </div>
                    </div>

                  </div>

                  <div class="col-lg-4 col-md-12">
                    <div class="card">
                      <div class="card-header">
                        <div class="row">
                          <div class="col-lg-8">
                            <h4 class="card-title">
                              Tenant Information
                            </h4>
                          </div>
                          <div class="col-lg-4">
                            <span style="float:right"><a class="ti-marker-alt" v-on:click="tenantInfoEdit"></a></span>
                          </div>
                        </div>

                      </div>
                      <hr />
                      <div class="card-content">
                        <div class="row">
                          <div class="col-lg-6">
                            <label>First Name</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{tenant.firstName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Surname</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{tenant.lastName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Date of birth	</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{tenant.dob | formatDate}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Gender</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{genderName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Ethnicity</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{ethnicityName}}</label>
                          </div>
                        </div>

                      </div>
                    </div>

                  </div>
                  <div class="col-lg-4 col-md-12">
                    <div class="card">
                      <div class="card-header">
                        <div class="row">
                          <div class="col-lg-8">
                            <h4 class="card-title">
                              Tenancy
                            </h4>
                          </div>
                          <div class="col-lg-4">
                            <span style="float:right"><a class="ti-marker-alt" v-on:click="tenancyEdit"></a></span>
                          </div>
                        </div>

                      </div>
                      <hr />
                      <div class="card-content">
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Claim Reference Number	</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{tenant.claimNumber}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>NI Number	</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{tenant.niNumber}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Provider</label>
                          </div>
                          <div class="col-lg-6">
                            <label>{{providerName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Local Authority	</label>
                          </div>
                          <div class="col-lg-6">
                            <label> {{authorityName}}</label>
                          </div>
                        </div>
                        <hr />
                        <div class="row">
                          <div class="col-lg-6">
                            <label>Tenancy from	</label>
                          </div>
                          <div class="col-lg-6">

                          </div>
                        </div>

                      </div>
                    </div>

                  </div>



                  <div class="row">
                    <div class="col-lg-4 col-md-12">
                      <div class="card">
                        <div class="card-header">
                          <div class="row">
                            <div class="col-lg-8">
                              <h4 class="card-title">
                                Contact Information

                              </h4>
                            </div>
                            <div class="col-lg-4">
                              <span style="float:right"><a class="ti-marker-alt" v-on:click="contactInfoEdit"></a></span>
                            </div>
                          </div>

                        </div>
                        <hr />
                        <div class="card-content">


                          <div class="row">
                            <div class="col-lg-6">
                              <label>Mobile</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.mobile}}</label>
                            </div>
                          </div>


                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Email</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.email}}</label>
                            </div>
                          </div>
                        </div>
                      </div>

                    </div>

                    <div class="col-lg-4 col-md-12">
                      <div class="card">
                        <div class="card-header">
                          <div class="row">
                            <div class="col-lg-8">
                              <h4 class="card-title">
                                Key Dates
                              </h4>
                            </div>
                            <div class="col-lg-4">
                              <span style="float:right"><a class="ti-marker-alt" v-on:click="keyDateEdit"></a></span>
                            </div>
                          </div>

                        </div>
                        <hr />
                        <div class="card-content">
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Pre Acceptance Inspection Date	</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.datePreAcceptanceInspection | formatDate}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Support Plan Review Date	</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.dateSupportPlanReview | formatDate}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Support Plan Completed Date		</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.dateSupportPlanCompleted | formatDate}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Risk Assessment Review Date	</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.dateRiskAssessmentReview | formatDate}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Risk Assessment Completed Date	</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.dateRiskAssessmentCompleted | formatDate}}</label>
                            </div>
                          </div>

                        </div>
                      </div>

                    </div>
                    <div class="col-lg-4 col-md-12">
                      <div class="card">
                        <div class="card-header">
                          <div class="row">
                            <div class="col-lg-8">
                              <h4 class="card-title">
                                Existing/Previous Address

                              </h4>
                            </div>
                            <div class="col-lg-4">
                              <span style="float:right"><a class="ti-marker-alt" v-on:click="addressEdit"></a></span>
                            </div>
                          </div>

                        </div>
                        <hr />
                        <div class="card-content">
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Address</label>
                            </div>
                            <div class="col-lg-6">
                              <label>
                                {{tenant.address1}}
                              </label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>Area</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.address2}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>City</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.city}}</label>
                            </div>
                          </div>
                          <hr />
                          <div class="row">
                            <div class="col-lg-6">
                              <label>PostCode</label>
                            </div>
                            <div class="col-lg-6">
                              <label>{{tenant.postCode}}</label>
                            </div>
                          </div>


                        </div>
                      </div>

                    </div>

                  </div>
                  <div class="row">
                    <div>
                      <div class="card">
                        <div class="card-header">
                          <div class="row">
                            <div class="col-lg-8">
                              <h4 class="card-title">
                                Note

                              </h4>
                            </div>
                            <div class="col-lg-4">
                              <span style="float:right"><a class="ti-marker-alt" v-on:click="noteEdit"></a></span>
                            </div>
                          </div>

                        </div>
                        <hr />
                        <div class="card-content">


                          <div class="row">

                            <div class="col-lg-12">
                              <label>{{form.notes}}</label>
                            </div>
                          </div>
                          <hr />

                        </div>
                      </div>

                    </div>
                  </div>
                </v-tab>
                <v-tab title="Tenant History">
                  <room-history></room-history>
                </v-tab>
                <v-tab title="Documents">
                  <document></document>
                </v-tab>
              </vue-tabs>
            </div>
          </form>
        </div>
      
        <!-- Tenant Information -->
        <Modal v-model="tenantInfoModel" title="Tenant Information">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>First Name</label>
                <input type="text" class="form-control" v-model="tenant.firstName" />
              </div>
              <div class="form-group">
                <label>Surname</label>
                <input type="text" class="form-control" v-model="tenant.lastName" />

              </div>
              <div class="form-group">
                <label>Date of birth</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.dob" type="date" placeholder="DoB"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
              </div>
              <div class="form-group">
                <label>Gender</label>
                <select class="form-control" v-model="tenant.gender">
                  <option v-for="item in genderList" :value="item.id">
                    {{item.title}}
                  </option>
                </select>
              </div>
              <div class="form-group">
                <label>Ethnicity</label>
                <select class="form-control" v-model="tenant.ethnicity">
                  <option v-for="item in ethnicityList" :value="item.id">
                    {{item.title}}
                  </option>
                </select>
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


        <!-- Clinet Model -->
        <Modal v-model="clinetModel" title="Client">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group" v-if="clientId== 0">

                <label>Client</label>
                <select class="form-control" v-model="tenant.clientId" name="clientId">
                  <option v-for="item in clients" :value="item.id">{{item.name}}</option>


                </select>

              </div>
              <div class="form-group">
                <label>Provider</label>
                <select class="form-control" v-model="tenant.providerId" name="providerId">
                  <option v-for="item in providers" :value="item.id">{{item.name}}</option>


                </select>
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



        <!-- Tenancy -->
        <Modal v-model="tenancyModel" title="Tenancy">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>Claim Reference Number</label>
                <input type="text" class="form-control" v-model="tenant.claimNumber" />
              </div>
              <div class="form-group">
                <label>NI Number</label>
                <input type="text" class="form-control" v-model="tenant.niNumber" />

              </div>
              <div class="form-group">
                <label>Provider</label>
                <select class="form-control" v-model="tenant.providerId" name="providerId">
                  <option v-for="item in providers" :value="item.id">{{item.name}}</option>


                </select>
              </div>
              <div class="form-group">
                <label>Local Authority</label>
                <select class="form-control" v-model="tenant.localAuthority">
                  <option v-for="item in localAuthorityList" :value="item.id">
                    {{item.title}}
                  </option>
                </select>
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

        <!-- Contact Info -->
        <Modal v-model="contactInfoModel" title="Contact Info">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>Mobile</label>
                <input type="text" class="form-control" @keyup="regexPhoneNumber" v-model="tenant.mobile" />
                <br />
                <small class="text-danger" v-if="!phoneValid">
                  Enter Valid Mobile Number
                </small>
              </div>
              <div class="form-group">
                <label>Email</label>
                <input type="email" class="form-control" v-model="tenant.email" />

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
        <!-- Key Dates -->
        <Modal v-model="keyDateModel" title="Key Dates">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>Pre Acceptance Inspection Date</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.datePreAcceptanceInspection" type="date" placeholder="Inspection Date"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
              </div>
              <div class="form-group">
                <label>Support Plan Review Date</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.dateSupportPlanReview" type="date" placeholder="Review Date"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
              </div>

              <div class="form-group">
                <label>Support Plan Completed Date</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.dateSupportPlanCompleted" type="date" placeholder="Plan Completed Date"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
              </div>
              <div class="form-group">
                <label>Risk Assessment Review Date</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.dateRiskAssessmentReview" type="date" placeholder="Assessment Review Date"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
              </div>
              <div class="form-group">
                <label>Risk Assessment Completed Date</label>
                <div class="form-group">
                  <el-date-picker v-model="tenant.dateRiskAssessmentCompleted" type="date" placeholder="Assessment Completed Date"
                                  :picker-options="pickerOptions1" format="dd/MM/yyyy">
                  </el-date-picker>
                </div>
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


        <!-- Existing/Previous Address -->
        <Modal v-model="addressModel" title="Existing/Previous Address">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>Address</label>
                <input type="text" class="form-control" v-model="tenant.address1" />
              </div>
              <div class="form-group">
                <label>Area</label>
                <input type="text" class="form-control" v-model="tenant.address2" />

              </div>
              <div class="form-group">
                <label>City</label>
                <input type="text" class="form-control" v-model="tenant.city" />

              </div>
              <div class="form-group">
                <label>PostCode</label>
                <input type="text" class="form-control" v-model="tenant.postCode" />

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
        <!-- Note -->
        <Modal v-model="noteModel" title="Note">
          <div class="row">
            <div class="col-lg-12">
              <div class="form-group">

                <label>Note</label>
                <textarea type="text" class="form-control" v-model="form.notes"> </textarea>
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
</template>
<script>
  import { mapFields } from 'vee-validate'
  import { addUpdateTenant, getTenantById }
    from '@/helpers/tenant'
  import { getClients } from '@/helpers/client'
  import { getProviders } from '@/helpers/provider'
  import { getOptionByHeader } from '@/helpers/option'

  import { DatePicker } from 'element-ui'
  import { ModelSelect } from 'vue-search-select'
  import { getNoteByParentId } from '@/helpers/note'
  import Vue from 'vue'
  import swal from 'sweetalert2'

  import VueTabs from 'vue-nav-tabs'
  Vue.use(VueTabs)
  import Document from '@/components/Documents/Document.vue'
  import RoomHistory from '@/components/RoomHistory/RoomHistory.vue'
  export default {
    components: {
      [DatePicker.name]: DatePicker,
      ModelSelect,
      Document,
      RoomHistory

    },
    data() {
      return {
        clinetModel: false,
        tenantInfoModel: false,
        tenancyModel: false,
        contactInfoModel: false,
        keyDateModel: false,
        addressModel: false,
        noteModel: false,
        phoneValid: true,
        isLoading: false,
        IsValid: false,

        errorMessage: null,
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
          ethnicity: 0,
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
        form: {
          notes: '',
          noteCategoryId: 0,
          parentId: 0,
          typeId: 0,
        },
        modelValidations: {
          clinetId: {
            required: true,
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
            required: true,
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
        localAuthorityList: [],
        tenantId: 0,
        clientId: 0,
        clientName: '',
        providerName: '',
        genderName: '',
        ethnicityName: '',
        referralName: '',
        authorityName: '',
        clientId:''
      }
    },
    mounted() {
      let str = this.$route.params.id;
      var dId = this.aesDecript1(str);
      var id = parseInt(dId.slice(0, -1));
      this.tenantId = id;
      this.clientId = window.localStorage.getItem("clientId");
      if (this.clientId != 0) {
        this.tenant.clientId == parseInt(this.clientId);
      }
      debugger;
      if (this.tenantId != null) {

        
        this.getTenantById(id).then(res => {
          this.tenant = res

          this.getProviders("", this.tenant.clientId).then(res => {
              //this.providers = res
              //conosle.log(this.providers);
            this.providers = res;
              //res.forEach((value, index) => {
              //  this.providers.push({ value: value.id, text: value.name });
              //});
            this.providerName = this.providers.find(x => x.id == this.tenant.providerId).name;

            }).catch(ex => {
              this.errorMessage = ex
            //  swal({
            //    title: 'Worng',
            //    text: this.errorMessage,
            //    type: 'warning',
            //    confirmButtonClass: 'btn btn-success btn-fill',
            //    buttonsStyling: false
            //  })
            })
          this.loadClient();
          this.loadOptions();
          this.loadEtitcity();
          this.referrall();
          this.localauthorityy();
          this.loadNote();
        })
      }
    },
    methods: {
      addUpdateTenant: addUpdateTenant,
      getTenantById: getTenantById,
      getClients: getClients,
      getProviders: getProviders,
      getOptionByHeader: getOptionByHeader,
      getNoteByParentId: getNoteByParentId,
      async submit() {
        try {
          this.tenant.gender = parseInt(this.tenant.gender);
          this.tenant.ethnicity = parseInt(this.tenant.ethnicity);
          const isValidForm = this.IsValid;
          if (isValidForm) {
            await this.addUpdateTenant(this.tenant)
              .then(res => {
                if (res != null || res != undefined)
                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Updated</b> </br> Tenant Updated Successfully</span>`
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

        this.getProviders("", this.tenant.clientId)
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
            this.genderName = this.genderList.find(x => x.id == this.tenant.gender).title;
            console.log(this.genderList.find(x => x.id == this.tenant.gender).title);

          })
          .catch(ex => {
            this.errorMessage = ex
            this.color = 'error', this.direction = 'bottom right', this.snackbar = true
          })


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

            this.ethnicityName = this.ethnicityList.find(x => x.id == this.tenant.ethnicity).title;
          })
          .catch(ex => {
            this.errorMessage = ex
                 })
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
                })
      },
      localauthorityy() {
        this.getOptionByHeader("LocalAuthority")
          .then(res => {
            //res.forEach((value, index) => {
            //  this.clients.push({ value: value.id, text: value.name });
            //});
            this.localAuthorityList = res;
            //console.log(res);
            //this.clients = res;
            this.authorityName = this.localAuthorityList.find(x => x.id == this.tenant.localAuthority).title;
          })
          .catch(ex => {
            this.errorMessage = ex
                   })
      },
      loadClient() {
        let val = "";
        this.getClients(val).then(res => {
          this.clients = res;
          //res.forEach((value, index) => {
          //  this.clients.push({ value: value.id, text: value.name });
          //});

          //this.clients = res;
          this.clientName = this.clients.find(x => x.id == this.tenant.clientId).name;
          console.log(this.clients.find(x => x.id == this.tenant.clientId).name);
          //   console.log(this.clients.find(x => x.id == this.tenant.clientId));
        })
          .catch(ex => {
            this.errorMessage = ex
                  })
      },
      loadNote() {
        this.getNoteByParentId(this.tenant.id, 3).then(res => {
          this.form = res;
          
        })
      },

      clientEdit() {
        this.clinetModel = true;
      },
      tenantInfoEdit() {
        this.tenantInfoModel = true;
      },
      tenancyEdit() {
        this.tenancyModel = true;
      },
      contactInfoEdit() {
        this.contactInfoModel = true;
      },
      keyDateEdit() {
        this.keyDateModel = true;
      },
      addressEdit() {
        this.addressModel = true;
      },

      noteEdit() {
        this.loadNote();
        this.noteModel = true;
      
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
      ...mapFields(['email', 'address1', 'dob', 'dateRiskAssessmentReview', 'datePreAcceptanceInspection', 'dateRiskAssessmentCompleted', 'dateSupportPlanCompleted', 'localAuthority', 'referralMethod', 'claimNumber', 'ethnicity', 'gender', 'lastName', 'firstName', 'postCode', 'city', 'mobile', 'niNumber', 'address2', 'dateSupportPlanReview']),
      //...mapState({
      //  list: state => state.client.list,
      //}),


    },

  }
</script>
<style>
  .modal-footer {
    padding: 15px;
    text-align: right;
    border-top: 0px solid #e5e5e5;
  }
</style>


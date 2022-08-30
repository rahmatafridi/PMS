<template>
  <div>

    <div class="row">
      <div class="col-sm-6">

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
          <el-table-column class="success" label="Room#" property="roomNo"></el-table-column>

          <el-table-column :min-width="120" label="Current Tenant" property="tenant"></el-table-column>
          <el-table-column class="success" label="Tenancy Start" property="tenancyStartDate">
            <template slot-scope="props">
              {{ props.row.tenancyStartDate | formatDate }}
            </template>
          </el-table-column>
          <el-table-column class="success" label="Core Rent" property="coreRent">
            <template slot-scope="props">
              {{'£'}} {{ props.row.coreRent }}
            </template>
          </el-table-column>
          <el-table-column class="success" label="Service Charge" property="serviceCharge">
            <template slot-scope="props">
              {{'£'}} {{ props.row.serviceCharge }}
            </template>

          </el-table-column>


          <el-table-column label="Personal Charge" property="personalCharge">
            <template slot-scope="props">
              {{'£'}} {{ props.row.personalCharge }}
            </template>
          </el-table-column>

          <el-table-column :min-width="60"
                           fixed="right"
                           label="Actions">
            <template slot-scope="props">
              <a class="btn btn-simple btn-xs btn-warning btn-icon edit" @click="handleEdit(props.$index, props.row)"><i class="ti-pencil-alt"></i></a>
              <a class="btn btn-simple btn-xs btn-info " @click="handleNote(props.$index, props.row)"><i class="ti-marker"></i></a>

              <a class="btn btn-simple btn-xs btn-secondary  " @click="handleEnd(props.$index, props.row)"><i class="ti-na"></i></a>
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
    <div>
      <Modal v-model="showModal" title="Room">
        <div class="row">
          <div class="col-lg-12">
            <div class="form-group">
              <label>Room No</label>
              <input class="form-control" v-model="room.roomNo" readonly />

            </div>
            <div class="form-group">
              <label>Current Tenant</label>
              <select class="form-control" v-validate="modelValidations.tenantId" name="tenantId" v-model="room.tenantId">
                <option v-for="item in tenants" :value="item.id">{{item.firstName}} {{item.lastName}}</option>
              </select>
              <small class="text-danger" v-show="tenantId.invalid">
                {{ getError('tenantId') }}
              </small>
            </div>
            <div class="form-group">
              <label>Tenancy Start Date:</label>
              <div class="form-group">


                <el-date-picker v-model="room.tenancyStartDate"
                                type="date" placeholder="Select To Valid"
                                format="dd/MM/yyyy"
                                v-validate="modelValidations.tenancyStartDate" name="tenancyStartDate"
                                :picker-options="pickerOptions1">
                </el-date-picker>
                <div class="form-group">
                  <small class="text-danger" v-show="tenancyStartDate.invalid">
                    {{ getError('tenancyStartDate') }}
                  </small>
                </div>
              </div>
              <!--<input class="form-control" v-model="room.tenancyStartDate" readonly />-->
            </div>
            <div class="form-group">
              <div class="row">
                <div class="col-lg-4">
                  <label>Core rent</label>
                  <input class="form-control" v-validate="modelValidations.coreRent" name="coreRent" style="text-align: right;" @keypress="isNumber($event)" v-model="room.coreRent" />
                  <small class="text-danger" v-show="coreRent.invalid">
                    {{ getError('coreRent') }}
                  </small>
                </div>
                <div class="col-lg-4">
                  <label>Service Charge</label>
                  <input class="form-control" v-validate="modelValidations.serviceCharge" name="serviceCharge" style="text-align: right;" @keypress="isNumber($event)" v-model="room.serviceCharge" />
                  <small class="text-danger" v-show="serviceCharge.invalid">
                    {{ getError('serviceCharge') }}
                  </small>
                </div>
                <div class="col-lg-4">
                  <label>Personal Charge</label>
                  <input class="form-control" v-validate="modelValidations.personalCharge" name="personalCharge" style="text-align: right;" @keypress="isNumber($event)" v-model="room.personalCharge" />

                </div>
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
    </div>
    <div>
      <Modal v-model="showModalEnd" title="Ending tenancy">
        <div class="row">
          <div class="col-lg-12">

            <div class="form-group">
              <label>
                Tenancy End Date
               
              </label>
              <div class="form-group">


                <el-date-picker v-model="room.tenancyEndDate"
                                type="date" placeholder="Select To Valid"
                                format="dd/MM/yyyy"
                                :picker-options="pickerBeginDateAfter">
                </el-date-picker>
                <!--<div class="form-group">
                  <small class="text-danger" v-show="tenancyEndDate.invalid">
                    {{ getError('tenancyStartDate') }}
                  </small>
                </div>-->
              </div>
              <div>
                <label>
                  Tenancy Allocation
                </label>
                <div class="form-group">
                  <div>
                    <input type="radio" v-model="room.picked" value="movingTenant" checked name="tenancyAllocation" />This tenant is moving rooms/properties
                  </div>
                  <div>
                    <input type="radio" v-model="room.picked" value="leavingTenant"  name="tenancyAllocation" />This tenant is leaving
                  </div>
                </div>
              </div>
              <div>
                <label>Note</label>
                <small>{{roomNote}}</small>
              </div>
              <div>
                <label>
                  Please do not create a duplicate tenant record for the same person.

                </label>
              </div>
              <!--<input class="form-control" v-model="room.tenancyStartDate" readonly />-->
            </div>
          </div>
          <div class="row modal-footer">
            <div class="col-sm-12">
              <div class="float-right">
                <button class="btn btn-secondary ml-2" type="button">Cancel</button>

                <button class="btn btn-info" type="button" @click.prevent="validate">Submit</button>
              </div>
            </div>
          </div>
        </div>
      </Modal>
    </div>

    <div>
      <Modal v-model="showModalNote" title="Note">
        <div class="row">
          <div class="col-lg-12">

            <div class="form-group">
              <label>Note</label>
              <div class="form-group">

                <textarea v-model="note.notes" class="form-control"></textarea>
               
                <!--<div class="form-group">
                  <small class="text-danger" v-show="tenancyStartDate.invalid">
                    {{ getError('tenancyStartDate') }}
                  </small>
                </div>-->
              </div>
         
              <!--<input class="form-control" v-model="room.tenancyStartDate" readonly />-->
            </div>
          </div>
          <div class="row modal-footer">
            <div class="col-sm-12">
              <div class="float-right">
                <button class="btn btn-info" type="button" @click="submitNote">Save</button>
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
  import Vue from 'vue'
  import VueModal from '@kouts/vue-modal'
  import '@kouts/vue-modal/dist/vue-modal.css'
  import swal from 'sweetalert2'

  import { Table, TableColumn, Select, Option, DatePicker } from 'element-ui'
  import PPagination from 'src/components/UIComponents/Pagination.vue'
  import { addUpdateRoom, getRoomById } from '@/helpers/room'
  import { loadTenant, getTenantById } from '@/helpers/tenant'
  import { addUpdateNote,getNoteById} from '@/helpers/note'
  Vue.component('Modal', VueModal)
  Vue.use(Table)
  Vue.use(TableColumn)
  Vue.use(Select)
  Vue.use(Option)
  Vue.use(DatePicker)

  import { mapState } from 'vuex'
  import { mapFields } from 'vee-validate'

  export default {
    components: {
      PPagination,
      'Modal': VueModal

    },
    data() {
      return {
        showModalEnd:false,
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
        propsToSearch: ['roomNo'],
        propertyId: 0,
        room: {
          roomNo: '',
          tenantId: 0,
          tenancyStartDate: null,
          serviceCharge: '',
          personalCharge: '',
          coreRent: '',
          id: 0,
          tenancyEndDate: null,
          picked: '',
          isTenantIsMoving:false,
          isTenantLeaving:false
        },
        note: {
          id: 0,
          noteCategoryId: 0,
          parentId: 0,
          typeId: 0,
          notes:''
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
        pickerBeginDateAfter: {
          disabledDate(time) {
            return time.getTime() > Date.now();
          }
        },
        showModal: false,
        tenants: [],
        modelValidations: {
          roomNo: {
            required: true,
          },
          tenantId: {
            required: true,
          },
          tenancyStartDate: {
            required:true
          },
          serviceCharge: {
            required:true
          },
          personalCharge: {
            required:true
          },
          coreRent: {
            required:true
          }

        },
        tenantName: '',
        showModalNote: false,
        roomNote:''

      }
    },
    methods: {
      addUpdateRoom: addUpdateRoom,
      getRoomById: getRoomById,
      loadTenant: loadTenant,
      getTenantById: getTenantById,
      addUpdateNote: addUpdateNote,
      getNoteById: getNoteById,
      async submit() {
        try {
          debugger
          this.room.serviceCharge = parseInt(this.room.serviceCharge);
          this.room.personalCharge = parseInt(this.room.personalCharge);
          this.room.coreRent = parseInt(this.room.coreRent);
          if (this.room.picked == "movingTenant") {
            this.room.isTenantIsMoving = true;
            this.room.isTenantLeaving = false;
          }
          if (this.room.picked == "leavingTenant") {
            this.room.isTenantIsMoving = false;
            this.room.isTenantLeaving = true;
          }
          const isValidForm = this.IsValid;
          if (isValidForm) {
            await this.addUpdateRoom(this.room)
              .then(res => {
                if (res != null || res != undefined)
                  this.showModal = false;
                this.roomList();
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Added</b> </br> Room Added Successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })

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
        } finally {
          //
        }
      },
      async submitNote() {
        try {
          //this.room.serviceCharge = parseInt(this.room.serviceCharge);
          //this.room.personalCharge = parseInt(this.room.personalCharge);
          //this.room.coreRent = parseInt(this.room.coreRent);
          this.note.noteCategoryId = 2;
          this.typeId = 2;
          await this.addUpdateNote(this.note)
              .then(res => {
                if (res != null || res != undefined)
                  this.showModalNote = false;
                this.roomList();
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
                swal({
                  title: 'Warning',
                  text: this.errorMessage,
                  type: 'warning',
                  confirmButtonClass: 'btn btn-success btn-fill',
                  buttonsStyling: false
                })              })
          
        }
        finally {
          //
        }
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
      roomList() {

        this.$store.dispatch('room/fetchList', {
          auth: {
            proId: this.propertyId
          },
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
      handleEdit(index, row) {
        this.loadTenant().then(res => {
          this.tenants = res.items;

        })
        this.getRoomById(row.id).then(res => {
          this.room = res;
          this.showModal = true;
        })
        //this.$router.push(`/property/edit/${row.id}`)

      },
      handleNote(index, row) {
        this.getRoomById(row.id).then(res => {
          this.note.parentId = res.propertyId;

          this.showModalNote = true;
        })
        //this.$router.push(`/property/edit/${row.id}`)

      },

      handleEnd(index, row) {
        this.loadTenant().then(res => {
          this.tenants = res.items;

        })
        this.getRoomById(row.id).then(res => {
          this.room = res;
          this.getTenantById(this.room.tenantId).then(res => {
            console.log(res);
            this.tenantName = res.firstName + ' ' + res.lastName;
            this.getNoteById(this.propertyId,2).then(res => {
              this.roomNote = res.note;
            })
            this.showModalEnd = true;

          })
        })
        //this.$router.push(`/property/edit/${row.id}`)

      },
      validate() {
        this.$validator.validateAll().then(isValid => {
          this.IsValid = isValid;
          this.$emit('submit', this.registerForm, isValid)
          this.submit();
        })
      },
      getError(fieldName) {
        return this.errors.first(fieldName)
      },

      onCancel() {
        this.showModal = false;
        this.showModalEnd = false;
        this.showModalNote = false;
      },
      isNumber: function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
          evt.preventDefault();;
        } else {
          return true;
        }
      }

    },
    computed: {
      ...mapFields(['personalCharge', 'serviceCharge', 'coreRent', 'tenancyStartDate','tenantId','roomNo']),

      ...mapState({
        list: state => state.room.list,
        isLoading: state => state.room.isLoading
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
      },
      docData() {

        if (this.listDoc.items != null) {
          let result = this.listDoc.items

          return result;
        }
      },

    },
    mounted: function () {
      let str = this.$route.params.id;
      var dId = this.aesDecript1(str);
      var id = parseInt(dId.slice(0, -1));
      this.propertyId = id;

      
     
    //  this.loadDocument();
      this.roomList();
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

<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add Role
            </h4>
          </div>
          <hr />
          <div class="card-content">
            <div class="row">
              <div class="col-lg-4" v-if="clientId == 0">
                <div class="form-group">
                  <label>Clients</label>
                  <model-select :options="clients"
                                v-model="role.clientId"
                                name="clientId"
                                placeholder="select client">
                  </model-select>
                  <small class="text-danger" v-show="clientId.invalid">
                    {{ getError('name') }}
                  </small>
                 
                  <!--<select class="form-control" v-model="role.clientId" v-validate="modelValidations.clientId" name="clientId">
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
                  <label>Name</label>
                  <input type="text" v-validate="modelValidations.name" @keyup="validateRole" name="name" placeholder="Enter Name" v-model="role.name" class="form-control">
                  <small class="text-danger" v-show="name.invalid">
                   {{ getError('name') }}
                  </small>
                </div>

              </div>

              <div class="col-lg-4">
                <div class="form-group">
                  <label>Description</label>
                  <input type="text" name="description" v-model="role.description" placeholder="Enter Description" class="form-control">

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
      </div>   
    </div>  

  </div>
</template>
<script>

  import { ModelSelect } from 'vue-search-select'
  import { mapFields } from 'vee-validate'
  //import { mapState } from 'vuex'
  import { addUpdateRole, validateRoleNameByClient, getRoleById } from '@/helpers/role'
  import { getClients } from '@/helpers/client'
  import swal from 'sweetalert2'


  export default {
    components: {
      ModelSelect
    },
    data() {
      return {
        isLoading: false,
        IsValid:false,
        errorMessage: null,
        role : {
          id: 0,
          clientId: null,
          name: '',
          description: '',
        },
        modelValidations: {
          clientId: {
            required: true,
          },
          name: {
            required: true,

          }
         
         
        },
        search: null,
        clients: [],
        selected: { name: null, id: null },
        isValidRole: true,
        clientId:''


      }
    },
   
    methods: {
      addUpdateRole: addUpdateRole,
      validateRoleNameByClient: validateRoleNameByClient,
      getRoleById: getRoleById,
      getClients: getClients,
      async submit() {
         
        try {
          if (this.isValidRole == false) {
            swal({
              title: 'Warning',
              text: `Role is already in use.`,
              type: 'warning',
              confirmButtonClass: 'btn btn-success btn-fill',
              buttonsStyling: false
            })
            return;
          }

          const isValidForm = this.IsValid;
          if (isValidForm) {
            //await this.validateRoleNameByClient(this.role)
            //  .then(res => {
            //  })
            //  .catch(e => {
            //    console.log(e);
            //    swal({
            //      title: 'Warning',
            //      text: this.errorMessage,
            //      type: 'warning',
            //      confirmButtonClass: 'btn btn-success btn-fill',
            //      buttonsStyling: false
            //    })
            //    return;
            //    this.errorMessage = e
            //    this.color = 'error', this.direction = 'bottom right', this.snackbar = true
            //  })

            await this.addUpdateRole(this.role)
              .then(res => {
                if (res != null || res != undefined)
                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Added</b> </br> Role Added Successfully</span>`
                      },
                      icon: 'ti-bell',
                      horizontalAlign: 'right',
                      verticalAlign: 'top',
                      type: 'info'
                    })

                  this.$router.push('/roles')
              })
              .catch(e => {
                this.errorMessage = e
                this.color = 'error', this.direction = 'bottom right', this.snackbar = true
              })

          
          }
        }
        finally {
          this.isSending = false
        }
      },
      onCancel() {
        this.$router.push('/roles')
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
      validateRole(event) {
        const value = event.target.value
        this.role.name = value;
        if (this.role.clientId != null || this.role.clientId != '') {
          this.validateRoleNameByClient(this.role).then(res => {
            if (res == true) {
              this.isValidRole = true;
            }
            else {
              this.isValidRole == false;
            }
            console.log(res);
          }).catch(e => {
            this.isValidRole == false;

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

    },
    computed: {
      ...mapFields(['name', 'clientId'])
    },
    mounted: function () {
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
        this.role.clientId = parseInt(this.clientId);
      }
      this.fetchList();
    
    
    }
  }
</script>

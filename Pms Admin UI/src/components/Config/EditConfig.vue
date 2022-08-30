<template>
  <div class="row">
    <div class="col-md-12">
      <div class="card">
        <form>
          <div class="card-header">
            <h4 class="card-title">
              Add Config
            </h4>
          </div>
          <hr />
          <div class="card-content">
            <div class="row">
              <div class="col-lg-4" v-if="clientId ==0">
                <div class="form-group">
                  <label>Client</label>
                  <model-select :options="clients"
                                v-model="config.clientId"
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
                  <label>Key</label>
                  <input type="text" v-validate="modelValidations.key" name="key" v-model="config.key" placeholder="Enter Key" class="form-control">
                  <small class="text-danger" v-show="key.invalid">
                    {{ getError('key') }}
                  </small>
                </div>

              </div>
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Value</label>
                  <input type="text" v-validate="modelValidations.value" name="value" v-model="config.value" placeholder="Enter value" class="form-control">
                  <small class="text-danger" v-show="value.invalid">
                    {{ getError('value') }}
                  </small>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-lg-4">
                <div class="form-group">
                  <label>Description</label>
                  <input type="text" placeholder="Enter Description" v-model="config.description" class="form-control">

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
  import { getClients } from '@/helpers/client'
  import { ModelSelect, MultiSelect } from 'vue-search-select'
  import swal from 'sweetalert2'

  import { addUpdateConfig,  getConfigById } from '@/helpers/config'
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
        secret: "123#$%",

        config: {
          key: '',
          value: '',
          description: '',
          clientId: 0,
          id:0

        },
        clinterror: false,
        modelValidations: {

          key: {
            required: true,

          },
          value: {
            required: true,

          }
        },
        clients:[],
        clientId:''
      }
    },
    mounted() {
      let id = this.$route.params.id;
      this.clientId = window.localStorage.getItem("clientId");
      var dId = this.aesDecript(id);
      var str = parseInt(dId.slice(0, -1));
      if (this.clientId != 0) {
        this.config.clientId = parseInt(this.clientId);
      }
      if (str != 0) {
        this.getClients().then(res => {
          res.forEach((value, index) => {
            this.clients.push({ value: value.id, text: value.name });
          });
          //  this.clients = res;
        })
        this.getConfigById(str).then(res => {
          this.config = res;
        })
      }
      
    },
    methods: {
      addUpdateConfig: addUpdateConfig,
      getClients: getClients,
      getConfigById: getConfigById,
      async submit() {
        try {
          const isValidForm = this.IsValid;
          if (isValidForm) {
            await this.addUpdateConfig(this.config)
              .then(res => {
                if (res != null || res != undefined)

                  this.$notify(
                    {
                      component: {
                        template: `<span> <b>Updated</b> </br> Config Updated Successfully</span>`
                      },
                      icon: 'alert-with-icon',
                      horizontalAlign: 'right',
                      verticalAlign: 'top',
                      type: 'info'
                    })
                  this.$router.push('/configs')
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
        this.$router.push('/configs')
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
      aesDecript(txt) {
        var CryptoJS = require("crypto-js");

        var bytes = CryptoJS.AES.decrypt(txt, this.secret);
        var originalText = bytes.toString(CryptoJS.enc.Utf8);
        return originalText;
      }

    },
    computed: {
      ...mapFields(['key', 'value'])

    },

  }</script>

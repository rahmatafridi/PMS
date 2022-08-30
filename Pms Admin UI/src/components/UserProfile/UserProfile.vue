<template>
  <div class="card">
    <div class="card-header">
      <h4 class="title">Edit Profile</h4>
    </div>
    <div class="card-content">
      <form>
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
          <!--<div class="col-lg-4">
            <div class="form-group">
              <label>Confirm Password</label>
              <input type="password" v-validate="modelValidations.confirmpassword" name="confirmpassword" placeholder="Enter Confirm Password" v-model="confirmpassword" class="form-control">
              <small class="text-danger" v-show="confirmpassword.invalid">
                {{ getError('confirmpassword') }}
              </small>
            </div>

          </div>-->
          <div class="col-lg-4">
            <div class="form-group">
              <label>Mobile</label>
              <input type="text" v-validate="modelValidations.mobile" name="mobile" v-model="user.mobile" placeholder="Enter Mobile" class="form-control">
              <small class="text-danger" v-show="mobile.invalid">
                {{ getError('mobile') }}
              </small>
            </div>
          </div>
          <!--<div class="col-lg-4">
    <div class="form-group" style="margin-top:30px;">
      <p-checkbox :checked="user.isActive" v-model="user.isActive">IsActive</p-checkbox>

    </div>
  </div>-->
        </div>
        <div class="text-center">
          <button type="submit" class="btn btn-info btn-fill btn-wd" @click.prevent="validate">
            Update Profile
          </button>
        </div>
        <div class="clearfix"></div>
      </form>
    </div>
  </div>
</template>
<script>
  import { mapFields } from 'vee-validate'
  import { getUserForProfile, getRolesByUserId, addUpdateUser} from '@/helpers/user'

  export default {
    data () {
      return {
        isLoading: false,
        IsValid: false,
        errorMessage: null,
        clinterror: false,
        roleerror: false,
        isActive: false,
        user: {
          clientId: 0,
          id: 0,
          userName: '',
          email: '',
          displayName: '',
          mobile: '',
          password: null,
          isActive: false,
         
          roleIds: [],
        },
        
        modelValidations: {
          userName: {
            required: true,
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
            required: true
          },
          //confirmpassword: {
          //  required:true
          //}
        },

      }
    },
    methods: {
      getUserForProfile: getUserForProfile,
      getRolesByUserId: getRolesByUserId,
      addUpdateUser: addUpdateUser,
      updateProfile () {
        alert('Your data: ' + JSON.stringify(this.user))
      },
      getError(fieldName) {
        return this.errors.first(fieldName)
      },
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
                this.$notify(
                  {
                    component: {
                      template: `<span> <b>Updated</b> User Profile Updated successfully</span>`
                    },
                    icon: 'alert-with-icon',
                    horizontalAlign: 'right',
                    verticalAlign: 'top',
                    type: 'info'
                  })

              //    this.$router.push('/users')
              })
              .catch(e => {
                this.errorMessage = e
                this.color = 'error', this.direction = 'bottom right', this.snackbar = true
              })
          }
        } finally {
          //
        }
      },
      validate() {
        this.$validator.validateAll().then(isValid => {
          this.IsValid = isValid;
          this.$emit('submit', this.registerForm, isValid)
          this.submit();
        })
      }
    },
    computed: {
      ...mapFields(['email', 'userName', 'displayName', 'password', 'mobile']),
      //...mapState({
      //  list: state => state.client.list,
      //}),


    },
    mounted() {
      //this.fetchList();
      this.getUserForProfile().then(res => {
        this.user = res;
        this.confirmpassword = this.user.password;

        this.getRolesByUserId(this.user.id).then(res1 => {
          this.user.roleIds = res1

        })
      
        console.log(this.user);
      })
    }
  }</script>
<style>
</style>

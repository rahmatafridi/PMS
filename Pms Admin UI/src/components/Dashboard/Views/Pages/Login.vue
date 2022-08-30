<template>
  <div>
    <nav class="navbar navbar-transparent navbar-absolute">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle"
                  data-toggle="collapse"
                  data-target="#navigation-example-2"
                  @click="toggleNavbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <router-link class="navbar-brand" to="/admin">PMS</router-link>
        </div>
        <div class="collapse navbar-collapse">
          <ul class="nav navbar-nav navbar-right">
            <router-link to="/register" tag="li">
              <a>Register</a>
            </router-link>
           
          </ul>
        </div>
      </div>
    </nav>

    <div class="wrapper wrapper-full-page">
      <div class="full-page login-page" data-color=""
           data-image="static/img/background/background-2.jpg">
        <!--   you can change the color of the filter page using: data-color="blue | azure | green | orange | red | purple" -->
        <div class="content">
          <div class="container">
            <div class="row">
              <div class="col-md-4 col-sm-6 col-md-offset-4 col-sm-offset-3">
                <form >
                  <div class="card" data-background="color" data-color="blue">
                    <div class="card-header">
                      <span class="text-danger" if="massge">{{errorMessage}}</span>
                      <h3 class="card-title">Login</h3>
                    </div>
                    <div class="card-content">
                      <div class="form-group">
                        <label>Email </label>
                        <input type="text" v-validate="modelValidations.email" name="email" placeholder="Enter email" v-model="login.email" class="form-control">
                        <small class="text-danger" v-show="email.invalid">
                          {{ getError('email') }}
                        </small>
                      </div>
                      <div class="form-group">
                        <label>Password</label>
                        <input type="password" placeholder="Password" v-validate="modelValidations.password" name="password" v-model="login.password" class="form-control">
                        <small class="text-danger" v-show="password.invalid">
                          {{ getError('password') }}
                        </small>
                      </div>
                    </div>
                    <div class="card-footer text-center">
                      <button type="button" @click.prevent="validate" class="btn btn-fill btn-wd ">Login</button>
                      <div class="forgot">
                        <router-link to="/register">
                          Forgot your password?
                        </router-link>
                      </div>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>

        <footer class="footer footer-transparent">
          <div class="container">
            <div class="copyright">
              &copy; Coded with
              <i class="fa fa-heart heart"></i> by
              <a href="https://github.com/cristijora" target="_blank">Cristi Jora</a>.
              Designed by <a href="https://www.creative-tim.com/?ref=pdf-vuejs" target="_blank">Creative Tim</a>.
            </div>
          </div>
        </footer>
        <div class="full-page-background" style="background-image: url(static/img/background/background-2.jpg) "></div>
      </div>
    </div>
    <div class="collapse navbar-collapse off-canvas-sidebar">
      <ul class="nav nav-mobile-menu">
        <router-link to="/register" tag="li">
          <a>Register</a>
        </router-link>
        
      </ul>
    </div>
  </div>
</template>
<script>
  import { mapState } from 'vuex'
  import { mapFields } from 'vee-validate'

  export default {
    data() {
      return {
        isLoading: false,
        IsValid: false,
        massge: false,
        errorMessage: null,
        login: {
          email: '',
          password:''
        },
        modelValidations: {
          email: {
            required: true,
          },
          password: {
            required: true,

          }


        },
       


      }
    },
      computed: {
      ...mapState({
        postLoginUrl: state => state.login.postLoginUrl,
      }),
        ...mapFields(['email', 'password'])

    },
    methods: {
      submit() {
        const isValidForm = this.IsValid;
        if (isValidForm) {
          try {
           // this.loading = true
            const redirectFrom = this.$route.query.redirectFrom
            this.$store
              .dispatch('login/login', {
                auth: {
                  UserName: this.login.email,
                  Password: this.login.password,
                },
              })
              .then(() => {
                this.$router
                  .push(redirectFrom ? redirectFrom : this.postLoginUrl)
                  .catch(() => {
                   
                  })
              })
              .catch(e => {
                this.massge = true;
                this.errorMessage = e
              })
              .finally(() => {
                this.loading = false
              })
          }
          finally {
            //
          }
        }
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
      toggleNavbar () {
        document.body.classList.toggle('nav-open')
      },
      closeMenu () {
        document.body.classList.remove('nav-open')
        document.body.classList.remove('off-canvas-sidebar')
      }
    },
    beforeDestroy () {
      this.closeMenu()
    }
  }
</script>
<style>
</style>

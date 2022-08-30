import Vue from 'vue'
import './pollyfills'
import VueRouter from 'vue-router'
import VueNotify from 'vue-notifyjs'
import VeeValidate from 'vee-validate'
import lang from 'element-ui/lib/locale/lang/en'
import locale from 'element-ui/lib/locale'
import App from './App.vue'
import moment from 'moment';
import sidebar1 from './components/UIComponents/SidebarPlugin/index'
// Import fontawesome 
import { library } from '@fortawesome/fontawesome-svg-core'
import { fas } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
library.add(fas);
Vue.component('font-awesome-icon', FontAwesomeIcon)
// Plugins
import GlobalComponents from './gloablComponents'
import GlobalDirectives from './globalDirectives'
import SideBar from './components/UIComponents/SidebarPlugin'

// router setup

import store from './store/index'
import routes from './routes/routes'
// library imports

import './assets/sass/paper-dashboard.scss'
import './assets/sass/element_variables.scss'
import './assets/sass/demo.scss'

import sidebarLinks from './sidebarLinks'
window.Vue = Vue;
import 'vue-search-select/dist/VueSearchSelect.css'
import Multiselect from 'vue-multiselect'
Vue.component('multiselect', Multiselect)
import LiquorTree from 'liquor-tree'

Vue.use(LiquorTree)



//import VueCryptojs from 'vue-cryptojs'

//Vue.use(VueCryptojs)
// plugin setup
Vue.use(VueRouter)
Vue.use(GlobalDirectives)
Vue.use(GlobalComponents)
Vue.use(VueNotify)
Vue.use(SideBar)
Vue.use(VeeValidate)
locale.use(lang)
Vue.filter('formatDate', function (value) {
  if (value) {
    return moment(String(value)).format('DD/MM/YYYY')
  }
});

var secret1 = "123#$%";
Vue.mixin({
  methods: {
    aesEncrypt1: function (txt) {
      var CryptoJS = require("crypto-js");
      var ciphertext = CryptoJS.AES.encrypt(txt, secret1).toString();
      return ciphertext.toString()
    },
    aesDecript1: function (txt) {
      var CryptoJS = require("crypto-js");

      var bytes = CryptoJS.AES.decrypt(txt, secret1);
      var originalText = bytes.toString(CryptoJS.enc.Utf8);
      return originalText;
    },
  },
})

//Vue.directive('uppercase', {
//  bind(el, _, vnode) {
//    el.addEventListener('input', (e) => {
//      e.target.value = e.target.value.toUpperCase()
//      //vnode.componentInstance.$emit('input', e.target.value.toUpperCase())
//    })
//  }
//})

// configure router
const router = new VueRouter({
  mode: "history",
  routes, // short for routes: routes
  linkActiveClass: 'active'
})
const waitForStorageToBeReady =  (to, from, next) => {
  //await store.restored
  debugger;
  var userPermissions = store.state.login.permissions;
  //const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const requiresAuth = to.meta.requiresAuth;
  const permission = to.meta.permission;
  if (permission != "") {
    if (permission != undefined) {
      if (userPermissions != null) {
        var result = userPermissions.filter(function (elem) {
          if (elem.permission == permission) return elem.permission;
        });
        if (result.length <= 0) {
          next({
            path: '*',
          })
        }
      }
      else {
        next({
          path: '*',
        })
      }
    }
  }
  const adminAuth = to.matched.some(record => record.meta.adminAuth);
  if (store.state.login.isAdminAuthenticated) {
    if (adminAuth == true) {
      debugger;
      //routes.push('/');
    //  router.push('/')
      next({
        path: '*',
      })
    }
    else if ((requiresAuth && !store.state.login.isAuthenticated))
      next({
        path: 'Login',
        // query: { redirectFrom: to.fullPath },
      })
    else next()
  }
  if (!store.state.login.isAdminAuthenticated) {
    if (requiresAuth && !store.state.login.isAuthenticated )
      next({
        name: 'Login',
        // query: { redirectFrom: to.fullPath },
      })
    else next()
  }

  sidebar1.install(Vue, sidebarLinks);
}
router.beforeEach(waitForStorageToBeReady)
/* eslint-disable no-new */
new Vue({
  
  store,
  el: '#app',
  render: h => h(App),
  router
})

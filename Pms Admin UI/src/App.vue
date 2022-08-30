<template>
  <div :class="{'nav-open': $sidebar.showSidebar}">
    <notifications transition-name="notification-list" transition-mode="out-in">

    </notifications>
    <transition name="fade"
                mode="out-in">
      <router-view></router-view>
    </transition>
  </div>
</template>


<script>
  export default {
    data: () => ({
      bgInterval: null,
    }),
    created() {
      this.setupBg()
    },
    methods: {
      setupBg() {
        this.bgInterval = setInterval(this.periodicCheck, 1 * 60 * 1000)
      },
      periodicCheck() {
        try {
          if (this.$route.meta.requiresAuth) this.$store.dispatch('login/refresh')
        } catch (e) {

        }
        
      },
    },
  }
  import('sweetalert2/dist/sweetalert2.css')
  import('vue-notifyjs/themes/default.css')
</script>
<style lang="scss">
  .notifications.vue-notifyjs {
    .notification-list-move {
      transition: transform 0.3s, opacity 0.4s;
    }
    .notification-list-item {
      display: inline-block;
      margin-right: 10px;

    }
    .notification-list-enter-active, .notification-list-leave-active {
      transition: opacity 0.4s;
    }
    .notification-list-enter, .notification-list-leave-to /* .list-leave-active for <2.1.8 */
    {
      opacity: 0;
    }
  }
  .ui.fluid.dropdown > .dropdown.icon {
    float: right;
    margin-top: 10px;
  }
  a.label:focus, a.label:hover {
    color: #66615b !important;

  text-decoration: none;
    cursor: pointer;
  }
  .card .card-content {
    padding: 1px 15px 11px 17px !important;
  }
  .form-control {
     background-color: #ffffff !important; 
    border: 1px solid #e8e7e3;
    border-radius: 4px;
    color: #66615b;
    font-size: 14px;
    padding: 7px 18px;
    height: 40px;
    -webkit-box-shadow: none;
    box-shadow: none;
  }
 .lbltab {
     float:left !important;
 }
 .headtab{
     margin-right:80%;
 }

  .paggingcss{
      margin-top:32px;
  }
  .headersize{
      font-size:18px;
  }
  .el-tag.el-tag--info {
    background-color: #f0f7fa;
    border-color: #e1f0f4;
    color: #090c0c !important;
  }
  .exceldowload {
    float: right;
    height: 26px;
    width: 36px;
  }
  /*.tabEditValidmsg{
      margin-right:200px;
  }*/
</style>

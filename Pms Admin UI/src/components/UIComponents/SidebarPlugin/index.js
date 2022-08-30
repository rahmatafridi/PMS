import Sidebar from './SideBar.vue'
import SidebarItem from './SidebarItem.vue'
import store from '../../../store/index'

const SidebarStore = {
  showSidebar: false,
  sidebarLinks: [],
  clientId:'',
  isMinimized: false,
  displaySidebar (value) {
    this.showSidebar = value
  },
  toggleMinimize () {
    document.body.classList.toggle('sidebar-mini')
    // we simulate the window Resize so the charts will get updated in realtime.
    const simulateWindowResize = setInterval(() => {
      window.dispatchEvent(new Event('resize'))
    }, 180)

    // we stop the simulation of Window Resize after the animations are completed
    setTimeout(() => {
      clearInterval(simulateWindowResize)
    }, 1000)

    this.isMinimized = !this.isMinimized
  }
}

const SidebarPlugin = {

  install(Vue, options) {
    SidebarStore.sidebarLinks = [];
 
    SidebarStore.clientId = window.localStorage.getItem("clientId")
    if (options) {
      options.forEach((value, index) => {
        /* arr.push(value);*/
        debugger;
        var userPermissions = store.state.login.permissions;

        if (value.permisson == 'dashboard') {
          SidebarStore.sidebarLinks.push(value)
        }
        else {
          var result = userPermissions.filter(function (elem) {
            if (elem.permission == value.permisson) return elem.permission;
          });
          if (result.length > 0) {
            SidebarStore.sidebarLinks.push(value)
          }
          //if (value.permisson == 'dashboard') {
          //  SidebarStore.sidebarLinks.push(value)
          //}
        }
       
      });
    //  SidebarStore.sidebarLinks = options.sidebarLinks
    }
    Vue.mixin({
      data () {
        return {
          sidebarStore: SidebarStore
        }
      }
    })

    Object.defineProperty(Vue.prototype, '$sidebar', {
     
      get () {
        return this.$root.sidebarStore
      }
    }

    )
  Vue.component('side-bar', Sidebar)
   Vue.component('sidebar-item', SidebarItem)
  }
}

export default SidebarPlugin

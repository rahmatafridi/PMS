
import Vue from 'vue'
import Router from 'vue-router'
import store from '../store/index'

Vue.use(Router)

import DashboardLayout from '../components/Dashboard/Layout/DashboardLayout.vue'
// GeneralViews
import NotFound from '../components/GeneralViews/NotFoundPage.vue'
// Dashboard pages
import Overview from 'src/components/Dashboard/Views/Dashboard/Overview.vue'


import Login from 'src/components/Dashboard/Views/Pages/Login.vue'

import Clinets from '../components/Clients/ClientList.vue';
import AddClient from '../components/Clients/AddClient.vue';
import EditClient from '../components/Clients/EditClient.vue';
import Roles from '../components/Role/RoleList.vue';
import AddRole from '../components/Role/AddRole.vue';
import EditRole from '../components/Role/EditRole.vue';

import Providers from '../components/Provider/ProviderList.vue';
import AddProvider from '../components/Provider/AddProvider.vue';
import EditProvider from '../components/Provider/EditProivder.vue';
import Users from '../components/User/UserList.vue';
import AddUser from '../components/User/AddUser.vue';
import EditUser from '../components/User/EditUser.vue';
import Property from '../components/Property/PropertyList.vue';
import AddProperty from '../components/Property/AddProperty.vue';
import EditProperty from '../components/Property/EditProperty.vue';


import TenantList from '../components/Tenant/TenantList.vue';
import AddTenant from '../components/Tenant/AddTenant.vue';

import Compliance from '../components/Compliance/Compliance.vue'

import OptionHeader from '../components/ListManagement/OptionHeader/OptionHeader.vue';
import Option from '../components/ListManagement/Options/Option.vue';
import EditTenantVue from '../components/Tenant/EditTenant.vue';

import Config from '../components/Config/ConfigList.vue';
import AddConfigVue from '../components/Config/AddConfig.vue';
import EditConfig from '../components/Config/EditConfig.vue';
import TenantReport from '../components/Report/TenantReport.vue';
import EmptyRoomReport from '../components/Report/EmptyRoomReport.vue';
import MissingDoc from '../components/Report/MissingDocReport.vue';
import ExpiryDocReportVue from '../components/Report/ExpiryDocReport.vue';

import Dashobard from '../components/Dashboard/Dashboard.vue';
import UserProfileVue from '../components/UserProfile/UserProfile.vue'
import Permission from '../components/Permission/AddEditPermission.vue'
const routes = [
  {
    path: '',
    component: Login,
    children: [
      {
        name: 'Login',
        path: '',
        component: Login,
      }
    ],
  },
  {
    path: '/dashboard',
    component: DashboardLayout,
   
    children: [
      // Dashboard
      {
        name: 'Dashboard',
        path: '/dashboard',
        component: Dashobard,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission:''
          // title: 'Users',
        },
      },
      // Pages


    ],
  },
  {
    path: '/Client',
    component: DashboardLayout,
    children: [
      {
        path: '/clients',
        name: 'Clients',
        component: Clinets,
        meta: {
          requiresAuth: true,
          adminAuth: true,

          title: 'Clients',
          permission: ''
        },
      },
      {
        path: '/client/add',
        name: 'Add',
        component: AddClient,
        meta: {
          requiresAuth: true,
          adminAuth: true,
          permission: ''
          // title: 'Users',
        },
      },
      {
        path: '/client/edit/:id?',
        name: 'Edit',
        component: EditClient,
        meta: {
          requiresAuth: true,
          adminAuth: true,
          permission: ''
          // title: 'Users',
        },
      }

    ]
  },
  {
    path: '/Roles',
    component: DashboardLayout,
    children: [
      {
        path: '/roles',
        name: 'Role',
        component: Roles,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission:''
          // title: 'Users',
        },

      },
      {
        path: '/role/add',
        name: 'Add',
        component: AddRole,
        meta: {
          requiresAuth: true,
          permission:'MD_CONFIG_CREATE'
          // title: 'Users',
        },
      },
      {
        path: '/role/Edit/:id?',
        name: 'Edit',
        component: EditRole,
        meta: {
          requiresAuth: true,
          permission:'MD_CONFIG_EDIT1'
          // title: 'Users',
        },
      }
    ]
  },
  {
    path: '/Providers',
    component: DashboardLayout,
    children: [
      {
        path: '/providers',
        name: 'Providers',
        component: Providers,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/provider/add',
        name: 'Add',
        component: AddProvider,
        meta: {
          requiresAuth: true,
          permission: '',
           adminAuth: false,
          // title: 'Users',
        },
      },
      {
        path: '/provider/edit/:id?',
        name: 'Edit',
        component: EditProvider,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },
      },
    ]
  },
  {
    path: '/Users',
    component: DashboardLayout,
    children: [
      {
        path: '/users',
        name: 'Users',
        component: Users,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/users/add',
        name: 'Add',
        component: AddUser,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/users/edit/:id?',
        name: 'Edit',
        component: EditUser,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
    ]
  },
  {
    path: '/Property',
    component: DashboardLayout,
    children: [
      {
        path: '/property',
        name: 'Property',
        component: Property,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/property/add',
        name: 'Add',
        component: AddProperty,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/property/edit/:id?',
        name: 'Edit',
        component: EditProperty,
        meta: {
          requiresAuth: true,
          adminAuth: false,
          permission: ''
          // title: 'Users',
        },
      },
    ]
  },
  {
    path: '/Tenants',
    component: DashboardLayout,
    children: [
      {
        path: '/tenants',
        name: 'Tenant',
        component: TenantList,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/tenants/add',
        name: 'Add',
        component: AddTenant,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/tenants/edit/:id?',
        name: 'Edit',
        component: EditTenantVue,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }
    ]
  },
  {
    path: '/Compliance',
    component: DashboardLayout,
    children: [
      {
        path: '/compliances',
        name: 'Compliance',
        component: Compliance,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/OptionHeader',
    component: DashboardLayout,
    children: [
      {
        path: '/optionheader',
        name: 'OptionHeder',
        component: OptionHeader,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/ListManagement',
    component: DashboardLayout,
    children: [
      {
        path: '/listmanagement',
        name: 'List Management',
        component: Option,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/TenantReport',
    component: DashboardLayout,
    children: [
      {
        path: '/tenantreport',
        name: 'Tenant Report',
        component: TenantReport,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/EmptyRoomReport',
    component: DashboardLayout,
    children: [
      {
        path: '/emptyroom',
        name: 'Empty Rooms Report',
        component: EmptyRoomReport,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/MissingDocumnet',
    component: DashboardLayout,
    children: [
      {
        path: '/missingdoc',
        name: 'Missing Document Report',
        component: MissingDoc,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/userprofile',
    component: DashboardLayout,
    children: [
      {
        path: '/userprofile',
        name: 'User Profile',
        component: UserProfileVue,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/Permission',
    component: DashboardLayout,
    children: [
      {
        path: '/permission/addEdit/:id?',
        name: 'Permissions',
        component: Permission,
        meta: {
          requiresAuth: true,
          permission: ''
          /*adminAuth: true,*/
          // title: 'Users',
        },
      }

    ]
  },

  {
    path: '/Document Expiry',
    component: DashboardLayout,
    children: [
      {
        path: '/docexpiry',
        name: 'Document Expiry Report',
        component: ExpiryDocReportVue,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }

    ]
  },
  {
    path: '/Config',
    component: DashboardLayout,
    children: [
      {
        path: '/configs',
        name: 'Config',
        component: Config,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/configs/add',
        name: 'Add',
        component: AddConfigVue,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      },
      {
        path: '/configs/edit/:id?',
        name: 'Edit',
        component: EditConfig,
        meta: {
          requiresAuth: true,
          permission: ''
          // title: 'Users',
        },

      }
    ]
  },
  {
    path: '*',
    component: DashboardLayout,
    children: [
      {
        name: 'Access Denied',
        path: '',
        component: NotFound,
      },
    ],
  }
]


//const router = new Router({
//        mode: "history",

//  routes
//})



export default routes


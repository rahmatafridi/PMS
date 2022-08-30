export default [
  {
    name: 'Dashboard',
    icon: 'ti-panel',
    path: '/dashboard',
    permisson: 'dashboard',
    //collapsed: false,
    //children: [{
    //  name: 'Overview',
    //  path: '/admin/overview'
    //}
    //]
  },
 
  {
    name: 'Clients',
    icon: 'ti-gift',
    path: '/clients',
    permisson:'MD_CLIENT_VIEW',
  },



  
  {
    name: 'Properties',
    icon: 'ti-gift',
    path: '/property',
    permisson: 'MD_PROP_VIEW',

  },
  {
    name: 'Providers',
    icon: 'ti-gift',
    path: '/providers',
    permisson:'MD_PROV_VIEW',
  },
  {
    name: 'Tenants',
    icon: 'ti-gift',
    path: '/tenants',
    permisson: 'MD_TNT_VIEW',

  },
  {
    name: 'Reports',
    icon: 'ti-panel',
    permisson: 'MD_REPORT_VIEW',

    children: [

      {
        name: 'Tenant',
        path: '/tenantreport',
      },
      {
        name: 'Empty Room',
        path: '/emptyroom',
      },
      {
        name: 'Missing Doc',
        path: '/missingdoc',
      },
      {
        name: 'Document Expiry',
        path: '/docexpiry',
      }

    ]
  },
  {
    name: 'User Management',
    icon: 'ti-panel',
    permisson: 'MD_USER_VIEW',

    children: [

      {
        name: 'Roles',
        path: '/roles',
      },
      {
        name: 'Users',
        path: '/users',
      },
    ]
  },
  {
    name: 'List Management',
    icon: 'ti-panel',
    permisson: 'MD_LIST_VIEW',

    children: [

      {
        name: 'Compliance',
        path: '/compliances',
      },
      {
        name: 'Option Header',
        path: '/optionheader',
      },
       {
        name: 'List Management',
        path: '/listmanagement',
      },
      {
        name: 'Configs',
        path:'/configs'
      }
    ]
  },


 
]

import ApiService from './api.service'
const apiService = new ApiService('', true);
const RoleApi = {
  role: {
    get (roleId) {
      return apiService.query('/v1/role/getrolebyid', { roleId })
    },
    list (params) {
      return apiService.query('/v1/role/getroles', params)
    },
    add (addRole) {
      return apiService.post('/v1/role/add', addRole)
    },
    update (updateRole) {
      return apiService.post('/v1/role/update', updateRole)
    },
    delete (roleId) {
      return apiService.deleteWithParames('/v1/role/delete', { roleId })
    },
    getrolesbyclientid (clientId) {
      return apiService.query('/v1/role/getrolesbyclientid', { clientId })
    },
    getRolePermission(params) {
      debugger;
      return apiService.query('v1/role/getpermission', params )
    },
    addPermission(params) {
      return apiService.post('/v1/role/addpermission', params)
    },
    validateRoleName (id, clientId, name) {
      return apiService.query('/v1/role/validaterolename', {
        id,
        clientId,
        name
      })
    }
  }
}

export default RoleApi


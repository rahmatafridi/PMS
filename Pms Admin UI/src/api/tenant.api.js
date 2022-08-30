import ApiService from './api.service'

const apiService = new ApiService('', true)

const TenantApi = {
  tenant: {
    get(tenantId) {
      return apiService.query('/v1/tenant/gettenantbyid', { tenantId })
    },
    list(params) {
      return apiService.query('/v1/tenant/gettenants', params)
    },
    add(addtenant) {
      return apiService.post('/v1/tenant/add', addtenant)
    },
    update(updatetenant) {
      return apiService.put('/v1/tenant/update', updatetenant)
    },
    delete(tenantId) {
      return apiService.deleteWithParames('/v1/tenant/delete', { tenantId })
    },

  }
}

export default TenantApi

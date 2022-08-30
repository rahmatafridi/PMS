import ApiService from './api.service'

const apiService = new ApiService('', true)
const ComplianceApi = {
  compliance: {
    get(complianceId) {
      return apiService.query('/v1/compliance/getcompliancebyid', { complianceId })
    },
    getByClient(clientId) {
      return apiService.query('/v1/compliance/getbyclient', { clientId })
    },
    list(params) {
      return apiService.query('/v1/compliance/getcompliances', params)
    },
    add(add) {

      return apiService.post('/v1/compliance/add', add)
    },
    update(updateComp) {
      return apiService.put('/v1/compliance/update', updateComp)
    },
    delete(id) {
      return apiService.deleteWithParames('/v1/compliance/delete', { id })
    },

  }
}

export default ComplianceApi

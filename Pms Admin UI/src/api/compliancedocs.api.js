import ApiService from './api.service'

const apiService = new ApiService('', true)
const ComplianceDocApi = {
  compliance: {
    get(complianceId) {
      return apiService.query('/v1/compliancePropertyDoc/getcompliancebyid', { complianceId })
    },
    getPropDoc(compliancePropertyDocId) {
      return apiService.query('/v1/compliancePropertyDoc/getcompliancePropertyDocbyid', { compliancePropertyDocId })
    },
    list(params) {
      return apiService.query('/v1/compliancePropertyDoc/getcompliancePropertyDocs', params)
    },
    add(data) {
      return apiService.post('/v1/compliancePropertyDoc/add', data)
    },
    update(updateComp) {
      return apiService.put('/v1/compliancePropertyDoc/update', updateComp)
    },
    delete(id) {
      return apiService.deleteWithParames('/v1/compliancePropertyDoc/delete', { id })
    },

  }
}

export default ComplianceDocApi

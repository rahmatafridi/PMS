import ApiService from './api.service'

const apiService = new ApiService('', true)
const OptionHeaderApi = {
  header: {
    get(headerId) {
      return apiService.query('/v1/optionheader/getheaderbyid', { headerId })
    },
    getList(clientId) {
      return apiService.query('/v1/optionheader/getheaderlistbyid', { clientId })
    },
    list(params) {
      return apiService.query('/v1/optionheader/getheader', params)
    },
    add(addHeader) {
      debugger;

      return apiService.post('/v1/optionheader/add', addHeader)
    },
    update(updateHeader) {
      debugger;
      return apiService.put('/v1/optionheader/update', updateHeader)
    },
    delete(headerId) {
      return apiService.deleteWithParames('/v1/optionheader/delete', { headerId })
    },

  }
}

export default OptionHeaderApi

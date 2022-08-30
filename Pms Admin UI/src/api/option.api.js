import ApiService from './api.service'

const apiService = new ApiService('', true)
const OptionApi = {
  option: {
    get(optionId) {
      return apiService.query('/v1/option/getoptionbyid', { optionId })
    },
    getList(headerId) {
      return apiService.query('/v1/option/getoptionlistbyid', { headerId })
    },
    list(params) {
      return apiService.query('/v1/option/getheader', params)
    },
    add(addOption) {

      return apiService.post('/v1/option/add', addOption)
    },
    update(updateOption) {
      return apiService.put('/v1/option/update', updateOption)
    },
    delete(optionId) {
      return apiService.deleteWithParames('/v1/option/delete', { optionId })
    },
    validateOption(prams) {
      return apiService.query('/v1/option/validateoption', prams )
    },
    getOptionByHeader(header) {
      return apiService.query('/v1/option/getoptionheader', {header})
    }
  }
}

export default OptionApi

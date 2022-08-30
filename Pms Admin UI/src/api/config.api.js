import ApiService from './api.service'
const apiService = new ApiService('', true)

const ConfigApi = {
  config: {
    get(id) {
      return apiService.query('/v1/config/getconfigbyid', { id })
    },
    list(params) {
      return apiService.query('/v1/config/getconfigs', params)
    },
    add(config) {
      return apiService.post('/v1/config/add', config)
    },
    update(updateConfig) {
      return apiService.put('/v1/config/update', updateConfig)
    },
    delete(configId) {
      return apiService.deleteWithParames('/v1/config/delete', { configId })
    }
  }
}

export default ConfigApi

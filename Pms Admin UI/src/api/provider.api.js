import ApiService from './api.service'
const apiService = new ApiService('', true)

const ProviderApi = {
  provider: {
    get (providerId) {
      return apiService.query('/v1/provider/getproviderbyid', { providerId })
    },
    list(params) {
      return apiService.query('/v1/provider/getproviders', params)
    },
    add (provider) {
      return apiService.post('/v1/provider/add', provider)
    },
    update (updateprovider) {
      return apiService.put('/v1/provider/update', updateprovider)
    },
    delete (providerId) {
      return apiService.deleteWithParames('/v1/provider/delete', { providerId })
    },
    adduser(provider) {
      return apiService.post('/v1/provider/addprovideruser', provider)
    },
    updateUser(updateprovider) {
      return apiService.put('/v1/provider/updateprovideruser', updateprovider)
    },
    getUser(providerId) {
      return apiService.query('/v1/provider/getprovideruserbyid', { providerId })
    },
    validateEmail (providerId, email) {
      return apiService.query('/v1/provider/validateemail', {
        providerId,
        email
      })
    }
  }
}

export default ProviderApi

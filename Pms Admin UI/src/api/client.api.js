import ApiService from './api.service'
const apiService = new ApiService('', true)

const ClientApi = {
    client: {
      get(clientId) {
        return apiService.query('/v1/client/getclientbyid', { clientId })
      },
    list(params) {
        return apiService.query('/v1/client/getclients', params)
      },
      add(client) {
        return apiService.post('/v1/client/add', client)
      },
      update(updateClient) {
        return apiService.put('/v1/client/update', updateClient)
      },
      delete(clientId) {
        return apiService.deleteWithParames('/v1/client/delete', { clientId })
      },
    validateEmail(prame) {

        return apiService.query('/v1/client/validateemail', prame)
      },
      copyRoles(clientId) {
        return apiService.query('/v1/client/copyroles', { clientId })
      },
    }
}

export default ClientApi

import ApiService from './api.service'
const apiService = new ApiService('', true)

const DashbaordApi = {
  dashboard: {
    loadData(clientId) {
      return apiService.query('/v1/dashboard/getdata', { clientId })
    }

  }
}

export default DashbaordApi

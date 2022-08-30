import ApiService from './api.service'

const apiService = new ApiService('', true)

const RoomHistoryApi = {
  room: {
    get(tenantId) {
      return apiService.query('/v1/roomhistory/getroombyid', { tenantId })
    }
    

  }
}

export default RoomHistoryApi

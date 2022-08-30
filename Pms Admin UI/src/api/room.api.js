import ApiService from './api.service'
const apiService = new ApiService('', true);
const RoomApi = {
  room: {
    get(id) {
      return apiService.query('/v1/room/getroombyid', { id })
    },
    list(params) {
      return apiService.query('/v1/room/getrooms', params)
    },
    add(addRole) {
      return apiService.post('/v1/room/add', addRole)
    },
    update(update) {
      return apiService.post('/v1/room/update', update)
    },
    delete(roleId) {
      return apiService.deleteWithParames('/v1/room/delete', { roleId })
    },
    
  }
}

export default RoomApi


import ApiService from './api.service'

const apiService = new ApiService('', true)
const UserApi = {
  authorize(auth) {

    return apiService.post('/v1/user/authenticate', auth)
  },
  reauthorize(auth) {
    return apiService.post('/v1/user/reauthenticate', auth)
  },
  user: {
    get(userId) {
      return apiService.query('/v1/user/getuserbyid', { userId })
    },
    list(params) {
      return apiService.query('/v1/user/getusers', params)
    },
    add(user) {
      return apiService.post('/v1/user/add', user)
    },
    update(user) {
      return apiService.put('/v1/user/update', user)
    },
    delete(userId) {
      return apiService.deleteWithParames('/v1/user/delete', { userId })
    },
    assignrolestouser(assignRolesToUser) {
      return apiService.post('/v1/user/assignrolestouser', assignRolesToUser)
    },
    changePassword(userId, password) {
      return apiService.post(`/v1/user/changepassword`, {
        UserId: userId,
        Password: password,
      })
    },
    validateUsername(userId, username) {
      return apiService.query('/v1/user/validateUsername', {
        userId,
        username,
      })
    },
    validateEmail(userId, email) {
      return apiService.query('/v1/user/validateEmail', {
        userId,
        email,
      })
    },
    getrolesbyuserid(userId) {
      return apiService.query('/v1/user/getrolesbyuserid', { userId })
    },
    listUserByClient(params) {
      return apiService.query('/v1/user/getusersbyclient', params)
    },
  },
}

export default UserApi

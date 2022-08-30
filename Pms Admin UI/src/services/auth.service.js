class AuthService {
  setToken(token) {
    localStorage.setItem('token', token)
  }

  removeToken() {
    localStorage.removeItem('token')
  }

  getToken() {
    return localStorage.getItem('token')
  }
}

export default new AuthService()

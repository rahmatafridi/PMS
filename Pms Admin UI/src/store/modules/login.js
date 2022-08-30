import jwtDecode from 'jwt-decode'

import UserApi from '@/api/user.api'
//import router from '../../routes/routes'
import authService from '@/services/auth.service'
import { getErrorMessage } from '../../helpers/error'

const loginInitialState = () => ({
  token: null,
  rememberMe: false,
  user: null,
  postLoginUrl: '/dashboard',
  postLogoutUrl: '/',
  isAuthenticated: false,
  isAdminAuthenticated: false,
  permissions:[]
})

const state = loginInitialState()

const getters = {
  getName: state => id => {
    if (state.user && state.user.Id == id) {
      return `${state.user.displayName}`
    }

    return ''
  },
  currentName: state => {
    if (state.user) {
      return `${state.user.displayName}`
    }

    return ''
  },
  clientId: state => {
    if (state.user) {
      return `${state.user.clientId}`
    }
  },
  role: state => {
    return state.user ? state.user.Role : null
  },
  permissions: state => {
    return state.user.permissionList
  }
}

const actions = {
  async login({ commit }, { auth }) {
    const req = await UserApi.authorize(auth)
    let message = null
    if (req.ok && req.data) {
      const Token = req.data
      commit('setToken', Token)

      authService.setToken(Token)
      const data = jwtDecode(Token)
      if (data) {
        let userReq = await UserApi.user.get(
          data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid']
        )
        if (userReq && userReq.ok) {
          const user = userReq.data
          commit('setUser', user)
          commit('setUserId', user.id)
          window.localStorage.setItem('clientId', user.clientId)
          debugger;
          commit('setClientId', user.clientId)
          commit('setPermissionList', user.permissionList)
          return Promise.resolve(true)
        } else {
          message = getErrorMessage(userReq.error)
          return Promise.reject(message)
        }
      }
      return Promise.resolve(false)
    }
    message = getErrorMessage(req.error)
    return Promise.reject(message)
  },

  logout({ state }) {

    //commit('setUser', null)
    //commit('setToken', null)

    // authService.setToken(null)

//this.$router.push({ path: '/' })
  },

  async refresh({ state, commit, dispatch }) {
    let data = jwtDecode(state.token)
    if (data) {
      const { exp } = data
      if (Date.now() + 15 * 60 * 1000 >= exp * 1000) {
        let req = await UserApi.reauthorize({
          Token: state.token,
        })

        if (req && req.ok && req.data && req.data.Success) {
          let token = req.data.Token

          commit('setToken', token)
          authService.setToken(token)
        } else if (Date.now() + 5 * 60 * 1000 >= exp * 1000) {
          dispatch('logout')
        }
      }
    }
  },
}

const mutations = {
  setToken(state, token) {
    state.token = token
    state.isAuthenticated = !!token
  },
  setUser(state, user) {
    state.user = user
  },
  setUserId(state, userId) {
    state.userId = userId
  },
  setPermissionList(state, permission) {
    debugger;
    state.permissions = permission
  },
  setClientId(state, clientId) {

    state.clientId = clientId
    if (clientId == 0) {
      state.isAdminAuthenticated = false;
    }
    else {
      state.isAdminAuthenticated = true;
    }
  },
  RESET(state) {
    const newState = loginInitialState();
    Object.keys(newState).forEach(key => {
      state[key] = newState[key]
    });
  },
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
}

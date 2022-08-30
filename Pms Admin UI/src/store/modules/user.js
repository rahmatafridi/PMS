import UserApi from '@/api/user.api'
import { getErrorMessage } from '../../helpers/error'
import store from '@/store/index'

const userInitialState = () => ({
  isLoading: false,
  list: {
    page: 1,
    perPage: 10,
    totalCount: 0,
    sort: [],
    search: null,
    items: [],
  },
})

const state = userInitialState()

const getters = {
  get: state => {
    return state.list
  },
}

const actions = {
  fetch: async ({ state, commit }) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'
      let req = await UserApi.user.list({
        page: state.list.page,
        limit: state.list.perPage,
        search: state.list.search || null,
        sort:
          state.list.sort.map(s => `${s.name} ${s.direction}`).join(',') ||
          null,
      })
      if (req.ok && req.data) {
        commit('setListItems', {
          items: req.data.items,
        })
        debugger;

        commit('setListProperty', {
          property: 'totalCount',
          value: req.data.totalCount,
        })
      } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
      }
    } catch (e) {
      return Promise.reject(e)
    } finally {
      commit('setLoading', false)
    }
  },
  fetchclient: async ({ state, commit }) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'
      let req = await UserApi.user.listUserByClient({
        clientId: store.state.login.user.clientId,
        page: state.list.page,
        limit: state.list.perPage,
        search: state.list.search || null,
        sort:
          state.list.sort.map(s => `${s.name} ${s.direction}`).join(',') ||
          null,
      })
      if (req.ok && req.data) {
        commit('setListItems', {
          items: req.data.items,
        })

        commit('setListProperty', {
          property: 'totalCount',
          value: req.data.totalCount,
        })
      } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
      }
    } catch (e) {
      return Promise.reject(e)
    } finally {
      commit('setLoading', false)
    }
  },
  fetchclientuser: async ({ state, commit }, { id}) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'
      let req = await UserApi.user.listUserByClient({
        clientId: id,
        page: state.list.page,
        limit: state.list.perPage,
        search: state.list.search || null,
        sort:
          state.list.sort.map(s => `${s.name} ${s.direction}`).join(',') ||
          null,
      })

      if (req.ok && req.data) {
        commit('setListItems', {
          items: req.data.items,
        })
        commit('setListProperty', {
          property: 'totalCount',
          value: req.data.totalCount,
        })
      } else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
      }
    } catch (e) {
      return Promise.reject(e)
    } finally {
      commit('setLoading', false)
    }
  },

  async delete({ commit }, id) {
    try {
      commit('setLoading', true)

      let req = await UserApi.user.delete(id)
      let message = 'Api error'
      if (req.ok && req.data) {
        return Promise.resolve(true)
      }
      else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
      }
    } catch (e) {
      return Promise.reject(e)
    } finally {
      commit('setLoading', false)
    }
  },

  async userprofile({ commit }, id) {
    try {
      var id1 = store.state.login.user.id;

      
      commit('setLoading', true)
      let req = await UserApi.user.get(id1)
      let message = 'Api error'
      if (req.ok && req.data) {
        return Promise.resolve(true)
      }
      else {
        message = getErrorMessage(req.error)
        return Promise.reject(message)
      }
    } catch (e) {
      return Promise.reject(e)
    } finally {
      commit('setLoading', false)
    }
  },

}

const mutations = {
  setLoading(state, isLoading) {
    state.isLoading = isLoading
  },
  setListItems(state, { nextPage, items }) {
    if (nextPage) state.ist.items.push(...items)
    else state.list.items = items
  },
  setListProperty(state, { property, value }) {
    state.list[property] = value
    if (property == 'page' && value == 1) state.list.items = []
  },
  RESET(state) {
    const newState = userInitialState();
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

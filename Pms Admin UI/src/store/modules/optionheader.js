import OptionHeaderApi from '@/api/optionheader.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'

const headerInitialState = () => ({
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

const state = headerInitialState()

const getters = {
  get: state => {
    return state.list
  },
}

const actions = {
  fetchList: async ({ state, commit }) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'
      let req = await OptionHeaderApi.header.list({
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

        commit('setListHeader', {
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

  delete: async ({ commit }, id) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'

      let req = await OptionHeaderApi.header.delete(id)
      if (req.ok && req.data) {
        commit('removeItem', id)
        return Promise.resolve(true)
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
}

const mutations = {
  setLoading(state, isLoading) {
    state.isLoading = isLoading
  },
  setListItems(state, { nextPage, items }) {
    if (nextPage) state.list.items.push(...items)
    else state.list.items = items
  },
  setListHeader(state, { header, value }) {
    state.list[header] = value

    if (note == 'page' && value == 1) state.list.items = []
  },
  removeItem(state, id) {
    state.list.items = state.list.items.filter(u => u.id != id)
  },
  RESET(state) {
    const newState = headerInitialState();
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

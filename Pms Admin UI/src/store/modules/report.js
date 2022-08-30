import ReportApi from '@/api/report.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'

const reportInitialState = () => ({
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

const state = reportInitialState()

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
      let req = await ReportApi.report.emptyRoomReport()
      if (req.ok && req.data) {
        state.list.items = req.data;
        commit('setListItems', {
          items: req.data,
        })

        commit('setListReport', {
          report: 'totalCount',
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
 
}

const mutations = {
  setLoading(state, isLoading) {
    state.isLoading = isLoading
  },
  setListItems(state, { nextPage, items }) {
    if (nextPage) state.list.items.push(...items)
    else state.list.items = items
  },
  setListReport(state, { report, value }) {
    state.list[report] = value

    if (report == 'page' && value == 1) state.list.items = []
  },
  removeItem(state, id) {
    state.list.items = state.list.items.filter(u => u.id != id)
  },
  RESET(state) {
    const newState = reportInitialState();
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

import ReportApi from '@/api/report.api'
import { getErrorMessage } from '@/helpers/error'
import store from '@/store/index'

const tenantReportInitialState = () => ({
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

const state = tenantReportInitialState()

const getters = {
  get: state => {
    return state.list
  },
}

const actions = {
  fetchList: async ({ state, commit }, { auth }) => {
    try {
      commit('setLoading', true)
      let message = 'Api error'
      let req = await ReportApi.report.tenantReport({
        typeId: auth.typeId
      })
      if (req.ok && req.data) {
        commit('setListItems', {
          items: req.data,
        })

        commit('setTenantListReport', {
          report: 'totalCount',
          value: req.data,
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
  setTenantListReport(state, { report, value }) {
    state.list[report] = value

    if (report == 'page' && value == 1) state.list.items = []
  },
  removeItem(state, id) {
    state.list.items = state.list.items.filter(u => u.id != id)
  },
  RESET(state) {
    const newState = tenantReportInitialState();
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

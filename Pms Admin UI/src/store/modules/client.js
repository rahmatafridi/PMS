import ClientApi from '@/api/client.api'
import { getErrorMessage } from '@/helpers/error'

const clientInitialState = () => ({
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

const state = clientInitialState()

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
            let req = await ClientApi.client.list({
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

    delete: async ({ commit }, id) => {
        try {
            commit('setLoading', true)
            let message = 'Api error'

            let req = await ClientApi.client.delete(id)
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
        if (nextPage) state.ist.items.push(...items)
        else state.list.items = items
    },
    setListProperty(state, { property, value }) {
        state.list[property] = value

        if (property == 'page' && value == 1) state.list.items = []
    },
    removeItem(state, id) {
        state.list.items = state.list.items.filter(u => u.id != id)
    },
    RESET(state) {
        const newState = clientInitialState();
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

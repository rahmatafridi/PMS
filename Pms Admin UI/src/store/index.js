import localForage from 'localforage'
import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'
Vue.use(Vuex)

function loadModules() {
  const context = require.context('./modules', false, /([a-z_]+)\.js$/i)

  const modules = context
    .keys()
    .map(key => ({ key, name: key.match(/([a-z_]+)\.js$/i)[1] }))
    .reduce(
      (modules, { key, name }) => ({
        ...modules,
        [name]: context(key).default,
      }),
      {},
    )

  return { context, modules }
}

const { context, modules } = loadModules()


const debug = process.env.NODE_ENV !== 'production'

const vuexLocal = new VuexPersistence({
  storage: localForage,
  asyncStorage: true,
  modules: ['login'],
})

const store = new Vuex.Store({
  modules,
  strict: debug,
  plugins: [vuexLocal.plugin],
  state: {
    barColor: 'rgba(0, 0, 0, .8), rgba(0, 0, 0, .8)',
    barImage: 'https://demos.creative-tim.com/material-dashboard-pro/assets/img/sidebar-1.jpg',
    drawer: null,
  },
  actions: {
    reset({ commit }) {
      
      // resets state of all the modules
      Object.keys(modules).forEach(moduleName => {
        commit(`${moduleName}/RESET`);
      })
    },
  },
  mutations: {
    SET_BAR_IMAGE(state, payload) {
      state.barImage = payload
    },
    SET_DRAWER(state, payload) {
      state.drawer = payload
    },
    SET_SCRIM(state, payload) {
      state.barColor = payload
    },
  },
})

export default store

if (module.hot) {
  // Hot reload whenever any module changes.
  module.hot.accept(context.id, () => {
    const { modules } = loadModules()

    store.hotUpdate({
      modules,
    })
  })
}

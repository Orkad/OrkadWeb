export default {
  namespaced: true,
  state: {
    profile: {}
  },
  getters: {
    isAuthenticated: state => state.profile.name && state.profile.email
  },
  mutations: {
    setProfile (state, profile) {
      state.profile = profile
    },
  },
  actions: {
    login ({ commit }, credentials) {
      return axios.post('account/login', credentials).then(res => {
        commit('setProfile', res.data)
      })
    },
    logout ({ commit }) {
      return axios.post('account/logout').then(() => {
        commit('setProfile', {})
      })
    }
  }
}
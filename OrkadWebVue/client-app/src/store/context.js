import AccountApi from '@/services/api/AccountApi'

export default {
  namespaced: true,
  state: {
    profile: {}
  },

  getters: {
    isAuthenticated: state =>  (state.profile.email) && (state.profile.name)
  },

  mutations: {
    setProfile(state, profile) {
      state.profile = profile
    },
  },
  actions: {
    refresh({ commit }) {
      return AccountApi.refresh()
        .then(data => { commit('setProfile', data); return data; });
    },
    login({ commit }, credentials) {
      return AccountApi.login(credentials.username, credentials.password)
        .then(data => { commit('setProfile', data); return data; });
    },
    logout({ commit }) {
      return AccountApi.logout()
        .then(() => { commit('setProfile', {}) });
    }
  }
}
import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

// Vue material
// import { MdButton, MdContent, MdTabs, MdField, MdApp } from 'vue-material/dist/components'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'

// Vue.use(MdButton)
// Vue.use(MdContent)
// Vue.use(MdTabs)
// Vue.use(MdField)
// Vue.use(MdApp)

import VueMaterial from 'vue-material'
Vue.use(VueMaterial)

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
  router,
  store
}).$mount('#app')

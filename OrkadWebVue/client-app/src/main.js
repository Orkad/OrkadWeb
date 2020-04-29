import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

// vuetify plugin
import vuetify from "@/plugins/vuetify.js";
import "material-design-icons-iconfont/dist/material-design-icons.css";

// moment plugin
import VueMoment from "vue-moment";
Vue.use(VueMoment);

// lodash lib
import _ from 'lodash'
Vue.prototype._ = _

Vue.config.productionTip = false;

new Vue({
  render: (h) => h(App),
  router,
  store,
  vuetify,
}).$mount("#app");
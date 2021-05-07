import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import store from "./store/store";
import appConfig from "./appConfig";
import Vuebar from "vuebar";
import "./plugins/base";
import VueSkycons from "vue-skycons";
import GAuth from 'vue-google-oauth2';

const authConfig = {
  clientId: appConfig.oauthClientId,
  scope: 'profile email',
  prompt: 'select_account'
};
Vue.use(GAuth, authConfig);

Vue.use(VueSkycons, {
  color: "#1e88e5",
});

Vue.config.productionTip = false;
Vue.use(Vuebar);

new Vue({
  vuetify,
  store,
  router,
  render: (h) => h(App),
}).$mount("#app");

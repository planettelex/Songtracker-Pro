import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import router from "./router";
import store from "./store/store";
import Vuebar from "vuebar";
import "./plugins/base";
import VueSkycons from "vue-skycons";
import GAuth from 'vue-google-oauth2';

const authConfig = {
  clientId: '1084971919395-tkrcdm6pm9fal8d21c4vk552gpqmvq6o.apps.googleusercontent.com',
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

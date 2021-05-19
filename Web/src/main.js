import Vue from "vue";
import VueI18n from 'vue-i18n';
import App from "./App.vue";
import axios from 'axios';
import { Model } from 'vue-api-query';
import vuetify from "./plugins/vuetify";
import router from "./router";
import store from "./store/store";
import appConfig from "./appConfig";
import dateTimeFormats from "./resources/dateTimeFormats";
import numberFormats from "./resources/numberFormats";
import messages from "./resources/messages";
import Vuebar from "vuebar";
import "./plugins/base";
import VueSkycons from "vue-skycons";
import GAuth from 'vue-google-oauth2';

Model.$http = axios;

const authConfig = {
  clientId: appConfig.oauthClientId,
  scope: 'profile email',
  prompt: 'select_account'
};
Vue.use(GAuth, authConfig);

Vue.use(VueI18n);
const i18n = new VueI18n({
  locale: appConfig.locale,
  dateTimeFormats: dateTimeFormats,
  numberFormats: numberFormats,
  messages: messages
});

Vue.use(VueSkycons, {
  color: "#1e88e5",
});

Vue.config.productionTip = false;
Vue.use(Vuebar);

new Vue({
  i18n,
  vuetify,
  store,
  router,
  render: (h) => h(App),
}).$mount("#app");

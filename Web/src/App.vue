<template>

  <v-app id="songtrackerpro" :class="`${!$vuetify.breakpoint.smAndDown ? 'full-sidebar' : 'mini-sidebar'}`">
      <router-view />
  </v-app>

</template>

<script>
import UserType from "./enums/UserType";
import ApplicationData from './models/api/Application';
import { mapState } from "vuex";

export default {
  name: 'App',

  components: { },

  computed: {
    ...mapState(["User"]),
    ...mapState(["Application"]),
    Application: {
      get() { return this.$store.state.Application; },
      set(val) { this.$store.commit("SET_APPLICATION", val); }
    },
  },

  methods: {
    async getAppInfo() {
      return await ApplicationData.first();
    },

    setDocumentTitle(appTitle, tagline, userType, pageTitle) {
      let documentTitle = '';
      if (pageTitle) documentTitle = pageTitle + " | " + appTitle;
      else documentTitle = appTitle;

      if (userType == UserType.SystemAdministrator || userType == UserType.SystemUser) {
          documentTitle += ' - ' + tagline;
      }

      document.title = documentTitle;
    },

    getUserType() {
      if (this.User && this.User.type)
        return this.User.type;
      else
        return UserType.Unsassigned;
    },

    getPageTitle(page) {
      if (page.meta.titleKey)
        return this.$tc(page.meta.titleKey, 2);
      else if (page.meta.title)
        return page.meta.title;

      return null;
    }
  },

  watch: {
    $route: {
      immediate: true,
      handler(to) {
        if (this.Application) {
          let appTitle = this.Application.entityName ? this.Application.entityName : this.Application.name;
          let userType = this.getUserType();
          let pageTitle = this.getPageTitle(to);
          this.setDocumentTitle(appTitle, this.Application.tagline, userType, pageTitle);
        }
        else {
          this.getAppInfo().then(response => {
            this.Application = response;
            let appTitle = response.name;
            let userType = this.getUserType();
            let pageTitle = this.getPageTitle(to);
            this.setDocumentTitle(appTitle, response.tagline, userType, pageTitle);
          });
        }
      }
    }
  }
};
</script>

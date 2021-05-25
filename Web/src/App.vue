<template>

  <v-app id="songtrackerpro" :class="`${!$vuetify.breakpoint.smAndDown ? 'full-sidebar' : 'mini-sidebar'}`">
      <router-view />
  </v-app>

</template>

<script>
import UserType from "./enums/UserType";
import { mapState } from "vuex";

export default {
  name: 'App',
  components: {
    
  },
  computed: {
    ...mapState(["AppInfo", "User"])
  },
  watch: {
    $route: {
      immediate: true,
      handler(to) {
        let appTitle = this.AppInfo.entityName ? this.AppInfo.entityName : this.AppInfo.name;
        let userType = UserType.Unsassigned;
        if (this.User && this.User.type)
          userType = this.User.type;

        if (userType == UserType.SystemAdministrator || userType == UserType.SystemUser) {
          appTitle += ' - ' + this.AppInfo.tagline;
        }
        if (to.meta.titleKey) {
          let pageTitle = this.$tc(to.meta.titleKey, 2);
          appTitle = pageTitle + " | " + appTitle;
        }
        else if (to.meta.title)
          appTitle = to.meta.title + " | " + appTitle;

        document.title = appTitle;
      }
    }
  }
};
</script>

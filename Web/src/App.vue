<template>

  <v-app id="songtrackerpro" :class="`${!$vuetify.breakpoint.smAndDown ? 'full-sidebar' : 'mini-sidebar'}`">
      <router-view />
  </v-app>

</template>

<script>
import { mapState } from "vuex";

export default {
  name: 'App',
  components: {
    
  },
  computed: {
    ...mapState(["AppInfo"])
  },
  watch: {
    $route: {
      immediate: true,
      handler(to) {
        let appTitle = this.AppInfo.name + ' - ' + this.AppInfo.tagline;
        if (to.meta.title)
          appTitle = to.meta.title + " | " + appTitle;

        document.title = appTitle;
      }
    }
  }
};
</script>

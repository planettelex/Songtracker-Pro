<template>
  <v-app-bar
    app
    clipped-left
    clipped-right
    elevation="1">

    <!--- Logo -->
    <v-toolbar-title
      class="align-center d-flex logo-section"
      :class="`${showLogo ? 'logo-width' : ''}`">
      <div class="logo-icon">
        <img src="../../../assets/images/logo-header.svg" class="mt-2" /> 
        <h1 class="app-name">{{ appName }}</h1>
      </div>
    </v-toolbar-title>
    <!--- /Logo -->

    <!--- Toggle Sidebar -->
    <div @click="showhideLogo">
      <v-app-bar-nav-icon
        @click="$vuetify.breakpoint.smAndDown
            ? setSidebarDrawer(!SidebarDrawer)
            : $emit('input', !value)"/>
    </div>
    <!--- /Toggle Sidebar -->

    <v-spacer />
    <!--- Right Side -->

    <!--- User Menu -->
    <v-menu
      bottom
      left
      offset-y
      origin="top right"
      transition="scale-transition">
      <template v-slot:activator="{ on }">
        <v-btn dark icon v-on="on" class="mr-1">
          <v-avatar size="40">
            <img :src="ProfileImage" :alt="profileImageAlt" />
          </v-avatar>
        </v-btn>
      </template>
      <v-list class="v-user-menu">
        <v-list-item v-for="(item, i) in userMenu"
          :key="i"
          router :to="item.route"
          color="primary">
          <v-list-item-icon v-if="item.icon">
            <v-icon v-text="item.icon" />
          </v-list-item-icon>
          <v-list-item-content v-if="item.title">
            <v-list-item-title v-text="item.title"/>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-menu>
    <!--- /User Menu -->
  </v-app-bar>
</template>

<script>
import { mapState, mapMutations } from "vuex";

export default {
  name: "Header",

  components: {},

  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },

  data: () => ({
    appName: "",
    profileImageAlt: "",
    showLogo: true,
    userMenu: [
      { titleKey: "MyProfile", route: "/my-profile", icon: 'mdi-account' },
      { titleKey: "Logout", route: "/login?logout=true", icon: 'mdi-logout' },
    ]
  }),

  computed: {
    ...mapState(["SidebarDrawer", "ProfileImage", "AppInfo"]),
  },

  methods: {
    ...mapMutations({
      setSidebarDrawer: "SET_SIDEBAR_DRAWER",
    }),

    showhideLogo: function() {
      this.showLogo = !this.showLogo;
    },
  },

  async mounted() {
    this.appName = this.AppInfo.entityName ? this.AppInfo.entityName : this.AppInfo.name;
    this.userMenu.forEach(menuItem => {
      menuItem.title = this.$t(menuItem.titleKey);
    });
    this.profileImageAlt = this.$t('ProfileImage');
  }
};
</script>

<style lang="scss">
.app-name {
    color: #121c37;
  }
.v-application--is-ltr .v-user-menu .v-list-item__icon:first-child {
  margin-right: 10px;
}
.v-sheet.v-app-bar.v-toolbar:not(.v-sheet--outlined),
.v-sheet.v-card:not(.v-sheet--outlined) {
  box-shadow: $box-shadow;
}
.v-btn--icon.v-size--default .v-icon {
  width: 20px;
  font-size: 20px;
}
.v-application .mt-2 {
    margin-top: 0 !important;
}
.logo-icon h1 {
  float: left;
  font-size: 30px;
  margin: -3px 5px 0 15px;
}
.logo-icon img {
  float: left;
  width: 40px;
  height: 40px;
}
.hidelogo {
  display: none;
}
.descpart {
  max-width: 220px;
}
</style>

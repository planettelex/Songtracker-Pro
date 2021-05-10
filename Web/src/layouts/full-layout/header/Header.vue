<template>
  <v-app-bar
    app
    clipped-left
    clipped-right>

    <!--- Logo -->
    <v-toolbar-title
      class="align-center d-flex logo-section"
      :class="`${showLogo ? 'logo-width' : ''}`"
    >
      <div class="logo-icon">
        <img src="../../../assets/images/logo.svg" class="mt-2" /> 
        <h1>Songtracker Pro</h1>
      </div>
    </v-toolbar-title>
    <!--- /Logo -->

    <!--- Toggle Sidebar -->
    <div @click="showhideLogo">
      <v-app-bar-nav-icon
        @click="$vuetify.breakpoint.smAndDown
            ? setSidebarDrawer(!SidebarDrawer)
            : $emit('input', !value)"
      />
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
            <img
              :src="ProfileImage"
              alt="Profile Image"
            />
          </v-avatar>
        </v-btn>
      </template>
      <v-list>
        <v-list-item
          v-for="(item, i) in userprofile"
          :key="i"
          router :to="item.route"
          color="primary">
          <v-list-item-title>{{ item.title }}</v-list-item-title>
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
    showLogo: true,
    userprofile: [
      { title: "My Profile", route: "/my-profile" },
      { title: "Logout", route: "/login?logout=true"},
    ]
  }),

  computed: {
    ...mapState(["SidebarDrawer", "ProfileImage"]),
  },

  methods: {
    ...mapMutations({
      setSidebarDrawer: "SET_SIDEBAR_DRAWER",
    }),
    showhideLogo: function() {
      this.showLogo = !this.showLogo;
    },
  },
};
</script>

<style lang="scss">
.v-application .theme--dark.white .theme--dark.v-btn.v-btn--icon {
  color: $font-color !important;
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
  margin: -2px 5px 0 10px;
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

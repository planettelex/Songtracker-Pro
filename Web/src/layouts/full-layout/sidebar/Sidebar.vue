<template>
  <v-navigation-drawer
    v-model="Sidebar_drawer"
    :dark="SidebarColor !== 'white'"
    :color="SidebarColor"
    mobile-breakpoint="960"
    clipped
    :right="$vuetify.rtl"
    mini-variant-width="70"
    :expand-on-hover="expandOnHover"
    app
    id="main-sidebar"
    v-bar
  >
    <!---USer Area -->
    <v-list-item two-line class="profile-bg">
      <v-list-item-avatar>
        <img src="https://randomuser.me/api/portraits/men/81.jpg" />
      </v-list-item-avatar>

      <v-list-item-content >
        <v-list-item-title>Dohnathan Deo</v-list-item-title>
        <v-list-item-subtitle class="caption">Webdesigner</v-list-item-subtitle>
      </v-list-item-content>
    </v-list-item>
    <v-divider></v-divider>
    <!---USer Area -->

    <v-list expand nav class="mt-1">
      <template v-for="(item, i) in items">
        <!---If Sidebar Caption -->
        <v-row v-if="item.header" :key="item.header" align="center">
          <v-col cols="12">
            <v-subheader v-if="item.header" class="d-block text-truncate">{{ item.header }}</v-subheader>
          </v-col>
        </v-row>
        <!---If Sidebar Caption -->
        <BaseItemGroup v-else-if="item.children" class="icon-size" :key="`group-${i}`" :item="item"></BaseItemGroup>

        <BaseItem v-else :key="`item-${i}`" :item="item"/>
      </template>
      <!---Sidebar Items -->
    </v-list>
    <v-divider></v-divider>
    <!--- Progress -->
    <v-list-item two-line>
      <v-list-item-content class>
        <v-list-item-title class="d-flex mb-3 align-center">
          <span class="body-2 text-truncate">monthly profit</span>
          <div class="ml-auto">
            <h6 class="mb-0 info--text">80%</h6>
          </div>
        </v-list-item-title>
        <v-progress-linear rounded value="80"></v-progress-linear>
      </v-list-item-content>
    </v-list-item>
    <v-list-item two-line>
      <v-list-item-content class>
        <v-list-item-title class="d-flex mb-3 align-center">
          <span class="body-2 text-truncate">Sales of the year</span>
          <div class="ml-auto">
            <h6 class="mb-0 success--text">54%</h6>
          </div>
        </v-list-item-title>
        <v-progress-linear color="success" rounded value="54"></v-progress-linear>
      </v-list-item-content>
    </v-list-item>
    <!--- Progress -->
  </v-navigation-drawer>
</template>

<script>
import { mapState } from "vuex";
import SidebarItems from "./SidebarItems";
export default {
  name: "Sidebar",
  props: {
    expandOnHover: {
      type: Boolean,
      default: false
    }
  },
  data: () => ({
    items: SidebarItems
  }),
  computed: {
    ...mapState(["SidebarColor", "SidebarBg"]),
    Sidebar_drawer: {
      get() {
        return this.$store.state.Sidebar_drawer;
      },
      set(val) {
        this.$store.commit("SET_SIDEBAR_DRAWER", val);
      }
    }
  },
  watch: {
    "$vuetify.breakpoint.smAndDown"(val) {
      this.$emit("update:expandOnHover", !val);
    }
  },

  methods: {}
};
</script>
<style lang="scss">
#main-sidebar{
  box-shadow:1px 0 20px rgba(0,0,0,.08);
  -webkit-box-shadow:1px 0 20px rgba(0,0,0,.08);
  .v-navigation-drawer__border{
    display: none;
  }
  .v-list {
    padding: 8px 15px;
  }
  .v-list-item{
      min-height:35px;
      &__icon--text,
      &__icon:first-child{
        justify-content: center;
        text-align: center;
        width: 20px;
      }
      
  }
  .v-list-item__icon i{
        width:20px;
      }
  .icon-size .v-list-group__items i{
    width:16px;
    font-size:16px;
  }
  .profile-bg{
      &.theme--dark.v-list-item:not(.v-list-item--active):not(.v-list-item--disabled){
      opacity:1;
    }
    .v-avatar{
      padding:15px 0;
    }
  }
  .theme--dark.v-list-item:not(.v-list-item--active):not(.v-list-item--disabled){
    opacity:0.5;
    &:hover{
      opacity:1;
    }
  }
  

}
</style>
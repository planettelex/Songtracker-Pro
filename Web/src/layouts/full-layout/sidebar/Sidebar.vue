<template>
  <v-navigation-drawer
    v-model="SidebarDrawer"
    :dark="SidebarColor !== 'white'"
    :color="SidebarColor"
    mobile-breakpoint="960"
    clipped
    :right="$vuetify.rtl"
    mini-variant-width="70"
    :expand-on-hover="expandOnHover"
    app
    id="main-sidebar"
    v-bar>

    <v-list expand nav class="mt-1">
      <template v-for="(item, i) in items">

        <!--- Sidebar Caption -->
        <v-row v-if="item.header" :key="item.header" align="center">
          <v-col cols="12">
            <v-subheader v-if="item.header" class="d-block text-truncate">{{ item.header }}</v-subheader>
          </v-col>
        </v-row>
        <!--- /Sidebar Caption -->

        <BaseItemGroup v-else-if="item.children" class="icon-size" :key="`group-${i}`" :item="item"></BaseItemGroup>

        <BaseItem v-else :key="`item-${i}`" :item="item"/>

      </template>
    </v-list>
    <v-divider></v-divider>
  </v-navigation-drawer>
</template>

<script>

import { mapState } from "vuex";
import SidebarMenu from "./SidebarMenu";

export default {
  name: "Sidebar",

  props: {
    expandOnHover: {
      type: Boolean,
      default: false
    }
  },

  data: () => ({
    items: null
  }),

  computed: {
    ...mapState(["SidebarColor", "User"]),
    SidebarDrawer: {
      get() {
        return this.$store.state.SidebarDrawer;
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

  methods: { },

  mounted() {
    this.items = SidebarMenu(this.User.type);
  }
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
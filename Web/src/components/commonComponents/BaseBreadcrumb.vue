<template>
  <v-row class="page-breadcrumb">
    <v-col cols="12" sm="12" class="pt-0">
      <v-toolbar dense flat class="transparent mb-1" height="40">
        <v-toolbar-title>
          <span class="info--text">{{ title }}</span>
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-title>
          <v-breadcrumbs :items="breadcrumbs" class="pa-0">
            <template v-slot:divider v-if="breadcrumbs">
              <v-icon>mdi-chevron-right</v-icon>
            </template>
          </v-breadcrumbs>
        </v-toolbar-title>
      </v-toolbar>
    </v-col>
  </v-row>
</template>
<script>
export default {
  name: "BaseBreadcrumb",
  props: {
    title: String,
    breadcrumbs: Array,
    icon: String,
  },
  data: () => ({}),
};
</script>
<style lang="scss">
.page-breadcrumb {
  .v-toolbar__content {
    padding: 0px;
  }
}
</style>

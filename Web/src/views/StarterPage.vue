<template>
  <v-container fluid class="down-top-padding pt-0">
    <BaseBreadcrumb
      :title="page.title"
      :icon="page.icon"
      :breadcrumbs="breadcrumbs"
    ></BaseBreadcrumb>
    <v-row>
      <v-col cols="12" md="6">
        <v-card>
          <v-toolbar dense flat>
            <v-toolbar-title>Hello world !</v-toolbar-title>

            <v-spacer></v-spacer>

            <v-btn icon>
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </v-toolbar>
          <v-divider></v-divider>
          <v-card-text>
            <span>Lorem ipsum dolor sit amet, consectetur adipiscing elit</span>
            <br />
            <br />
            <br />
            <br />
            <br />
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card>
          <v-toolbar dense flat>
            <v-toolbar-title>Hello world !</v-toolbar-title>

            <v-spacer></v-spacer>

            <v-btn icon>
              <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
          </v-toolbar>
          <v-divider></v-divider>
          <v-card-text>
            <span>Lorem ipsum dolor sit amet, consectetur adipiscing elit</span>
            <br />
            <br />
            <br />
            <br />
            <br />
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "StarterPage",
  data: () => ({
    page: {
      title: "StarterPage",
    },
    elementVisible: false,
    breadcrumbs: [
      {
        text: "StarterPage",
        disabled: true,
      },
    ],
  }),
};
</script>

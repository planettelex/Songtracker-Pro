<template>
  <v-container fluid class="down-top-padding pt-0">
    <h2>System Information</h2>
    <br/>
    <p>{{ systemInformation.name }} v{{ systemInformation.version }}</p>
    <p>{{ systemInformation.tagline }}</p>
  </v-container>
</template>

<script>
import SystemInformation from '../../models/SystemInformation';
import { mapState } from "vuex";
import apiRequest from '../../apiRequest';

export default {
  name: "SystemInformation",
  
  data: () => ({
    page: {
      title: "System Information",
    },
    systemInformation: {}
  }),

  computed: {
    ...mapState(["Login", "User"])
  },

  async mounted() {
    apiRequest.headers.AuthenticationToken = this.Login.authenticationToken;
    this.systemInformation = await SystemInformation.config(apiRequest).first();
  }
};
</script>
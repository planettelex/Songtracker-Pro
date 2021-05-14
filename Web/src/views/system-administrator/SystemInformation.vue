<template>
  <v-container fluid class="down-top-padding pt-0">
    <h2>System Information</h2>
    <br/>
    <h3>{{ systemInformation.name }}</h3>
    <h4>{{ systemInformation.tagline }}</h4>
    <br/>
    <p>
      <strong>Domain:</strong>&nbsp;<label>{{ systemInformation.domain }}</label><br/>
      <strong>Version:</strong>&nbsp;<label>{{ systemInformation.version }}</label><br/>
      <strong>ID:</strong>&nbsp;<label>{{ systemInformation.uuid }}</label><br/>
      <strong>Culture:</strong>&nbsp;<label>{{ systemInformation.culture }}</label><br/>
      <strong>Currency:</strong>&nbsp;<label>{{ systemInformation.currency }}</label><br/>
      <strong>Database Server:</strong>&nbsp;<label>{{ systemInformation.databaseServer }}</label><br/>
      <strong>Database Name:</strong>&nbsp;<label>{{ systemInformation.databaseName }}</label><br/>
      <strong>API Domain:</strong>&nbsp;<label>{{ systemInformation.apiDomain }}</label><br/>
      <strong>Email Server:</strong>&nbsp;<label>{{ systemInformation.emailServer }}</label><br/>
      <strong>Email Account:</strong>&nbsp;<label>{{ systemInformation.emailAccount }}</label><br/>
      <strong>oAuth Client ID:</strong>&nbsp;<label>{{ systemInformation.oAuthId }}</label><br/>
      <strong>oAuth Console:</strong>&nbsp;<label>{{ systemInformation.oAuthConsole }}</label><br/>
      <strong>Database Console:</strong>&nbsp;<label>{{ systemInformation.databaseConsole }}</label><br/>
      <strong>API Hosting Console:</strong>&nbsp;<label>{{ systemInformation.apiHostingConsole }}</label><br/>
      <strong>Web Hosting Console:</strong>&nbsp;<label>{{ systemInformation.hostingConsole }}</label><br/>
      <strong>Email Console:</strong>&nbsp;<label>{{ systemInformation.emailConsole }}</label><br/>
    </p>
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
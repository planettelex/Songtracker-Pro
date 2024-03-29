<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row>
      <v-col cols="12"><h2>{{ $t('SystemInformation') }}</h2></v-col>
    </v-row>
    <v-row>
      <v-col cols="12"><br/></v-col>
    </v-row>
    <v-row>
      <v-col class="mb-6" cols="12" sm="12" md="6" lg="6" xl="4">
        <info-box :title="$t('Installation')" v-bind:info="installationInfo" :linkName="$t('Console')" 
        :linkUrl="systemInformation.hostingConsole" headerBackgroundColor="#e3b1a2" />
      </v-col>
      <v-col class="mb-6" cols="12" sm="12" md="6" lg="6" xl="4">
        <info-box :title="$t('API')" v-bind:info="apiInfo" :linkName="$t('Console')"
        :linkUrl="systemInformation.apiHostingConsole" headerBackgroundColor="#e9d5a4" />
      </v-col>
      <v-col class="mb-6" cols="12" sm="12" md="6" lg="6" xl="4">
        <info-box title="OAuth" v-bind:info="oauthInfo" :linkName="$t('Console')"
        :linkUrl="systemInformation.oAuthConsole" headerBackgroundColor="#99d0b8" />
      </v-col>
      <v-col class="mb-6" cols="12" sm="12" md="6" lg="6" xl="4">
        <info-box :title="$t('Database')" v-bind:info="databaseInfo" :linkName="$t('Console')" 
        :linkUrl="systemInformation.databaseConsole" headerBackgroundColor="#85c5d0" />
      </v-col>
      <v-col class="mb-6" cols="12" sm="12" md="6" lg="6" xl="4">
        <info-box :title="$t('Email')" v-bind:info="emailInfo" :linkName="$t('Console')" 
        :linkUrl="systemInformation.emailConsole" headerBackgroundColor="#dabbf6" />
      </v-col>
    </v-row>
    
  </v-container>
</template>

<script>
import ErrorHandler from '../../models/local/ErrorHandler';
import ApiRequestHeaders from '../../models/local/ApiRequestHeaders';
import SystemInformationModel from '../../models/api/SystemInformation';
import NameValuePair from '../../models/local/NameValuePair';
import InfoBox from '../../components/InfoBox.vue';
import { mapState } from "vuex";

export default {
  components: { InfoBox },

  name: "SystemInformation",
  
  data: () => ({
    systemInformation: {},
    installationInfo: [],
    apiInfo: [],
    oauthInfo: [],
    databaseInfo: [],
    emailInfo: []
  }),

  computed: {
    ...mapState(["Authentication"]),
    RequestHeaders: {
      get () { return new ApiRequestHeaders(this.Authentication.authenticationToken); }
    },
  },
  
  methods: {
    handleError(error) {
      this.error = new ErrorHandler(error).handleError(this.$router);
    }
  },

  async mounted() {
    this.systemInformation = await SystemInformationModel.config(this.RequestHeaders).first()
      .catch(error => this.handleError(error));
    
    this.installationInfo = new Array(3);
    this.installationInfo[0] = new NameValuePair(this.$t("Domain"), this.systemInformation.domain);
    this.installationInfo[1] = new NameValuePair(this.$t("Id"), this.systemInformation.uuid);
    this.installationInfo[2] = new NameValuePair(this.$t("Version"), this.systemInformation.version);
    
    this.apiInfo = new Array(3);
    this.apiInfo[0] = new NameValuePair(this.$t("Domain"), this.systemInformation.apiDomain);
    this.apiInfo[1] = new NameValuePair(this.$t("Culture"), this.systemInformation.culture);
    this.apiInfo[2] = new NameValuePair(this.$t("Currency"), this.systemInformation.currency);

    this.oauthInfo = new Array(1);
    this.oauthInfo[0] = new NameValuePair(this.$t("ClientId"), this.systemInformation.oAuthId);

    this.databaseInfo = new Array(2);
    this.databaseInfo[0] = new NameValuePair(this.$t("Server"), this.systemInformation.databaseServer);
    this.databaseInfo[1] = new NameValuePair(this.$t("Name"), this.systemInformation.databaseName);

    this.emailInfo = new Array(2);
    this.emailInfo[0] = new NameValuePair(this.$t("Server"), this.systemInformation.emailServer);
    this.emailInfo[1] = new NameValuePair(this.$tc("Account"), this.systemInformation.emailAccount);
  }
};
</script>

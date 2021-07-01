<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row v-if="error" justify="center">
      <v-col cols="12" sm="10" md="7" lg="6" xl="4">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12" sm="10" md="7" lg="6" xl="4">
        
        <v-card class="login-card elevation-4">
          <v-container>
            <v-row>
              <v-col class="d-flex justify-center" cols="1" offset="2">
                  <img class="login-logo-icon" src="../assets/images/logo-light.svg"/> 
              </v-col>
              <v-col cols="9">
                <div class="login-box">
                  <h2 class="login-app-name">{{ this.applicationInfo.name }}</h2>
                  <span style="display:none;">v {{ this.applicationInfo.version }}</span>
                  <em class="login-tagline">{{ this.applicationInfo.tagline }}</em>
                  <div class="login-button">
                    <button class="v-button" @click="login" v-if="!userAuthenticated" :disabled="!authInitialized">{{ $t("Login") }} &gt;&gt;</button> 
                    <button class="v-cancel-button" @click="logout(false)" v-if="userAuthenticated" :disabled="!authInitialized">{{ $t("Logout") }}</button>
                  </div>
                </div>
              </v-col>
            </v-row>
          </v-container>
        </v-card>
        
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ApiRequestHeaders from '../models/local/ApiRequestHeaders';
import ApplicationModel from '../models/api/Application';
import LoginModel from '../models/api/Login';
import LogoutModel from '../models/api/Logout';
import UserType from '../enums/UserType';
import { mapState } from "vuex";

export default {
  name: "Login",

  data: () => ({
    authInitialized: false,
    userAuthenticated: false,
    applicationInfo: {
      name: null,
      tagline: null,
      version: null
    },
    error: null
  }),

  computed: {
    ...mapState(["Application"]),
    Application: {
      get() { return this.$store.state.Application; },
      set(val) { this.$store.commit("SET_APPLICATION", val); }
    },
    ...mapState(["Authentication"]),
    Authentication: {
      get() { return this.$store.state.Authentication; },
      set(val) { this.$store.commit("SET_AUTHENTICATION", val); }
    },
    ...mapState(["ProfileImage"]),
    ProfileImage: {
      get() { return this.$store.state.ProfileImage; },
      set(val) { this.$store.commit("SET_PROFILE_IMAGE", val); }
    },
    ...mapState(["User"]),
    User: {
      get() { return this.$store.state.User; },
      set(val) { this.$store.commit("SET_USER", val); }
    },
    ...mapState(["LastPageViewed"]),
    LastPageViewed: {
      get() { return this.$store.state.LastPageViewed; },
      set(val) { this.$store.commit("SET_LAST_PAGE_VIEWED", val); }
    },
    RequestHeaders: {
      get () { return new ApiRequestHeaders(this.Authentication.authenticationToken); }
    },
    UnauthenticatedRequestHeaders: {
      get () { return new ApiRequestHeaders(); }
    }
  },

  methods: {    
    async login() {
      try {
        const googleUser = await this.$gAuth.signIn();
        if (this.$gAuth.isAuthorized) {
          let profile = googleUser.getBasicProfile();
          let authResponse = googleUser.getAuthResponse();
          let authentication = {
            authenticationId: profile.getEmail(),
            authenticationToken: authResponse.access_token,
            tokenExpiration: new Date(authResponse.expires_at).toISOString()
          }
          this.Authentication = authentication;
          this.ProfileImage = profile.getImageUrl();
          const loginModel = new LoginModel(authentication);
          loginModel.config(this.UnauthenticatedRequestHeaders).save()
            .then(response => { 
              this.User = response.user;
              if (this.LastPageViewed)
                this.$router.push(this.LastPageViewed);
              else {
                switch (response.user.type) {
                  case UserType.SystemAdministrator:
                    this.$router.push("/system-information");
                    break;
                  case UserType.PublisherAdministrator:
                    this.Application.entityName = this.User.publisher.name;
                    this.$router.push("/publisher-earnings");
                    break;
                  case UserType.LabelAdministrator:
                    this.Application.entityName = this.User.recordLabel.name;
                    this.$router.push("/label-earnings");
                    break;
                  case UserType.SystemUser:
                    this.$router.push("/my-earnings");
                    break;
                }
              }
            })
            .catch(error => this.handleError(error));
        }
      } 
      catch (error) {
        this.handleError(error);
      }
    },

    async logout(redirect, reason) {
      try {
        let isLoggedOut = this.Authentication === null;
        if (isLoggedOut) {
          this.userAuthenticated = false;
          return;
        }
        await this.$gAuth.signOut();
        const logoutModel = new LogoutModel(null);
        let that = this;
        logoutModel.config(this.RequestHeaders).save()
        .then(() => {
          this.userAuthenticated = false;
          that.resetState();

          if (reason == "expired")
            this.error = this.$t('SessionExpired');
          else
            this.LastPageViewed = null;

          if (redirect)
            that.$router.push("/");
        })
        .catch(error => { 
          this.userAuthenticated = false;
          that.resetState();
          that.handleError(error);
        });
      } 
      catch (error) {
        this.userAuthenticated = false;
        this.resetState();
        this.handleError(error);
      }
    },

    resetState() {
      this.Authentication = null;
      this.ProfileImage = null;
      this.User = null;
    },

    handleError(error) {
      this.error = error;
    },
  },

  async mounted() {
    try {
      if (this.Application == null || this.Application.name == null) {
        this.Application = await ApplicationModel.first().catch(error => this.handleError(error));
      }
      Object.assign(this.applicationInfo, this.Application);
      let that = this;
      let authLoaded = setInterval(function() {
        that.authInitialized = that.$gAuth.isInit;
        if (that.authInitialized) {
          clearInterval(authLoaded);
          let isLoggedIn = that.Authentication !== null && Date.parse(that.Authentication.tokenExpiration) > Date.now();
          if (that.$route.query.logout)
            that.logout(true, that.$route.query.reason);
          else
            that.userAuthenticated = isLoggedIn && that.$gAuth.isAuthorized;
        }
      }, 500);
    } 
    catch (error) {
        this.handleError(error);
    }
  }
};
</script>

<style lang="scss">
  .login-app-name {
    color: $light-primary;
  }
  .login-tagline {
    color: $gray-250;
  }
  .theme--light.v-application {
    background-image: url('../assets/images/concert.jpg');
    background-repeat: no-repeat;
    background-size: cover;
  }
  #login .theme--light.v-card {
    background-color: transparent;
    background-image: url('../assets/images/dark-transparent.png');
    background-repeat: repeat;
    border-top-left-radius: $border-radius-root;
    border-top-right-radius: $border-radius-root;
  }
  .login-card {
    padding-top: 45px;
    margin-top: -30vh;
  }
  .login-logo-icon {
    height: 70%;
    margin-left: -20px;
  }
  .login-box {
    padding: 0 0 0 40px;
  }
  .login-button {
    margin-top: 10px;
  }
</style>

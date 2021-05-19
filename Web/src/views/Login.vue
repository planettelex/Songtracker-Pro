<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row v-if="error" justify="center">
      <v-col sm="10" md="7" lg="6" xl="4">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col sm="10" md="7" lg="6" xl="4">
        <v-card class="login-card elevation-4">
          <v-row>
            <v-col class="d-flex justify-center" cols="1" offset="2">
                <img class="login-logo-icon" src="../assets/images/logo.svg"/> 
            </v-col>
            <v-col cols="8">
              <div class="login-box">
                <h2 class="app-name">{{ this.AppInfo.name }}</h2>
                <span style="display:none;">v {{ this.AppInfo.version }}</span>
                <em>{{ this.AppInfo.tagline }}</em>
                <div class="login-button">
                  <button class="v-button" @click="login" v-if="!userAuthenticated" :disabled="!authInitialized">{{ $t("Login") }}</button>
                  <button class="v-cancel-button" @click="logout(false)" v-if="userAuthenticated" :disabled="!authInitialized">{{ $t("Logout") }}</button>
                </div>
              </div>
            </v-col>
          </v-row>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import apiRequest from '../apiRequest';
import AppInfo from '../models/AppInfo';
import Login from '../models/Login';
import Logout from '../models/Logout';
import UserType from '../enums/UserType';
import { mapState } from "vuex";

export default {
  name: "Login",

  data: () => ({
    authInitialized: false,
    userAuthenticated: false,
    error: null
  }),

  computed: {
    ...mapState(["ProfileImage"]),
    ProfileImage: {
      get() { return this.$store.state.ProfileImage; },
      set(val) { this.$store.commit("SET_PROFILE_IMAGE", val); }
    },
    ...mapState(["Login"]),
    Login: {
      get() { return this.$store.state.Login; },
      set(val) { this.$store.commit("SET_LOGIN", val); }
    },
    ...mapState(["User"]),
    User: { 
      get() { return this.$store.state.User; },
      set(val) { this.$store.commit("SET_USER", val); }
    },
    ...mapState(["AppInfo"]),
    AppInfo: {
      get() { return this.$store.state.AppInfo; },
      set(val) { this.$store.commit("SET_APP_INFO", val); }
    }
  },

  methods: {
    handleError(error) {
      this.error = error;
    },
    
    async login() {
      try {
        const googleUser = await this.$gAuth.signIn();
        if (this.$gAuth.isAuthorized) {
          this.isAuthorized = true;
          let profile = googleUser.getBasicProfile();
          let authResponse = googleUser.getAuthResponse();
          let login = {
            authenticationId: profile.getEmail(),
            authenticationToken: authResponse.access_token,
            tokenExpiration: new Date(authResponse.expires_at).toISOString()
          }
          this.ProfileImage = profile.getImageUrl();
          this.Login = login;
          const loginModel = new Login(login);
          loginModel.save()
          .then(response => { 
            let user = response.user;
            this.User = user;
            switch (user.type) {
              case UserType.SystemAdministrator:
                this.$router.push("/system-information");
                break;
              case UserType.PublisherAdministrator:
                this.AppInfo.entityName = this.User.publisher.name;
                this.$router.push("/publisher-earnings");
                break;
              case UserType.LabelAdministrator:
                this.AppInfo.entityName = this.User.recordLabel.name;
                this.$router.push("/label-earnings");
                break;
              case UserType.SystemUser:
                this.$router.push("/my-earnings");
                break;
            }
          })
          .catch(error => this.handleError(error));          
        }
      } 
      catch (error) {
        this.handleError(error);
      }
    },

    async logout(redirect) {
      try {
        let isLoggedOut = this.Login === null;
        if (isLoggedOut) {
          this.userAuthenticated = false;
          return;
        }
        await this.$gAuth.signOut();
        const logoutModel = new Logout();
        apiRequest.headers.AuthenticationToken = this.Login.authenticationToken;
        let that = this;
        logoutModel.config(apiRequest).save()
        .then(() => {
          that.AppInfo.entityName = null;
          that.Login = null;
          that.ProfileImage = null;
          that.User = null;
          that.userAuthenticated = false;
          if (redirect)
            that.$router.push("/");
        })
        .catch(error => { 
          that.AppInfo.entityName = null;
          that.Login = null;
          that.ProfileImage = null;
          that.User = null;
          that.userAuthenticated = false;
          that.handleError(error);
        });
      } 
      catch (error) {
        this.handleError(error);
      }
    }
  },

  async mounted() {
    try {
      if (!this.AppInfo) {
        this.AppInfo = await AppInfo.first();
      }
      document.title = this.AppInfo.name + ' - ' + this.AppInfo.tagline;
      let that = this;
      let authLoaded = setInterval(function() {
        that.authInitialized = that.$gAuth.isInit;
        if (that.authInitialized) {
          clearInterval(authLoaded);
          let isLoggedIn = that.Login !== null;
          if (that.$route.query.logout && isLoggedIn) {
            that.logout(true);
          }
          else {
            that.userAuthenticated = isLoggedIn;
          }
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
  .app-name {
    color: $default;
  }
  .theme--light.v-application {
    background-image: url('../assets/images/concert.jpg');
    background-repeat: no-repeat;
    background-size: cover;
  }
  .theme--light.v-card {
    background-color: transparent;
    background-image: url('../assets/images/white-transparent.png');
    background-repeat: repeat;
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

<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row justify="center">
      <v-col lg="6" sm="4" xl="8">
        <v-card class="login-card elevation-4">
          <v-row>
            <v-col lg="3">
              <div class="pa-8 pa-sm-8 login-logo-icon">
                <img src="../assets/images/logo.svg"/> 
              </div>
            </v-col>
            <v-col lg="9">
              <div class="pa-sm-4">
                <h2>{{ this.appInfo.name }}</h2>
                <span style="display:none;">v {{ this.appInfo.version }}</span>
                <em>{{ this.appInfo.tagline }}</em>
                <div class="login-button">
                  <button @click="login" v-if="!userAuthenticated" :disabled="!authInitialized">Login</button>
                  <button @click="logout(false)" v-if="userAuthenticated" :disabled="!authInitialized">Logout</button>
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
    appInfo: {},
    authInitialized: false,
    userAuthenticated: false
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
    }
  },

  methods: {
    handleError(error) {
      console.error(error);
      //TODO: Error UI Feedback.
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
                this.$router.push("/publisher-earnings");
                break;
              case UserType.LabelAdministrator:
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
          that.Login = null;
          that.User = null;
          that.userAuthenticated = false;
          if (redirect)
            that.$router.push("/");
        })
        .catch(error => { 
          that.Login = null;
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
      this.appInfo = await AppInfo.first();
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
  .theme--light.v-application {
    background-image: url('../assets/images/concert.jpg');
    background-repeat: no-repeat;
    background-size: cover;
  }
  .login-card {
    padding-top: 20px;
    margin-top: -30vh;
  }
  .login-button {
    margin-top: 10px;
  }
  .login-button button {
    display: inline-block;
    padding: 0.7em 1.7em;
    margin: 0 0.3em 0.3em 0;
    border-radius: 0.2em;
    box-sizing: border-box;
    text-decoration: none;
    color: $light-primary;
    background-color: $primary;
    box-shadow: inset 0 -0.6em 1em -0.35em rgba(0,0,0,0.17),inset 0 0.6em 2em -0.3em rgba(255,255,255,0.15),inset 0 0 0em 0.05em rgba(255,255,255,0.12);
    text-align: center;
  }
  .login-button button:active {
    box-shadow: inset 0 0.6em 2em -0.3em rgba(0,0,0,0.15),inset 0 0 0em 0.05em rgba(255,255,255,0.12);
  }
  .login-logo-icon img {
    width: 90px;
    height: 90px;
  }
</style>

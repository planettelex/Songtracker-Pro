<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row justify="center">
      <v-col lg="6" sm="4" xl="8">
        <v-card class="elevation-4">
          <v-row>

            <v-col lg="5">
              <div class="pa-7 pa-sm-12">
                <h3>Login</h3>
                <button @click="login" v-if="!userAuthenticated" :disabled="!authInitialized">Login</button>
                <button @click="logout(false)" v-if="userAuthenticated" :disabled="!authInitialized">Logout</button>
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
import Login from '../models/Login';
import Logout from '../models/Logout';
import UserType from '../enums/UserType';
import { mapState } from "vuex";

export default {
  name: "Login",

  data: () => ({
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

  mounted() {
    try {
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

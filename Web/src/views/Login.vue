<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row justify="center">
      <v-col lg="6" sm="4" xl="8">
        <v-card class="elevation-4">
          <v-row>

            <v-col lg="5">
              <div class="pa-7 pa-sm-12">
                <h3>Login</h3>
                <button @click="login" v-if="!UserAuthenticated" :disabled="!authInitialized">Login</button>
                <button @click="logout(false)" v-if="UserAuthenticated" :disabled="!authInitialized">Logout</button>
              </div>
            </v-col>
            
          </v-row>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "Login",

  data: () => ({
    authInitialized: false
  }),

  computed: {
    ...mapState(["UserAuthenticated"]),
    UserAuthenticated: {
      get() {
        return this.$store.state.UserAuthenticated;
      },
      set(val) {
        this.$store.commit("SET_USER_AUTHENTICATED", val);
      }
    },
    ...mapState(["User"]),
    User: {
      get() {
        return this.$store.state.User;
      },
      set(val) {
        this.$store.commit("SET_USER", val);
      }
    }
  },

  methods: {
    async login() {
      try {
        const googleUser = await this.$gAuth.signIn();
        console.log('user', googleUser);
        let profile = googleUser.getBasicProfile();
        let authResponse = googleUser.getAuthResponse();
        
        let authenticationToken = authResponse.access_token;
        let authenticationId = profile.getEmail();
        console.log(authenticationToken);
        console.log(authenticationId);
        this.UserAuthenticated = this.$gAuth.isAuthorized;
        if (this.UserAuthenticated) {
          // TODO: Make API call, determine user type, and then route user accordingly.
          // For now we are hard-coding the user as a system administrator.
          this.$router.push("/system-information");
        }
      } catch (error) {
        // TODO: On fail do something.
        console.error(error);
        return null;
      }
    },
    async logout(redirect) {
      try {
        await this.$gAuth.signOut();
        this.UserAuthenticated = false;
        this.User = null;
        if (redirect)
          this.$router.push("/");
      } catch (error) {
        // TODO: On fail do something.
        console.error(error);
        return null;
      }
    }
  },
  mounted() {
    let that = this;
    let authLoaded = setInterval(function() {
      that.authInitialized = that.$gAuth.isInit;
      that.UserAuthenticated = that.$gAuth.isAuthorized;
      if (that.authInitialized) {
        clearInterval(authLoaded);
        if (that.$route.query.logout) {
          that.logout(true);
        }
      }
    }, 500);
  }
};
</script>

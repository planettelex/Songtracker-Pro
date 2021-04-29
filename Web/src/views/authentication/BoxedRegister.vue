<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row justify="center">
      <v-col lg="11" sm="8" xl="7">
        <v-card class="elevation-4">
          <v-row>
            <v-col lg="5" class="pr-0">
              <div class="pa-7 pa-sm-12">
                <img src="@/assets/images/logo-icon.png" />
                <h2 class="font-weight-bold mt-4 blue-grey--text text--darken-2">Sign Up</h2>
                <h6 class="subtitle-1">
                  Don't have an account?
                  <a href="#/pages/boxedlogin/" class>Sign in</a>
                </h6>

                <v-form ref="form" v-model="valid" lazy-validation action="/pages/boxedlogin">
                  <v-text-field
                    v-model="fname"
                    :rules="fnameRules"
                    label="Full Name"
                    class="mt-4"
                    required
                    outlined
                  ></v-text-field>
                  <v-text-field
                    v-model="email"
                    :rules="emailRules"
                    label="E-mail"
                    required
                    outlined
                  ></v-text-field>
                  <v-text-field
                    v-model="password"
                    :counter="10"
                    :rules="passwordRules"
                    label="Password"
                    required
                    outlined
                    :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                    :type="show1 ? 'text' : 'password'"
                  ></v-text-field>

                  <div class="d-block d-sm-flex align-center mb-4 mb-sm-0">
                    <v-checkbox
                      v-model="checkbox"
                      :rules="[v => !!v || 'You must agree to continue!']"
                      label="I agree to the terms and privacy"
                      required
                    ></v-checkbox>
                  </div>
                  <v-btn
                    :disabled="!valid"
                    color="info"
                    block
                    class="mr-4"
                    submit
                    @click="submit"
                  >Sign In</v-btn>
                </v-form>

                <div class="text-center mt-6">
                  <v-chip pill class="mr-2">
                    <v-avatar left>
                      <v-btn color="blue lighten-1" class="white--text">
                        <v-icon>mdi-twitter</v-icon>
                      </v-btn>
                    </v-avatar>Twitter
                  </v-chip>
                  <v-chip pill>
                    <v-avatar left>
                      <v-btn color="teal lighten-2" class="white--text">
                        <v-icon>mdi-github</v-icon>
                      </v-btn>
                    </v-avatar>Github
                  </v-chip>
                </div>
              </div>
            </v-col>
            <v-col lg="7" class="info d-none d-md-flex align-center justify-center">
              <div class="d-none d-sm-block">
                <div class="d-flex align-center pa-10">
                  <div>
                    <h2
                      class="display-1 white--text font-weight-medium"
                    >Elegant Design with unlimited features, built with love</h2>
                    <h6
                      class="subtitle-1 mt-4 white--text op-5 font-weight-regular"
                    >Wrappixel helps developers to build organized and well-coded admin dashboards full of beautiful and feature rich modules.</h6>
                    <v-btn class="mt-4 text-capitalize" x-large outlined color="white">Learn More</v-btn>
                  </div>
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
export default {
  name: "BoxedLogin",
  data: () => ({
    password: "",
    show1: false,
    passwordRules: [
      v => !!v || "Password is required",
      v => (v && v.length <= 10) || "Password must be less than 10 characters"
    ],
    email: "",
    emailRules: [
      v => !!v || "E-mail is required",
      v => /.+@.+\..+/.test(v) || "E-mail must be valid"
    ],
    checkbox: false,
    fname: "",
    fnameRules: [
      v => !!v || "Name is required",
      v => (v && v.length <= 10) || "Name must be less than 10 characters"
    ]
  }),
  methods: {
    submit() {
      this.$refs.form.validate();
      if (this.$refs.form.validate(true)) {
        this.$router.push({ path: "/pages/boxedlogin" });
      }
    }
  }
};
</script>

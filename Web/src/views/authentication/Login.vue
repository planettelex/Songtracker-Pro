<template>
  <v-container id="login" class="fill-height justify-center" tag="section">
    <v-row justify="center">
      <v-col lg="6" sm="4" xl="8">
        <v-card class="elevation-4">
          <v-row>

            <v-col lg="5">
              <div class="pa-7 pa-sm-12">
                <h3>Login</h3>
                <div class="g-signin2" data-onsuccess="onSuccess"></div>
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
  name: "Login",

  data: () => ({
    valid: true
  }),
  computed: {},
  methods: {
    onSuccess() {
      
    }
  }
};
</script>

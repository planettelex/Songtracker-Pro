<template>
  <div class="error-box blue-grey lighten-5">
    <div class="py-12">
      <div class="text-center">
        <div class="logo-icon">
          <img src="../assets/images/logo.svg" /> 
          <h1 class="app-name">{{ appName }}</h1>
        </div>
        <h2 class="error-title error--text">404</h2>
        <br />
        <h3 class="text-uppercase error-subtitle">{{ $t('PageNotFound') }}</h3>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";
import AppInfo from '../models/AppInfo';

export default {
  name: "Error",

  data: () => ({
    appName: ""
  }),

  computed: {
    ...mapState(["AppInfo"]),
  },

  async mounted() {
    if (this.AppInfo)
      this.appName = this.AppInfo.name;
    else {
      const appInfo = await AppInfo.first();
      this.appName = appInfo.name;
    }
  }
};
</script>

<style lang="scss">
.error-box {
  height: 100%;
  width: 100%;
  position: fixed;
}
.error-title {
  clear: both;
  font-size: 180px;
  font-weight: 800;
  text-shadow: 4px 4px 0 #fff, 6px 6px 0 #343a40;
  line-height: 180px;
}
.logo-icon {
  width: 50%;
  margin: 0 auto;
  img { float: left; width: 16%; }
  .app-name { font-size: 60px; padding-top: 10px; }
}

@media (max-width: 991px) {
  .error-title {
    font-size: 120px;
    line-height: 120px;
  }
}

@media (max-width: 767px) {
  .error-title {
    font-size: 40px;
    line-height: 40px;
  }
}
</style>

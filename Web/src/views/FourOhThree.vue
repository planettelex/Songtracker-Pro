<template>
<v-container fluid class="down-top-padding pt-0">
  <div class="error-box">
    <div class="py-12">
      <div class="text-center">
        <h2 class="error-title error--text">403</h2>
        <br />
        <h3 class="text-uppercase error-subtitle">{{ $t('Forbidden') }}</h3>
      </div>
    </div>
  </div>
  </v-container>
  
</template>

<script>
import { mapState } from "vuex";
import ApplicationData from '../models/api/Application';

export default {
  name: "FourOhThree",

  data: () => ({
    appName: ""
  }),

  computed: {
    ...mapState(["Application"]),
  },

  async mounted() {
    if (this.Application)
      this.appName = this.Application.name;
    else {
      const appInfo = await ApplicationData.first().catch(error => console.error(error));
      this.appName = appInfo.name;
    }
  }
};
</script>

<style lang="scss">
.error-box {
  height: 100%;
  width: 100%;
}
.error-title {
  clear: both;
  font-size: 180px;
  font-weight: 800;
  text-shadow: 4px 4px 0 #fff, 6px 6px 0 #343a40;
  line-height: 180px;
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

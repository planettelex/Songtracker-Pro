<template>
  <v-app id="inspire">
    <Header v-model="expandOnHover"></Header>
    
    <v-main>
      <v-container fluid class="page-wrapper">
        <router-view />
      </v-container>
    </v-main>
    
    <Sidebar :expand-on-hover.sync="expandOnHover"></Sidebar>
     
    <Footer></Footer> 
    
  </v-app>

</template>

<script>
import Header from './header/Header';
import Sidebar from './sidebar/Sidebar';
import Footer from './footer/Footer';
export default {
  name: 'Layout',

  components: {
    Header,
    Sidebar,
    Footer
  },

  props: {
    source: String,
  },
  data: () => ({
    expandOnHover: false
  })
};
</script>

<style>

</style>
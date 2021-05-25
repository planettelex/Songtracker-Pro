<template>
  <v-container fluid class="down-top-padding pt-0">
    <h2>{{ $tc('User', 2) }}</h2>
  </v-container>
</template>

<script>
export default {
  name: "Users",
  data: () => ({
    page: {
      title: "Users",
    },
  }),
};
</script>
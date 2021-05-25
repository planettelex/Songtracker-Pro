<template>
    <v-container class="fill-height pt-0" fluid>
        <v-row class="fill-height">
            <v-col class="info-box elevation-2" cols="12">
                <div class="info-box-header" :style="headerStyle">
                    <h5>{{ title }}</h5>
                    <a v-if="linkName != null" :href="linkUrl" target="_blank">{{ linkName }}</a>
                </div>
                <table>
                    <tbody>
                        <tr v-for="item in info" :key="item.name">
                            <td>{{ item.name }}</td>
                            <td>{{ item.value }}</td>
                        </tr>
                    </tbody>
                </table>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        name: 'InfoBox',

        props: {
            title: String,
            linkName: String,
            linkUrl: String,
            info: Array,
            headerBackgroundColor: String
        },

        data: () => ({
            headerStyle: { backgroundColor: "#a1aab2" }
        }),

        mounted() {
            if (this.headerBackgroundColor)
                this.headerStyle.backgroundColor = this.headerBackgroundColor;
        }
    }
</script>

<style lang="scss">
  .info-box {
    border-color: $subtitle-color;
    border-style: solid;
    border-width: 1px;
    border-radius: $border-radius-root;
    padding: 0;
    table {
        clear: both;
        border-spacing: 0;
        width: 100%;
        margin-bottom: 50px;
        table-layout: fixed;
        td {
            border-color: $gray-500;
            border-style: solid;
            border-left-width: 0;
            border-right-width: 0;
            border-top-width: 0;
            border-bottom-width: 1px;
            padding: 4px 12px;
            word-wrap: break-word;
            vertical-align: top;
        }
        td:first-child {
            border-right-width: 1px;
            width: 100px;
        }
    }
  }
  .info-box-header {
    padding: 4px 12px;
    height: 2.2rem;
    border-color: $subtitle-color;
    border-bottom-style: solid;
    border-bottom-width: 1px;
    border-top-left-radius: $border-radius-root !important;
    border-top-right-radius: $border-radius-root !important;
  }
  .info-box-header h5 {
      float: left;
      color: $default;
  }
  .info-box-header a {
      float: right;
      color: $default;
      font-size: 13px;
      text-decoration: underline;
      margin-top: 1px;
  }
</style>
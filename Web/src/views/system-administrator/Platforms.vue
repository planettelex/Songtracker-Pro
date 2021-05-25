<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="platforms" sort-by="name" must-sort>

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('Platform', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="675px">
            <template v-slot:activator="{ attrs }">
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="editPlatform(null)">{{ $t('New') }} {{ $tc('Platform', 1) }}</v-btn>
            </template>
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ formTitle }}</span>
              </v-card-title>
              <v-divider />
              <v-card-text>
                <v-container class="app-form">
                  <v-row>
                    <v-col cols="4" class="pl-0">
                      <v-text-field :label="$tc('Platform', 1)" v-model="editedPlatform.name"></v-text-field>
                    </v-col>
                    <v-col cols="8" class="pr-0">
                      <v-text-field :label="$t('Website')" v-model="editedPlatform.website"></v-text-field>
                    </v-col>
                    <v-col class="pa-0">
                      <h6>{{ $tc('Service', 2) }}</h6>
                      <span v-for="(service, index) in services" :key="service.id">
                        <v-checkbox dense hide-details class="checkbox mr-3" v-model="selectedServices[index]" 
                        :label="service.name" :value="selectedServices[index]" />
                      </span>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="close">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="save">{{ $t('Save') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small @click="editPlatform(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ApiRequest from '../../models/local/ApiRequest';
import PlatformData from '../../models/api/Platform';
import ServiceData from '../../models/api/Service';
import { mapState } from "vuex";

export default {
  name: "Platforms",

  data: () => ({
    dialog: false,
    platforms: [],
    services: [],
    selectedServices: [ false, false, false, false, false, false, false, false, false, false,
                        false, false, false, false, false, false, false, false, false, false],
    editedIndex: -1,
    editedPlatform: {
      id: -1,
      name: '',
      website: '',
      services: []
    },
    defaultPlatform: {
      name: '',
      website: '',
      services: []
    },
    error: null
  }),

    computed: {
    ...mapState(["Login"]),
    formTitle () {
      let verb = this.editedIndex === -1 ? this.$t('New') : this.$t('Edit');
      return verb + ' ' + this.$tc('Platform', 1);
    },
    headers () {
      return [
        { text: this.$tc('Platform', 1), value: "name", width: "23%" },
        { text: this.$t('Website'), value: "website", width: "27%" },
        { text: this.$tc('Service', 2), value: "serviceList", sortable: false },
        { text: '', value: 'actions', sortable: false, align: "center", width: "4%" },
      ];},

    selectedServiceCount () {
      let count = 0;
      this.selectedServices.forEach(isSelected => { if (isSelected) count++; })
      return count;
    }
  },

  watch: {
    dialog (val) {
      val || this.close();
    }
  },

  methods: {
    async initialize () { 
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.services = await ServiceData.config(apiRequest).all();
      this.platforms = await PlatformData.config(apiRequest).all();
      this.buildServiceLists();
    },

    buildServiceLists () {
      this.platforms.forEach(platform => {
        this.buildServiceList(platform);
      });
    },

    buildServiceList (platform) {
      platform.serviceList = "";
      if (!platform.services)
        return;

      platform.services.forEach(service => {
        platform.serviceList += service.name + ", ";
      });
      let listLength = platform.serviceList.length;
      if (listLength > 2)
          platform.serviceList = platform.serviceList.substring(0, listLength - 2);
    },

    editPlatform (platform) {
      for (let i = 0; i < this.services.length; i++) {
        let platformHasService = false;
        if (platform)
          platformHasService = platform.services.some(platformService => platformService.id == this.services[i].id);
        this.selectedServices[i] = platformHasService;
      }
      this.editedIndex = this.platforms.indexOf(platform);
      this.editedPlatform = Object.assign({}, platform);
      this.dialog = true;
    },

    close () {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.editedPlatform = Object.assign({}, this.defaultPlatform);
      });
    },

    save () {
      if (this.editedPlatform) {
        this.editedPlatform.services = new Array(this.selectedServiceCount);
        let editedPlatformServiceIndex = 0;
        for (let i = 0; i < this.services.length; i++) {
          if (this.selectedServices[i]) {
            this.editedPlatform.services[editedPlatformServiceIndex] = this.services[i];
            editedPlatformServiceIndex++;
          }
        }
        this.buildServiceList(this.editedPlatform);
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const platformData = new PlatformData(this.editedPlatform);
        platformData.config(apiRequest).save()
        .then (() => {
          this.initialize();
        })
        .catch(error => this.handleError(error));
      }
      this.close();
    },

    handleError(error) {
      this.error = error;
    },
  },

  async mounted () {
    this.initialize();
  }
};
</script>
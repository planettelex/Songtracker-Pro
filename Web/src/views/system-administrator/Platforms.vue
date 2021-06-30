<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row v-if="showAddedAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showAddedAlert" type="success" dismissible>{{ addedPlatform.name }} {{ $t('Added') }}</v-alert>
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
                      <v-text-field hide-details="true" :label="$tc('Platform', 1)" v-model="editedPlatform.name"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPlatform.name.$error">{{ validationMessages(v$.editedPlatform.name.$errors) }}</span>
                    </v-col>
                    <v-col cols="8" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('Website')" v-model="editedPlatform.website"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPlatform.website.$error">{{ validationMessages(v$.editedPlatform.website.$errors) }}</span>
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
import ApiRequestHeaders from '../../models/local/ApiRequestHeaders';
import PlatformModel from '../../models/api/Platform';
import ServiceModel from '../../models/api/Service';
import useVuelidate from '@vuelidate/core'
import { required, url } from '@vuelidate/validators'
import { mapState } from "vuex";

export default {
  name: "Platforms",

  setup () {
    return { v$: useVuelidate() }
  },

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
    addedPlatform: {
      id: -1,
      name: '',
      website: '',
      services: []
    },
    showAddedAlert: false,
    error: null
  }),

  computed: {
    ...mapState(["Authentication"]),
    RequestHeaders: {
      get () { return new ApiRequestHeaders(this.Authentication.authenticationToken); }
    },
    
    formTitle() {
      let verb = this.editedIndex === -1 ? this.$t('New') : this.$t('Edit');
      return verb + ' ' + this.$tc('Platform', 1);
    },

    headers() {
      return [
        { text: this.$tc('Platform', 1), value: "name" },
        { text: this.$t('Website'), value: "website" },
        { text: this.$tc('Service', 2), value: "serviceList", sortable: false, width: "50%" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
      ];},

    selectedServiceCount() {
      let count = 0;
      this.selectedServices.forEach(isSelected => { if (isSelected) count++; })
      return count;
    }
  },

  validations () {
    return {
      editedPlatform: {
        name: { required },
        website: { url }
      },
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  methods: {
    async initialize() { 
      this.services = await ServiceModel.config(this.RequestHeaders).all();
      this.platforms = await PlatformModel.config(this.RequestHeaders).all();
      this.buildServiceLists();
    },

    buildServiceLists() {
      this.platforms.forEach(platform => {
        this.buildServiceList(platform);
      });
    },

    buildServiceList(platform) {
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

    editPlatform(platform) {
      for (let i = 0; i < this.services.length; i++) {
        let platformHasService = false;
        if (platform)
          platformHasService = platform.services.some(platformService => platformService.id == this.services[i].id);
        this.selectedServices[i] = platformHasService;
      }
      this.editedIndex = this.platforms.indexOf(platform);
      if (this.editedIndex != -1)
        this.showAddedAlert = false;
      let emptyPlatform = JSON.parse(JSON.stringify(this.defaultPlatform));
      this.editedPlatform = Object.assign(emptyPlatform, platform);
      this.dialog = true;
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.editedPlatform = Object.assign({}, this.defaultPlatform);
        for (let i = 0; i < this.selectedServices.length; i++) {
          this.selectedServices[i] = false;
        }
        this.v$.$reset();
      });
    },

    async save() {
      const formIsValid = await this.v$.$validate();
      if (!formIsValid) 
        return;

      if (this.editedPlatform) {
        let isAdded = false;
        if (!this.editedPlatform.id) {
          isAdded = true;
          this.addedPlatform = Object.assign({}, this.editedPlatform);
        }
        else {
          this.showAddedAlert = false;
        }
        this.editedPlatform.services = new Array(this.selectedServiceCount);
        let editedPlatformServiceIndex = 0;
        for (let i = 0; i < this.services.length; i++) {
          if (this.selectedServices[i]) {
            this.editedPlatform.services[editedPlatformServiceIndex] = this.services[i];
            editedPlatformServiceIndex++;
          }
        }
        this.buildServiceList(this.editedPlatform);
        const platformModel = new PlatformModel(this.editedPlatform);
        platformModel.config(this.RequestHeaders).save()
          .then (() => {
            if (isAdded) {
              this.showAddedAlert = true;
            }
            this.initialize();
          })
          .catch(error => this.handleError(error));
      }
      this.close();
    },

    validationMessages(errors) {
      let messages = '';
      errors.forEach(error => {
        messages += error.$message + ' ';
      });
      return messages.trim();
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

<style lang="scss">
  .v-data-table-header > tr > th:first-child {
    min-width: 120px;
  }
  .v-data-table-header > tr > th:nth-child(2) {
    min-width: 155px;
  }
  .v-data-table-header > tr > th:nth-child(3) {
    min-width: 155px;
  }
</style>
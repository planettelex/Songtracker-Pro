<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="publishers" sort-by="name" must-sort>

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('PublishingCompany', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="800px">
            <template v-slot:activator="{ attrs }">
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="editPublisher(null)">{{ $t('New') }} {{ $tc('Publisher', 1) }}</v-btn>
            </template>
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ formTitle }}</span>
              </v-card-title>
              <v-divider />
              <v-card-text>
                <v-container class="app-form">
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field :label="$t('Name')" v-model="editedPublisher.name"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('TaxIndentifier')" v-model="editedPublisher.taxId"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-text-field :label="$t('PhoneNumber')" v-model="editedPublisher.phone"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field :label="$t('Email')" v-model="editedPublisher.email"></v-text-field>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field :label="$t('Address')" v-model="editedPublisher.address.street"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field :label="$t('City')" v-model="editedPublisher.address.city"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('PostalCode')" v-model="editedPublisher.address.postalCode"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-select :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object single-line></v-select>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object single-line></v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-select :label="$t('AffiliatedPro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object single-line></v-select>
                    </v-col>
                     <v-col cols="3">
                      <v-text-field :label="$t('ProIdentifier')" v-model="editedPublisher.performingRightsOrganizationPublisherNumber"></v-text-field>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="close">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="save">{{ $t('Save') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small @click="editPublisher(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ApiRequest from '../../models/local/ApiRequest';
import CountryData from '../../models/api/Country';
import PerformingRightsOrganizationData from '../../models/api/PerformingRightsOrganization';
import PublisherData from '../../models/api/Publisher';
import CountryRegions from '../../resources/countryRegions';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "PublishingCompanies",

  data: () => ({
    dialog: false,
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    performingRightsOrganizations: [],
    selectedPerformingRightsOrganization: null,
    publishers: [],
    editedIndex: -1,
    editedPublisher: {
      id: -1,
      name: '',
      performingRightsOrganization: null,
      performingRightsOrganizationPublisherNumber: '',
      email: '',
      phone: '',
      taxId: '',
      address: {
        street: '',
        city: '',
        region: '',
        postalCode: '',
        country: {
          id: -1,
          name: '',
          isoCode: ''
        }
      }
    },
    defaultPublisher: {
      name: '',
      performingRightsOrganization: null,
      performingRightsOrganizationPublisherNumber: '',
      email: '',
      phone: '',
      taxId: '',
      address: {
        street: '',
        city: '',
        region: '',
        postalCode: '',
        country: {
          name: '',
          isoCode: ''
        }
      }
    },
    error: null
  }),

  computed: {
    ...mapState(["Login"]),

    formTitle () {
        let verb = this.editedIndex === -1 ? this.$t('New') : this.$t('Edit');
        return verb + ' ' + this.$tc('PublishingCompany', 1);
    },

    headers () {
      return [
        { text: this.$tc('PublishingCompany', 1), value: "name" },
        { text: this.$t('AffiliatedPro'), value: "performingRightsOrganization.name", width: "20%" },
        { text: this.$t('ProIdentifier'), value: "performingRightsOrganizationPublisherNumber", width: "20%" },
        { text: this.$t('TaxIndentifier'), value: "taxId", width: "20%" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "4%" },
      ];},
  },

  watch: {
    dialog (val) {
      val || this.close();
    },
    selectedCountry (val) {
      if (val)
        this.loadCountryRegions();
    }
  },

  methods: {
    async initialize () { 
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.countries = await CountryData.config(apiRequest).all();
      this.performingRightsOrganizations = await PerformingRightsOrganizationData.config(apiRequest).all();
      this.publishers = await PublisherData.config(apiRequest).all();
    },

    loadCountryRegions() {
      if (this.selectedCountry == null)
        this.countryRegions = new Array(0);
      else {
        let culture = appConfig.locale + '-' + this.selectedCountry.isoCode;
        this.countryRegions = CountryRegions[culture];
      }
    },

    getCountryRegion(code) {
      let countryRegion = null;
      if (this.countryRegions)
        this.countryRegions.forEach(region => {
          if (region.code == code) {
            countryRegion = region;
          }
        });
      return countryRegion;
    },

    editPublisher (publisher) {
      if (publisher && !publisher.address)
          publisher.address = this.defaultPublisher.address;
      this.editedIndex = this.publishers.indexOf(publisher);
      this.editedPublisher = Object.assign(this.defaultPublisher, publisher);
      if (publisher) {
        this.selectedCountry = publisher.address.country;
        this.loadCountryRegions();
        this.selectedCountryRegion = this.getCountryRegion(publisher.address.region);
        this.selectedPerformingRightsOrganization = publisher.performingRightsOrganization;
      }
      this.dialog = true;
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.selectedPerformingRightsOrganization = null;
        this.editedPublisher = Object.assign({}, this.defaultPublisher);
      });
    },

    save() {
      if (this.editedPublisher) {
        this.editedPublisher.address.country = this.selectedCountry;
        this.editedPublisher.address.region = this.selectedCountryRegion.code;
        this.editedPublisher.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const publisherData = new PublisherData(this.editedPublisher);
        publisherData.config(apiRequest).save()
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
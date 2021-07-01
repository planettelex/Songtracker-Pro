<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row v-if="showAddedAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showAddedAlert" type="success" dismissible>{{ addedPublisher.name }} {{ $t('Added') }}</v-alert>
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
                      <v-text-field hide-details="true" :label="$t('Name')" v-model="editedPublisher.name"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.name.$error">{{ validationMessages(v$.editedPublisher.name.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('TaxIdentifier')" v-model="editedPublisher.taxId"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.taxId.$error">{{ validationMessages(v$.editedPublisher.taxId.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('PhoneNumber')" v-model="editedPublisher.phone"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.phone.$error">{{ validationMessages(v$.editedPublisher.phone.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('Email')" v-model="editedPublisher.email"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.email.$error">{{ validationMessages(v$.editedPublisher.email.$errors) }}</span>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('Address')" v-model="editedPublisher.address.street"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.address.street.$error">{{ validationMessages(v$.editedPublisher.address.street.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('City')" v-model="editedPublisher.address.city"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.address.city.$error">{{ validationMessages(v$.editedPublisher.address.city.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('PostalCode')" v-model="editedPublisher.address.postalCode"></v-text-field>
                      <span class="validation-error" v-if="v$.editedPublisher.address.postalCode.$error">{{ validationMessages(v$.editedPublisher.address.postalCode.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" >
                      <v-select hide-details="true" :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object></v-select>
                      <span class="validation-error" v-if="v$.selectedCountry.isoCode.$error">{{ validationMessages(v$.selectedCountry.isoCode.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select hide-details="true" :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object></v-select>
                      <span class="validation-error" v-if="v$.selectedCountryRegion.$error">{{ validationMessages(v$.selectedCountryRegion.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-select hide-details="true" :label="$t('AffiliatedPro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                     <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('ProIdentifier')" v-model="editedPublisher.performingRightsOrganizationPublisherNumber"></v-text-field>
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
        <v-icon small @click="editPublisher(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ErrorHandler from '../../models/local/ErrorHandler';
import ApiRequestHeaders from '../../models/local/ApiRequestHeaders';
import CountryModel from '../../models/api/Country';
import PerformingRightsOrganizationModel from '../../models/api/PerformingRightsOrganization';
import PublisherModel from '../../models/api/Publisher';
import CountryRegions from '../../resources/countryRegions';
import useVuelidate from '@vuelidate/core';
import { required, email, minLength } from '@vuelidate/validators';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "PublishingCompanies",

  setup () {
    return { v$: useVuelidate() }
  },

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
      performingRightsOrganizationPublisherNumber: null,
      email: '',
      phone: '',
      taxId: null,
      address: {
        street: null,
        city: null,
        region: null,
        postalCode: null,
        country: {
          id: -1,
          name: null,
          isoCode: null
        }
      }
    },
    defaultPublisher: {
      name: '',
      performingRightsOrganization: null,
      performingRightsOrganizationPublisherNumber: null,
      email: '',
      phone: null,
      taxId: null,
      address: {
        street: null,
        city: null,
        region: null,
        postalCode: null,
        country: {
          name: null,
          isoCode: null
        }
      }
    },
    addedPublisher: {
      id: -1,
      name: '',
      performingRightsOrganization: null,
      performingRightsOrganizationPublisherNumber: null,
      email: '',
      phone: null,
      taxId: null,
      address: {
        street: null,
        city: null,
        region: null,
        postalCode: null,
        country: {
          name: null,
          isoCode: null
        }
      }
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
        return verb + ' ' + this.$tc('PublishingCompany', 1);
    },

    headers() {
      return [
        { text: this.$tc('PublishingCompany', 1), value: "name" },
        { text: this.$t('AffiliatedPro'), value: "performingRightsOrganization.name" },
        { text: this.$t('ProIdentifier'), value: "performingRightsOrganizationPublisherNumber" },
        { text: this.$t('TaxIdentifier'), value: "taxId" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
      ];},
  },

  validations () {
    return {
      selectedCountry: {
        isoCode: { required }
      },
      selectedCountryRegion: { required },
      editedPublisher: {
        name: { required },
        email: { required, email },
        phone: { minLengthValue: minLength(10) },
        taxId: { minLengthValue: minLength(9) },
        address: {
          street: { required },
          city: { required },
          postalCode: { required, minLengthValue: minLength(5) }
        }
      },
    }
  },

  watch: {
    dialog(val) {
      if (val) {
        this.loadCountries();
        this.loadPerformingRightsOrganizations();
      }
      val || this.close();
    },

    selectedCountry(val) {
      if (val)
        this.loadCountryRegions();
    }
  },

  methods: {
    async initialize() { 
      this.publishers = await PublisherModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    async loadCountries() {
      this.countries = await CountryModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
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

    async loadPerformingRightsOrganizations() {
      this.performingRightsOrganizations = await PerformingRightsOrganizationModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    editPublisher(publisher) {
      if (publisher && !publisher.address)
          publisher.address = this.defaultPublisher.address;
      this.editedIndex = this.publishers.indexOf(publisher);
      if (this.editedIndex != -1)
        this.showAddedAlert = false;
      let emptyPublisher = JSON.parse(JSON.stringify(this.defaultPublisher));
      this.editedPublisher = Object.assign(emptyPublisher, publisher);
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
        this.v$.$reset();
      });
    },

    async save() {
      const formIsValid = await this.v$.$validate();
      if (!formIsValid) 
        return;

      if (this.editedPublisher) {
        let isAdded = false;
        this.editedPublisher.address.country = this.selectedCountry;
        this.editedPublisher.address.region = this.selectedCountryRegion.code;
        this.editedPublisher.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        if (!this.editedPublisher.id) {
          isAdded = true;
          this.addedPublisher = Object.assign({}, this.editedPublisher);
        }
        else {
          this.showAddedAlert = false;
        }

        const publisherModel = new PublisherModel(this.editedPublisher);
        publisherModel.config(this.RequestHeaders).save()
          .then (() => {
            if (isAdded) this.showAddedAlert = true;
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
      this.error = new ErrorHandler(error).handleError(this.$router);
    },
  },

  async mounted() {
    this.initialize();
  }
};
</script>

<style lang="scss">
  .v-data-table-header > tr > th:first-child {
    min-width: 210px;
  }
  .v-data-table-header > tr > th:nth-child(2) {
    min-width: 155px;
  }
  .v-data-table-header > tr > th:nth-child(3) {
    min-width: 155px;
  }
  .v-data-table-header > tr > th:nth-child(4) {
    min-width: 155px;
  }
</style>
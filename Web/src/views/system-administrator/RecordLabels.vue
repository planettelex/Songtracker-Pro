<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="recordLabels" sort-by="name" must-sort>

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('RecordLabel', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="800px">
            <template v-slot:activator="{ attrs }">
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="editRecordLabel(null)">{{ $t('New') }} {{ $tc('RecordLabel', 1) }}</v-btn>
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
                      <v-text-field :label="$t('Name')" v-model="editedRecordLabel.name"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('TaxIndentifier')" v-model="editedRecordLabel.taxId"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-text-field :label="$t('PhoneNumber')" v-model="editedRecordLabel.phone"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field :label="$t('Email')" v-model="editedRecordLabel.email"></v-text-field>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field :label="$t('Address')" v-model="editedRecordLabel.address.street"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field :label="$t('City')" v-model="editedRecordLabel.address.city"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('PostalCode')" v-model="editedRecordLabel.address.postalCode"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-select :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object single-line></v-select>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object single-line></v-select>
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
        <v-icon small @click="editRecordLabel(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ApiRequest from '../../models/local/ApiRequest';
import CountryData from '../../models/api/Country';
import RecordLabelData from '../../models/api/RecordLabel';
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
    recordLabels: [],
    editedIndex: -1,
    editedRecordLabel: {
      id: -1,
      name: '',
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
    defaultRecordLabel: {
      name: '',
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
        return verb + ' ' + this.$tc('RecordLabel', 1);
    },

    headers () {
      return [
        { text: this.$tc('RecordLabel', 1), value: "name" },
        { text: this.$t('TaxIndentifier'), value: "taxId", width: "40%" },
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
      this.recordLabels = await RecordLabelData.config(apiRequest).all();
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

    editRecordLabel (recordLabel) {
      if (recordLabel && !recordLabel.address)
          recordLabel.address = this.defaultRecordLabel.address;
      this.editedIndex = this.recordLabels.indexOf(recordLabel);
      let emptyRecordLabel = JSON.parse(JSON.stringify(this.defaultRecordLabel));
      this.editedRecordLabel = Object.assign(emptyRecordLabel, recordLabel);
      if (recordLabel) {
        this.selectedCountry = recordLabel.address.country;
        this.loadCountryRegions();
        this.selectedCountryRegion = this.getCountryRegion(recordLabel.address.region);
      }
      this.dialog = true;
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.editedRecordLabel = Object.assign({}, this.defaultRecordLabel);
      });
    },

    save() {
      if (this.editedRecordLabel) {
        this.editedRecordLabel.address.country = this.selectedCountry;
        this.editedRecordLabel.address.region = this.selectedCountryRegion.code;
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const recordLabelData = new RecordLabelData(this.editedRecordLabel);
        recordLabelData.config(apiRequest).save()
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
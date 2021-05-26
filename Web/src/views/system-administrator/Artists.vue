<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="artists" sort-by="name" must-sort>

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('Artist', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog v-model="dialog" max-width="800px">
            <template v-slot:activator="{ attrs }">
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="editArtist(null)">{{ $t('New') }} {{ $tc('Artist', 1) }}</v-btn>
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
                      <v-text-field :label="$t('Name')" v-model="editedArtist.name"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('TaxIndentifier')" v-model="editedArtist.taxId"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object single-line></v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field :label="$t('Email')" v-model="editedArtist.email"></v-text-field>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field :label="$t('Address')" v-model="editedArtist.address.street"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field :label="$t('City')" v-model="editedArtist.address.city"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('PostalCode')" v-model="editedArtist.address.postalCode"></v-text-field>
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
        <v-icon small @click="editArtist(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ApiRequest from '../../models/local/ApiRequest';
import CountryData from '../../models/api/Country';
import RecordLabelData from '../../models/api/RecordLabel';
import ArtistData from '../../models/api/Artist';
import CountryRegions from '../../resources/countryRegions';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "Artists",

  data: () => ({
    dialog: false,
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    recordLabels: [],
    selectedRecordLabel: null,
    artists: [],
    editedIndex: -1,
    editedArtist: {
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
        },
        hasServiceMark: false,
        websiteUrl: '',
        pressKitUrl: '',
        recordLabel : null
      },

    },
    defaultArtist: {
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
        },
        hasServiceMark: false,
        websiteUrl: '',
        pressKitUrl: '',
        recordLabel : null
      }
    },
    error: null
  }),

  computed: {
    ...mapState(["Login"]),

    formTitle () {
        let verb = this.editedIndex === -1 ? this.$t('New') : this.$t('Edit');
        return verb + ' ' + this.$tc('Artist', 1);
    },

    headers () {
      return [
        { text: this.$tc('Artist', 1), value: "name" },
        { text: this.$tc('RecordLabel', 1), value: "recordLabel.name", width: "30%" },
        { text: this.$t('TaxIndentifier'), value: "taxId", width: "30%" },
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
      this.artists = await ArtistData.config(apiRequest).all();
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

    editArtist (artist) {
      if (artist && !artist.address)
          artist.address = this.defaultArtist.address;
      this.editedIndex = this.artists.indexOf(artist);
      let emptyArtist = JSON.parse(JSON.stringify(this.defaultArtist));
      this.editedArtist = Object.assign(emptyArtist, artist);
      if (artist) {
        this.selectedCountry = artist.address.country;
        this.loadCountryRegions();
        this.selectedCountryRegion = this.getCountryRegion(artist.address.region);
        this.selectedRecordLabel = artist.recordLabel;
      }
      this.dialog = true;
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.selectedRecordLabel = null;
        this.editedArtist = Object.assign({}, this.defaultArtist);
      });
    },

    save() {
      if (this.editedArtist) {
        this.editedArtist.address.country = this.selectedCountry;
        this.editedArtist.address.region = this.selectedCountryRegion.code;
        this.editedArtist.recordLabel = this.selectedRecordLabel;
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const artistData = new ArtistData(this.editedArtist);
        artistData.config(apiRequest).save()
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
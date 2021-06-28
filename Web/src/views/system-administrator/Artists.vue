<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row v-if="showAddedAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showAddedAlert" type="success" dismissible>{{ addedArtist.name }} {{ $t('Added') }}</v-alert>
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
              <v-tabs v-if="editedIndex > -1" v-model="tab">
                <v-tab>{{ $t('Info') }}</v-tab>
                <v-tab>{{ $tc('Member', 2) }}</v-tab>
                <v-tab>{{ $tc('Manager', 2) }}</v-tab>
                <v-tab>{{ $tc('Account', 2) }}</v-tab>
                <v-tab>{{ $tc('Link', 2) }}</v-tab>
              </v-tabs>
              <v-card-text class="artist-modal-content">
                <!-- Info Tab -->
                <v-container v-if="tab == 0" class="app-form">
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('Name')" v-model="editedArtist.name"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.name.$error">{{ validationMessages(v$.editedArtist.name.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('TaxIdentifier')" v-model="editedArtist.taxId"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.taxId.$error">{{ validationMessages(v$.editedArtist.taxId.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select hide-details="true" :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('Email')" v-model="editedArtist.email"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.email.$error">{{ validationMessages(v$.editedArtist.email.$errors) }}</span>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('Address')" v-model="editedArtist.address.street"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.address.street.$error">{{ validationMessages(v$.editedArtist.address.street.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('City')" v-model="editedArtist.address.city"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.address.city.$error">{{ validationMessages(v$.editedArtist.address.city.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('PostalCode')" v-model="editedArtist.address.postalCode"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.address.postalCode.$error">{{ validationMessages(v$.editedArtist.address.postalCode.$errors) }}</span>
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
                      <v-checkbox hide-details="true" :label="$t('HasServiceMark')" v-model="editedArtist.hasServiceMark"></v-checkbox>
                    </v-col>
                  </v-row>
                </v-container>
                <!-- Members Tab -->
                <v-container v-if="tab == 1">
                  <v-data-table dense hide-default-footer :headers="artistMemberHeaders" :items="artistMembers">

                    <template v-slot:[`item.startedOn`]="{ item }">
                      <v-menu v-model="editedArtistMemberStartedOnMenu" v-if="item.id === editedArtistMember.id" :close-on-content-click="false">
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field dense single-line readonly class="small-font" prepend-icon="mdi-calendar" :hide-details="true"
                                        v-model="editedArtistMember.startedOn" v-bind="attrs" v-on="on">
                          </v-text-field>
                        </template>
                        <v-date-picker no-title @input="editedArtistMemberStartedOnMenu = false" v-model="editedArtistMember.startedOn"></v-date-picker>
                      </v-menu>
                      <span v-else>{{item.startedOn}}</span>
                    </template>

                    <template v-slot:[`item.endedOn`]="{ item }">
                      <v-menu v-model="editedArtistMemberEndedOnMenu" v-if="item.id === editedArtistMember.id" :close-on-content-click="false">
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field dense single-line readonly class="small-font" prepend-icon="mdi-calendar" :hide-details="true"
                                        v-model="editedArtistMember.endedOn" v-bind="attrs" v-on="on">
                          </v-text-field>
                        </template>
                        <v-date-picker no-title @input="editedArtistMemberEndedOnMenu = false" v-model="editedArtistMember.endedOn"></v-date-picker>
                      </v-menu>
                      <span v-else>{{item.endedOn}}</span>
                    </template>

                    <template v-slot:[`item.actions`]="{ item }">
                      <div v-if="item.id === editedArtistMember.id">
                        <v-icon small color="red" class="mr-3" @click="closeEditArtistMember">mdi-window-close</v-icon>
                        <v-icon small color="green" @click="updateArtistMember(item)">mdi-content-save</v-icon>
                      </div>
                      <div v-else>
                        <v-icon small @click="editArtistMember(item)">mdi-pencil</v-icon>
                      </div>
                    </template>

                  </v-data-table>
                </v-container>
                <!-- Managers Tab -->
                <v-container v-if="tab == 2">
                  <v-data-table dense hide-default-footer :headers="artistManagerHeaders" :items="artistManagers">

                    <template v-slot:[`item.startedOn`]="{ item }">
                      <v-menu v-model="editedArtistManagerStartedOnMenu" v-if="item.id === editedArtistManager.id" :close-on-content-click="false">
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field dense single-line readonly class="small-font" prepend-icon="mdi-calendar" :hide-details="true"
                                        v-model="editedArtistManager.startedOn" v-bind="attrs" v-on="on">
                          </v-text-field>
                        </template>
                        <v-date-picker no-title @input="editedArtistManagerStartedOnMenu = false" v-model="editedArtistManager.startedOn"></v-date-picker>
                      </v-menu>
                      <span v-else>{{item.startedOn}}</span>
                    </template>

                    <template v-slot:[`item.endedOn`]="{ item }">
                      <v-menu v-model="editedArtistManagerEndedOnMenu" v-if="item.id === editedArtistManager.id" :close-on-content-click="false">
                        <template v-slot:activator="{ on, attrs }">
                          <v-text-field dense single-line readonly class="small-font" prepend-icon="mdi-calendar" :hide-details="true"
                                        v-model="editedArtistManager.endedOn" v-bind="attrs" v-on="on">
                          </v-text-field>
                        </template>
                        <v-date-picker no-title @input="editedArtistManagerEndedOnMenu = false" v-model="editedArtistManager.endedOn"></v-date-picker>
                      </v-menu>
                      <span v-else>{{item.endedOn}}</span>
                    </template>

                    <template v-slot:[`item.actions`]="{ item }">
                      <div v-if="item.id === editedArtistManager.id">
                        <v-icon small color="red" class="mr-3" @click="closeEditArtistManager">mdi-window-close</v-icon>
                        <v-icon small color="green" @click="updateArtistManager(item)">mdi-content-save</v-icon>
                      </div>
                      <div v-else>
                        <v-icon small @click="editArtistManager(item)">mdi-pencil</v-icon>
                      </div>
                    </template>

                  </v-data-table>
                </v-container>
                <!-- Accounts Tab -->
                <v-container v-if="tab == 3">
                  <div class="datatable-toolbar">
                    <v-toolbar flat dense>
                      <v-select style="margin-bottom: -7px; width: 26%" dense hide-details="true" class="small-font ml-2 mr-2" :label="$tc('Platform', 1)" :items="platforms" v-model="selectedAccountPlatform" item-text="name" item-value="id" return-object></v-select>
                      <v-text-field style="width: 24%" hide-details="true" class="small-font mr-2" :label="$t('Username')"></v-text-field>
                      <v-checkbox style="margin-bottom: -10px;" hide-details="true" class="small-font" :label="$t('Preferred')"></v-checkbox>
                      <v-spacer></v-spacer>
                      <v-icon style="margin-bottom: -10px" @click="addNewArtistAccount">mdi-plus</v-icon>
                    </v-toolbar>
                  </div>
                  <v-data-table dense hide-default-footer :headers="artistAccountHeaders" :items="artistAccounts">

                    <template v-slot:[`item.actions`]="{ item }">
                      <div v-if="item.id === editedArtistAccount.id">
                        <v-icon small color="red" class="mr-3" @click="closeEditArtistAccount">mdi-window-close</v-icon>
                        <v-icon small color="green" @click="updateArtistAccount(item)">mdi-content-save</v-icon>
                      </div>
                      <div v-else>
                        <v-icon small @click="editArtistAccount(item)">mdi-pencil</v-icon>
                      </div>
                    </template>

                  </v-data-table>
                </v-container>
                <!-- Links Tab -->
                <v-container v-if="tab == 4">
                  <v-row>
                    <v-col cols="6" class="pl-5">
                      <v-text-field hide-details="true" class="mr-1" :label="$t('Website') + ' ' + $tc('Link', 1)" v-model="editedArtist.websiteUrl"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.websiteUrl.$error">{{ validationMessages(v$.editedArtist.websiteUrl.$errors) }}</span>
                    </v-col>
                    <v-col cols="6">
                      <v-text-field hide-details="true" :label="$t('PressKit') + ' ' + $tc('Link', 1)" v-model="editedArtist.pressKitUrl"></v-text-field>
                      <span class="validation-error" v-if="v$.editedArtist.pressKitUrl.$error">{{ validationMessages(v$.editedArtist.pressKitUrl.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <div class="datatable-toolbar">
                    <v-toolbar flat dense>
                      <v-select style="margin-bottom: -7px; width: 26%" dense hide-details="true" class="small-font ml-2 mr-2" :label="$tc('Platform', 1)" :items="platforms" v-model="selectedLinkPlatform" item-text="name" item-value="id" return-object></v-select>
                      <v-text-field style="width: 50%" hide-details="true" class="small-font mr-2" :label="$tc('Link', 1)"></v-text-field>
                      <v-spacer></v-spacer>
                      <v-icon style="margin-bottom: -10px" @click="addNewArtistLink">mdi-plus</v-icon>
                    </v-toolbar>
                  </div>
                  <v-data-table dense hide-default-footer :headers="artistLinkHeaders" :items="artistLinks">

                    <template v-slot:[`item.actions`]="{ item }">
                      <div v-if="item.id === editedArtistLink.id">
                        <v-icon small color="red" class="mr-3" @click="closeEditArtistLink">mdi-window-close</v-icon>
                        <v-icon small color="green" @click="updateArtistLink(item)">mdi-content-save</v-icon>
                      </div>
                      <div v-else>
                        <v-icon small @click="editArtistLink(item)">mdi-pencil</v-icon>
                      </div>
                    </template>

                  </v-data-table>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="close">{{ $t('Cancel') }}</v-btn>
                <v-btn v-if="tab == 0 || tab == 4" class="v-button mr-4 rounded" @click="save">{{ $t('Save') }}</v-btn>
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
import PlatformData from '../../models/api/Platform';
import RecordLabelData from '../../models/api/RecordLabel';
import ArtistData from '../../models/api/Artist';
import ArtistMemberData from '../../models/api/ArtistMember';
import ArtistManagerData from '../../models/api/ArtistManager';
import ArtistAccountData from '../../models/api/ArtistAccount';
import ArtistLinkData from '../../models/api/ArtistLink';
import CountryRegions from '../../resources/countryRegions';
import useVuelidate from '@vuelidate/core';
import { required, email, url, minLength } from '@vuelidate/validators';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "Artists",

  setup () {
    return { v$: useVuelidate() }
  },

  data: () => ({
    dialog: false,
    tab: 0,
    formTitle: '',
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    platforms: [],
    recordLabels: [],
    selectedRecordLabel: null,
    artists: [],
    artistMembers: [],
    artistManagers: [],
    artistAccounts: [],
    artistLinks: [],
    editedIndex: null,
    editedArtistData: null,
    editedArtist: {
      id: -1,
      name: '',
      email: '',
      phone: null,
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
        },
        hasServiceMark: false,
        websiteUrl: null,
        pressKitUrl: null,
        recordLabel : null
      },
    },
    defaultArtist: {
      name: '',
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
        },
        hasServiceMark: false,
        websiteUrl: null,
        pressKitUrl: null,
        recordLabel : null
      }
    },
    addedArtist: {
      id: -1,
      name: '',
      email: '',
      phone: null,
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
        },
        hasServiceMark: false,
        websiteUrl: null,
        pressKitUrl: null,
        recordLabel : null
      },
    },
    editedArtistMemberIndex: -1,
    editedArtistMember: {
      id: -1,
      member: null,
      isActive: false,
      startedOn: null,
      endedOn: null
    },
    defaultArtistMember: {
      member: null,
      isActive: false,
      startedOn: null,
      endedOn: null
    },
    editedArtistMemberStartedOnMenu: false,
    editedArtistMemberEndedOnMenu: false,
    editedArtistManagerIndex: -1,
    editedArtistManager: {
      id: -1,
      manager: null,
      isActive: false,
      startedOn: null,
      endedOn: null
    },
    defaultArtistManager: {
      manager: null,
      isActive: false,
      startedOn: null,
      endedOn: null
    },
    editedArtistManagerStartedOnMenu: false,
    editedArtistManagerEndedOnMenu: false,
    editedArtistAccountIndex: -1,
    editedArtistAccount: {
      id: -1,
      platform: null,
      username: null,
      isPreferred: false
    },
    defaultArtistAccount: {
      platform: null,
      username: null,
      isPreferred: false
    },
    selectedAccountPlatform: null,
    editedArtistLinkIndex: -1,
    editedArtistLink: {
      id: -1,
      platform: null,
      url: null
    },
    defaultArtistLink: {
      platform: null,
      url: null
    },
    selectedLinkPlatform: null,
    showAddedAlert: false,
    error: null
  }),

  computed: {
    ...mapState(["Login"]),

    headers() {
      return [
        { text: this.$tc('Artist', 1), value: "name" },
        { text: this.$tc('RecordLabel', 1), value: "recordLabel.name" },
        { text: this.$t('TaxIdentifier'), value: "taxId" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
      ];
    },

    artistMemberHeaders() {
      return [
        { text: this.$t('Name'), value: "member.firstAndLastName" },
        { text: this.$t('StartedOn'), value: "startedOn", width: "25%" },
        { text: this.$t('EndedOn'), value: "endedOn", width: "25%" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },

    artistManagerHeaders() {
      return [
        { text: this.$t('Name'), value: "manager.firstAndLastName" },
        { text: this.$t('StartedOn'), value: "startedOn", width: "25%" },
        { text: this.$t('EndedOn'), value: "endedOn", width: "25%" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },

    artistAccountHeaders() {
      return [
        { text: this.$tc('Platform', 1), value: "platform.name", width: "37%" },
        { text: this.$t('Username'), value: "username" },
        { text: this.$t('Preferred'), value: "isPreferred", width: "120px" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },

    artistLinkHeaders() {
      return [
        { text: this.$tc('Platform', 1), value: "platform.name", width: "32%" },
        { text: this.$tc('Link', 1), value: "url" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },
  },

  validations () {
    return {
      selectedCountry: {
        isoCode: { required }
      },
      selectedCountryRegion: { required },
      editedArtist: {
        name: { required },
        email: { email },
        taxId: { minLengthValue: minLength(9) },
        address: {
          street: { required },
          city: { required },
          postalCode: { required, minLengthValue: minLength(5) }
        },
        websiteUrl: { url },
        pressKitUrl: { url }
      },
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    },
    tab(val) {
      switch (val) {
        case 0:
          this.setFormTitle();
          break;
        case 1:
          this.loadArtistMembers();
          this.setFormTitle(this.$tc('Member', 2));
          break;
        case 2:
          this.loadArtistManagers();
          this.setFormTitle(this.$tc('Manager', 2));
          break;
        case 3:
          this.loadArtistAccounts();
          this.loadPlatforms();
          this.setFormTitle(this.$tc('Account', 2));
          break;
        case 4:
          this.loadArtistLinks();
          this.loadPlatforms();
          this.setFormTitle(this.$tc('Link', 2));
          break;
      }
    },
    editedIndex() {
      this.setFormTitle();
    },
    selectedCountry(val) {
      if (val)
        this.loadCountryRegions();
    }
  },

  methods: {
    async initialize() { 
      this.editedIndex = -1;
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.countries = await CountryData.config(apiRequest).all();
      this.recordLabels = await RecordLabelData.config(apiRequest).all();
      this.artists = await ArtistData.config(apiRequest).all();
    },

    setFormTitle(tabName) {
        if (tabName) {
          this.formTitle = this.editedArtist.name + ' ' + tabName;
        }
        else {
          let verb = this.editedIndex == -1 ? this.$t('New') : this.$t('Edit');
          this.formTitle = verb + ' ' + this.$tc('Artist', 1);
        }
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

    async loadPlatforms() {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.platforms = await PlatformData.config(apiRequest).all();
    },

    async loadArtistMembers() {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artistMembers = await ArtistMemberData.config(apiRequest).custom(this.editedArtistData, 'members').get();
      this.artistMembers.forEach(artistMember => {
        artistMember.startedOn = artistMember.startedOn.substring(0,10);
        if (artistMember.endedOn)
          artistMember.endedOn = artistMember.endedOn.substring(0,10);
      });
    },

    async loadArtistManagers() {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artistManagers = await ArtistManagerData.config(apiRequest).custom(this.editedArtistData, 'managers').get();
      this.artistManagers.forEach(artistManager => {
        artistManager.startedOn = artistManager.startedOn.substring(0,10);
        if (artistManager.endedOn)
          artistManager.endedOn = artistManager.endedOn.substring(0,10);
      });
    },

    async loadArtistAccounts() {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artistAccounts = await ArtistAccountData.config(apiRequest).custom(this.editedArtistData, 'accounts').get();
    },

    async loadArtistLinks() {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artistLinks = await ArtistLinkData.config(apiRequest).custom(this.editedArtistData, 'links').get();
    },

    async editArtist(artist) {
      if (artist && !artist.address)
          artist.address = this.defaultArtist.address;
      this.editedIndex = this.artists.indexOf(artist);
      if (this.editedIndex != -1)
        this.showAddedAlert = false;
      let emptyArtist = JSON.parse(JSON.stringify(this.defaultArtist));
      this.editedArtist = Object.assign(emptyArtist, artist);
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      if (artist) {
        this.editedArtistData = await ArtistData.config(apiRequest).find(artist.id);
        this.selectedCountry = artist.address.country;
        this.loadCountryRegions();
        this.selectedCountryRegion = this.getCountryRegion(artist.address.region);
        this.selectedRecordLabel = artist.recordLabel;
      }
      this.dialog = true;
    },

    editArtistMember(artistMember) {
      this.editedArtistMemberIndex = this.artistMembers.indexOf(artistMember);
      this.editedArtistMember = Object.assign({}, artistMember);
    },

    closeEditArtistMember() {
      setTimeout(() => {
        this.editedArtistMember = Object.assign({}, this.defaultArtistMember);
        this.editedArtistMemberIndex = -1;
      }, 300)
    },

    async updateArtistMember(artistMember) {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      // TODO: The following code does not work.
      const artist = await ArtistData.config(apiRequest).find(this.editedArtistData.id);
      await artist.members().sync(artistMember);
      // --------------------------------------
      this.closeEditArtistMember();
    },

    editArtistManager(artistManager) {
      this.editedArtistManagerIndex = this.artistMembers.indexOf(artistManager);
      this.editedArtistManager = Object.assign({}, artistManager);
    },

    closeEditArtistManager() {
      setTimeout(() => {
        this.editedArtistManager = Object.assign({}, this.defaultArtistManager);
        this.editedArtistManagerIndex = -1;
      }, 300)
    },

    async updateArtistManager(artistManager) {
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      // TODO: The following code does not work.
      const artist = await ArtistData.config(apiRequest).find(this.editedArtistData.id);
      await artist.managers().sync(artistManager);
      // --------------------------------------
      this.closeEditArtistMember();
    },

    addNewArtistAccount() {
      const newArtistAccount = Object.assign({}, this.defaultArtistAccount);
      this.artistAccounts.unshift(newArtistAccount);
    },

    editArtistAccount(artistAccount) {
      this.editedArtistAccountIndex = this.artistAccounts.indexOf(artistAccount);
      this.editedArtistAccount = Object.assign({}, artistAccount);
    },

    closeEditArtistAccount() {
      setTimeout(() => {
        this.editedArtistAccount = Object.assign({}, this.defaultArtistAccount);
        this.editedArtistAccountIndex = -1;
      }, 300)
    },

    addNewArtistLink() {
      const newArtistLink = Object.assign({}, this.defaultArtistLink);
      this.artistLinks.unshift(newArtistLink);
    },

    editArtistLink(artistLink) {
      this.editedArtistLinkIndex = this.artistLinks.indexOf(artistLink);
      this.editedArtistLink = Object.assign({}, artistLink);
    },

    closeEditArtistLink() {
      setTimeout(() => {
        this.editedArtistLink = Object.assign({}, this.defaultArtistLink);
        this.editedArtistLinkIndex = -1;
      }, 300)
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.tab = 0;
        this.editedIndex = -1;
        this.editedArtistData = null;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.selectedRecordLabel = null;
        this.editedArtist = Object.assign({}, this.defaultArtist);
        this.v$.$reset();
      });
    },

    async save() {
      const formIsValid = await this.v$.$validate();
      if (!formIsValid) 
        return;

      if (this.editedArtist) {
        let isAdded = false;
        this.editedArtist.address.country = this.selectedCountry;
        this.editedArtist.address.region = this.selectedCountryRegion.code;
        this.editedArtist.recordLabel = this.selectedRecordLabel;
        if (!this.editedArtist.id) {
          isAdded = true;
          this.addedArtist = Object.assign({}, this.editedArtist);
        }
        else {
          this.showAddedAlert = false;
        }
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const artistData = new ArtistData(this.editedArtist);
        artistData.config(apiRequest).save()
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
  .artist-modal-content {
    height: 325px;
  }
  .datatable-toolbar {
    .v-toolbar__content {
      padding: 0;
    }
  }
</style>
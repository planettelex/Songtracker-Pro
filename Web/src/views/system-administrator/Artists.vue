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
                      <v-text-field :label="$t('Name')" v-model="editedArtist.name"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('TaxIdentifier')" v-model="editedArtist.taxId"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object></v-select>
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
                      <v-select :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object></v-select>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object></v-select>
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
                      <v-select dense class="small-font" :label="$tc('Platform', 1)" :items="platforms" v-model="selectedAccountPlatform" item-text="name" item-value="id" return-object></v-select>
                      <v-text-field class="small-font" :label="$t('Username')"></v-text-field>
                      <v-checkbox class="small-font" :label="$t('Preferred')"></v-checkbox>
                      <v-spacer></v-spacer>

                        <v-icon @click="addNewArtistAccount">mdi-plus</v-icon> 

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
                  Links
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
import PlatformData from '../../models/api/Platform';
import RecordLabelData from '../../models/api/RecordLabel';
import ArtistData from '../../models/api/Artist';
import ArtistMemberData from '../../models/api/ArtistMember';
import ArtistManagerData from '../../models/api/ArtistManager';
import ArtistAccountData from '../../models/api/ArtistAccount';
import CountryRegions from '../../resources/countryRegions';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "Artists",

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
        { text: this.$tc('Platform', 1), value: "platform.name", width: "30%" },
        { text: this.$t('Username'), value: "username" },
        { text: this.$t('Is') + ' ' + this.$t('Preferred'), value: "isPreferred", width: "150px" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },
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

    async editArtist(artist) {
      if (artist && !artist.address)
          artist.address = this.defaultArtist.address;
      this.editedIndex = this.artists.indexOf(artist);
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

<style lang="scss">
  .artist-modal-content {
    height: 350px;
  }
  .datatable-toolbar {
    .v-toolbar__content {
      padding: 0;
    }
  }
</style>
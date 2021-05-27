<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="users">
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('User', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <v-dialog v-model="inviteDialog" max-width="800px">
            <template v-slot:activator="{ attrs }">
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="inviteUser()">{{ $t('Invite') }} {{ $tc('User', 1) }}</v-btn>
            </template>
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ $t('Invite') }} {{ $tc('User', 1) }}</span>
              </v-card-title>
              <v-card-text></v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="closeInvite">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="inviteUser">{{ $t('Invite') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="editDialog" max-width="800px">
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ $t('Edit') }} {{ $tc('User', 1) }}</span>
              </v-card-title>
              <v-card-text></v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="closeEdit">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="editUser">{{ $t('Save') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small @click="editUser(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ApiRequest from '../../models/local/ApiRequest';
import CountryData from '../../models/api/Country';
import CountryRegions from '../../resources/countryRegions';
import UserTypes from '../../models/local/UserTypes';
import UserRoles from '../../models/local/UserRoles';
import PerformingRightsOrganizationData from '../../models/api/PerformingRightsOrganization';
import PublisherData from '../../models/api/Publisher';
import UserData from '../../models/api/User';
import InvitationData from '../../models/api/Invitation';
import ArtistData from '../../models/api/Artist';
import RecordLabelData from '../../models/api/RecordLabel';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "Users",

  data: () => ({
    inviteDialog: false,
    editDialog: false,
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    performingRightsOrganizations: [],
    selectedPerformingRightsOrganization: null,
    artists: [],
    selectedArtist: null,
    publishers: [],
    selectedPublisher: null,
    recordLabels: [],
    selectedRecordLabel: null,
    userTypes: [],
    selectedUserType: null,
    userRoles: [],
    selectedUserRoles: [ false, false, false, false, false, false, false, false, false, false ],
    users: [],
    editedInvitation: {
      uuid: '',
      invitedByUserId: -1,
      name: '',
      email: '',
      type: 0,
      roles: 0,
      publisher: null,
      recordLabel: null,
      artist: null
    },
    defaultInvitation: {
      invitedByUserId: -1,
      name: '',
      email: '',
      type: 0,
      roles: 0,
      publisher: null,
      recordLabel: null,
      artist: null
    },
    editedIndex: -1,
    editedUser: {
      id: -1,
      authenticationId: '',
      type: 0,
      roles: 0,
      person: {
        id: -1,
        firstName: '',
        middleName: '',
        lastName: '',
        nameSuffix: '',
        email: '',
        phone: '',
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
        },
        socialSecurityNumber: '',
        publisher: null,
        recordLabel: null,
        performingRightsOrganization: null,
        performingRightsOrganizationMemberNumber: '',
        soundExchangeAccountNumber: ''
      }
    },
    defaultUser: {
      authenticationId: '',
      type: 0,
      roles: 0,
      person: {
        firstName: '',
        middleName: '',
        lastName: '',
        nameSuffix: '',
        email: '',
        phone: '',
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
        },
        socialSecurityNumber: '',
        publisher: null,
        recordLabel: null,
        performingRightsOrganization: null,
        performingRightsOrganizationMemberNumber: '',
        soundExchangeAccountNumber: ''
      }
    },
    error: null
  }),

  computed: {
    ...mapState(["Login"]),

    headers() {
      return [
        { text: this.$tc('User', 1), value: "name" },
        { text: this.$tc('Type', 1), value: "typeName" },
        { text: this.$tc('Role', 2), value: "roles" },
        { text: this.$t('AffiliatedPro'), value: "performingRightsOrganization.name" },
        { text: this.$t('ProIdentifier'), value: "performingRightsOrganizationMemberNumber" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
      ];},
  },

  watch: {
    inviteDialog(val) {
      val || this.inviteClose();
    },
    editDialog(val) {
      val || this.editClose();
    },
  },

  methods: {
    async initialize() { 
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.countries = await CountryData.config(apiRequest).all();
      this.performingRightsOrganizations = await PerformingRightsOrganizationData.config(apiRequest).all();
      this.publishers = await PublisherData.config(apiRequest).all();
      this.userTypes = UserTypes;
      this.userTypes.forEach(userType => {
        userType.name = this.$t(userType.key);
      });
      this.userRoles = UserRoles;
      this.userRoles.forEach(userRole => {
        userRole.name = this.$tc(userRole.key, 1);
      });
      this.users = await UserData.config(apiRequest).all();
      this.users.forEach(user => {
        user.typeName = this.getUserType(user.type).name;
      });
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

    async inviteUser() {
      for (let i = 0; i < this.userRoles.length; i++) {
        this.selectedUserRoles[i] = false;
      }
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artists = await ArtistData.config(apiRequest).all();
      this.recordLabels = await RecordLabelData.config(apiRequest).all();
      this.editedInvitation = Object.assign({}, this.defaultInvitation);
      this.inviteDialog = true; 
    },

    closeInvite() {
      this.inviteDialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.selectedUserType = null;
        this.selectedPublisher = null;
        this.selectedRecordLabel = null;
        this.selectedArtist = null;
        this.editedInvitation = Object.assign({}, this.defaultInvitation);
      });
    },

    sendInvite() {
      if (this.editedInvitation) {
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const invitationData = new InvitationData(this.editedInvitation);
        invitationData.config(apiRequest).save()
        .then (() => {
          this.initialize();
        })
        .catch(error => this.handleError(error));
      }
      this.closeInvite();
    },

    editUser(user) {
      for (let i = 0; i < this.userRoles.length; i++) {
        this.selectedUserRoles[i] = false;
      }
      if (user && !user.address)
        user.address = this.defaultUser.address;
      this.editedIndex = this.users.indexOf(user);
      this.editedUser = Object.assign({}, this.defaultUser);
      if (user) {
        this.selectedUserType = this.getUserType(user.type);
        this.selectedCountry = user.address.country;
        this.loadCountryRegions();
        this.selectedCountryRegion = this.getCountryRegion(user.address.region);
        this.selectedPerformingRightsOrganization = user.performingRightsOrganization;
        this.selectedPublisher = user.publisher;
      }
      this.editDialog = true;
    },

    getUserType(typeValue) {
      let userType = null;
      this.userTypes.forEach(type => {
        if (type.value == typeValue)
          userType = type;
      });
      return userType;
    },

    closeEdit() {
      this.editDialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.selectedPerformingRightsOrganization = null;
        this.selectedPublisher = null;
        this.editedUser = Object.assign({}, this.defaultUser);
      });
    },

    save() {
      if (this.editedUser) {
        this.editedUser.person.address.country = this.selectedCountry;
        this.editedUser.person.address.region = this.selectedCountryRegion.code;
        this.editedUser.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        this.editedUser.publisher = this.selectedPublisher;
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const userData = new UserData(this.editedUser);
        userData.config(apiRequest).save()
        .then (() => {
          this.initialize();
        })
        .catch(error => this.handleError(error));
      }
      this.closeEdit();
    },

    handleError(error) {
      this.error = error;
    },
  },
  
  async mounted() {
    this.initialize();
  }
};
</script>

<style lang="scss">
  .v-data-table-header > tr > th:first-child {
    min-width: 50px;
  }
  .v-data-table-header > tr > th:nth-child(2) {
    min-width: 100px;
  }
  .v-data-table-header > tr > th:nth-child(3) {
    min-width: 90px;
  }
  .v-data-table-header > tr > th:nth-child(4) {
    min-width: 155px;
  }
  .v-data-table-header > tr > th:nth-child(5) {
    min-width: 155px;
  }
</style>
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
              <v-btn class="v-button rounded mt-3" v-bind="attrs" @click="inviteUser()"><v-icon class="pr-3">mdi-email</v-icon> {{ $t('Invite') }} {{ $tc('User', 1) }}</v-btn>
            </template>
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ $t('Invite') }} {{ $tc('User', 1) }}</span>
              </v-card-title>
              <v-card-text>
                <v-row>
                  <v-col cols="5">
                    <v-select :label="$tc('User', 1) + ' ' + $tc('Type', 1)" :items="userTypes" v-model="selectedUserType" item-text="name" item-value="value" return-object></v-select>
                  </v-col>
                  <v-col cols="3">
                    <v-text-field :label="$t('Name')" v-model="editedInvitation.name"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field :label="$t('Email')" v-model="editedInvitation.email"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="showPublisherFields">
                  <v-col cols="5">
                    <v-select :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showLabelFields">
                  <v-col cols="5">
                    <v-select :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showUserFields">
                  <v-col cols="5">
                    <v-select :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                  <v-col cols="7">
                    <v-select :label="$tc('Artist', 1)" :items="artists" v-model="selectedArtist" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showUserFields">
                    <v-col cols="12">
                      <v-select :label="$tc('Role', 2)" :items="userRoles" v-model="selectedUserRoles" item-text="name" item-value="value" multiple></v-select>
                    </v-col>
                  </v-row>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="closeInvite">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="sendInvite">{{ $t('Invite') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <v-dialog v-model="editDialog" max-width="800px">
            <v-card>
              <v-card-title class="modal-title pt-2">
                <span>{{ $t('Edit') }} {{ selectedUserType.name }}</span>
              </v-card-title>
              <v-card-text>
                <v-container class="app-form">
                  <v-row>
                    <v-col cols="2" class="pl-0">
                      <v-text-field :label="$t('First')" v-model="editedUser.person.firstName"></v-text-field>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field :label="$t('MiddleAbbreviation')" v-model="editedUser.person.middleName"></v-text-field>
                    </v-col>
                    <v-col cols="2">
                      <v-text-field :label="$t('Last')" v-model="editedUser.person.lastName"></v-text-field>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field :label="$t('Suffix')" v-model="editedUser.person.nameSuffix"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('SSN')" v-model="editedUser.socialSecurityNumber"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-text-field :label="$t('PhoneNumber')" v-model="editedUser.person.phone"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field :label="$t('Email')" v-model="editedUser.authenticationId"></v-text-field>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field :label="$t('Address')" v-model="editedUser.person.address.street"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field :label="$t('City')" v-model="editedUser.person.address.city"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-text-field :label="$t('PostalCode')" v-model="editedUser.person.address.postalCode"></v-text-field>
                    </v-col>
                    <v-col cols="3">
                      <v-select :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object></v-select>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object></v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="4" class="pl-0">
                      <v-select :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                    <v-col cols="2">
                      <v-select :label="$t('AffiliatedPro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field :label="$t('ProIdentifier')" v-model="editedUser.performingRightsOrganizationMemberNumber"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-text-field :label="$t('SoundExchangeId')" v-model="editedUser.soundExchangeAccountNumber"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="12" class="pl-0 pr-0">
                      <v-select :label="$tc('Role', 2)" :items="userRoles" v-model="selectedUserRoles" item-text="name" item-value="value" multiple></v-select>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button rounded" @click="closeEdit">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="save" :disabled="disableSave">{{ $t('Save') }}</v-btn>
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
import PerformingRightsOrganizationData from '../../models/api/PerformingRightsOrganization';
import PublisherData from '../../models/api/Publisher';
import UserData from '../../models/api/User';
import InvitationData from '../../models/api/Invitation';
import ArtistData from '../../models/api/Artist';
import RecordLabelData from '../../models/api/RecordLabel';
import UserType from '../../enums/UserType';
import UserTypes from '../../models/local/UserTypes';
import UserRoles from '../../models/local/UserRoles';
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
    userType: UserType,
    userTypes: [],
    selectedUserType: {},
    userRoles: [],
    selectedUserRoles: [],
    users: [],
    showPublisherFields: false,
    showLabelFields: false,
    showUserFields: false,
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
    disableSave: false,
    error: null
  }),

  computed: {
    ...mapState(["Login", "User"]),

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
      val || this.closeInvite();
    },
    editDialog(val) {
      val || this.closeEdit();
    },
    selectedCountry(val) {
      if (val)
        this.loadCountryRegions();
    },
    selectedUserType(val) {
      switch(val.value) {
        case UserType.PublisherAdministrator:
          this.showPublisherFields = true;
          this.showLabelFields = false;
          this.showUserFields = false;
          break;
        case UserType.LabelAdministrator:
          this.showPublisherFields = false;
          this.showLabelFields = true;
          this.showUserFields = false;
          break;
        case UserType.SystemUser:
          this.showPublisherFields = false;
          this.showLabelFields = false;
          this.showUserFields = true;
          break;
        default:
          this.showPublisherFields = false;
          this.showLabelFields = false;
          this.showUserFields = false;
          break;
      }
    }
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
      let apiRequest = new ApiRequest(this.Login.authenticationToken);
      this.artists = await ArtistData.config(apiRequest).all();
      this.recordLabels = await RecordLabelData.config(apiRequest).all();
      let emptyInvitation = JSON.parse(JSON.stringify(this.defaultInvitation));
      this.editedInvitation = Object.assign(emptyInvitation, this.defaultInvitation);
      this.inviteDialog = true;
    },

    closeInvite() {
      this.inviteDialog = false;
      this.$nextTick(() => {
        this.selectedPublisher = null;
        this.selectedRecordLabel = null;
        this.selectedArtist = null;
        this.selectedUserRoles = [];
        this.selectedUserType = {};
        this.editedInvitation = Object.assign({}, this.defaultInvitation);
      });
    },

    sendInvite() {
      if (this.editedInvitation) {
        this.editedInvitation.invitedByUserId = this.User.id;
        let userType = this.selectedUserType.value;
        let userRoles = 0;
        this.editedInvitation.type = userType;
        switch (userType) {
          case UserType.PublisherAdministrator:
            this.editedInvitation.publisher = this.selectedPublisher;
            break;
          case UserType.LabelAdministrator:
            this.editedInvitation.recordLabel = this.selectedRecordLabel;
            break;
          case UserType.SystemUser:
            this.editedInvitation.publisher = this.selectedPublisher;
            this.editedInvitation.artist = this.selectedArtist;
            for (let i = 0; i < this.selectedUserRoles.length; i++) {
              userRoles = userRoles | this.selectedUserRoles[i];
            }
            this.editedInvitation.roles = userRoles;
            break;
        }
        let apiRequest = new ApiRequest(this.Login.authenticationToken);
        const invitationData = new InvitationData(this.editedInvitation);
        invitationData.config(apiRequest).save()
        .then (() => {
          this.initialize();
        })
        .catch(error => this.handleError(error));
        console.log(this.editedInvitation);
      }
      this.inviteDialog = false;
    },

    editUser(user) {
      if (user && !user.address)
        user.address = this.defaultUser.address;
      if (user && !user.person)
        user.person = this.defaultUser.person;
      for (let i = 0; i < UserRoles.length; i++) {
        if (user.roles & UserRoles[i].value)
          this.selectedUserRoles.push(UserRoles[i].value);
      }
      if (!user.person.id)
        this.disableSave = true;
      this.editedIndex = this.users.indexOf(user);
      this.editedUser = Object.assign({}, user);
      if (user) {
        this.selectedUserType = this.getUserType(user.type);
        if (user.person.address) {
          this.selectedCountry = user.person.address.country;
          this.loadCountryRegions();
          this.selectedCountryRegion = this.getCountryRegion(user.person.address.region);
        }
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
        this.selectedUserRoles = [];
        this.selectedUserType = {};
        this.disableSave = false;
        this.editedUser = Object.assign({}, this.defaultUser);
      });
    },

    save() {
      if (this.editedUser) {
        this.editedUser.person.address.country = this.selectedCountry;
        if (this.selectedCountryRegion)
          this.editedUser.person.address.region = this.selectedCountryRegion.code;
        this.editedUser.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        this.editedUser.publisher = this.selectedPublisher;
        let userRoles = 0;
        this.selectedUserRoles.forEach(userRole => userRoles = userRoles | userRole);
        this.editedUser.roles = userRoles;
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
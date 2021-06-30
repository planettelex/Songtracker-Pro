<template>
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row v-if="showInvitedUserAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showInvitedUserAlert" type="success" dismissible>{{ $t('InvitationSentTo') }} {{ addedInvitation.email }}</v-alert>
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
              <v-divider />
              <v-card-text class="user-invite-modal-content">
                <v-row>
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('User', 1) + ' ' + $tc('Type', 1)" :items="userTypes" v-model="selectedUserType" item-text="name" item-value="value" return-object></v-select>
                    <span class="validation-error" v-if="v$.selectedUserType.$error">{{ validationMessages(v$.selectedUserType.$errors) }}</span>
                  </v-col>
                  <v-col cols="3">
                    <v-text-field hide-details="true" :label="$t('Name')" v-model="editedInvitation.name"></v-text-field>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field hide-details="true" :label="$t('Email')" v-model="editedInvitation.email"></v-text-field>
                  </v-col>
                </v-row>
                <v-row v-if="showPublisherFields">
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showLabelFields">
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showUserFields">
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                  <v-col cols="7">
                    <v-select hide-details="true" :label="$tc('Artist', 1)" :items="artists" v-model="selectedArtist" item-text="name" item-value="id" return-object></v-select>
                  </v-col>
                </v-row>
                <v-row v-if="showUserFields">
                    <v-col cols="12">
                      <v-select hide-details="true" :label="$tc('Role', 2)" :items="userRoles" v-model="selectedUserRoles" item-text="name" item-value="value" multiple></v-select>
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
              <v-divider />
              <v-tabs v-model="editTab">
                <v-tab>{{ $t('Info') }}</v-tab>
                <v-tab>{{ $tc('Account', 2) }}</v-tab>
              </v-tabs>
              <v-card-text class="user-modal-content">
                <!-- Info Tab -->
                <v-container v-if="editTab == 0" class="app-form">
                  <v-row>
                    <v-col cols="2" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('First')" v-model="editedUser.person.firstName"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.firstName.$error">{{ validationMessages(v$.editedUser.person.firstName.$errors) }}</span>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field hide-details="true" :label="$t('MiddleAbbreviation')" v-model="editedUser.person.middleName"></v-text-field>
                    </v-col>
                    <v-col cols="2">
                      <v-text-field hide-details="true" :label="$t('Last')" v-model="editedUser.person.lastName"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.lastName.$error">{{ validationMessages(v$.editedUser.person.lastName.$errors) }}</span>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field hide-details="true" :label="$t('Suffix')" v-model="editedUser.person.nameSuffix"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-text-field hide-details="true" :label="$t('PhoneNumber')" v-model="editedUser.person.phone"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.phone.$error">{{ validationMessages(v$.editedUser.person.phone.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0" v-if="showUserFields">
                      <v-text-field hide-details="true" :label="$t('SSN')" v-model="editedUser.socialSecurityNumber"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.socialSecurityNumber.$error">{{ validationMessages(v$.editedUser.socialSecurityNumber.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('Email')" v-model="editedUser.authenticationId"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.authenticationId.$error">{{ validationMessages(v$.editedUser.authenticationId.$errors) }}</span>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('Address')" v-model="editedUser.person.address.street"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('City')" v-model="editedUser.person.address.city"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-text-field hide-details="true" :label="$t('PostalCode')" v-model="editedUser.person.address.postalCode"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.address.postalCode.$error">{{ validationMessages(v$.editedUser.person.address.postalCode.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-select hide-details="true" :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object></v-select>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select hide-details="true" :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object></v-select>
                    </v-col>
                  </v-row>
                  <v-row v-if="showUserFields || showPublisherFields">
                    <v-col cols="4" class="pl-0">
                      <v-select hide-details="true" :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                    <v-col cols="2">
                      <v-select hide-details="true" :label="$t('AffiliatedPro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object></v-select>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('ProIdentifier')" v-model="editedUser.performingRightsOrganizationMemberNumber"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0" v-if="showUserFields">
                      <v-text-field hide-details="true" :label="$t('SoundExchangeId')" v-model="editedUser.soundExchangeAccountNumber"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row v-if="showUserFields">
                    <v-col cols="12" class="pl-0 pr-0">
                      <v-select hide-details="true" :label="$tc('Role', 2)" :items="userRoles" v-model="selectedUserRoles" item-text="name" item-value="value" multiple></v-select>
                    </v-col>
                  </v-row>
                </v-container>
                <!-- Accounts Tab -->
                <v-container v-if="editTab == 1">
                  <div class="datatable-toolbar">
                    <v-toolbar flat dense>
                      <v-select style="margin-bottom: -7px; width: 26%" dense hide-details="true" class="small-font ml-2 mr-2" :label="$tc('Platform', 1)" :items="platforms" v-model="selectedAccountPlatform" item-text="name" item-value="id" return-object></v-select>
                      <v-text-field style="width: 24%" hide-details="true" class="small-font mr-2" :label="$t('Username')"></v-text-field>
                      <v-checkbox style="margin-bottom: -10px;" hide-details="true" class="small-font" :label="$t('Preferred')"></v-checkbox>
                      <v-spacer></v-spacer>
                      <v-icon style="margin-bottom: -10px" @click="addNewUserAccount">mdi-plus</v-icon>
                    </v-toolbar>
                  </div>
                  <v-data-table dense hide-default-footer :headers="userAccountHeaders" :items="userAccounts">

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
import ApiRequestHeaders from '../../models/local/ApiRequestHeaders';
import CountryModel from '../../models/api/Country';
import CountryRegions from '../../resources/countryRegions';
import PlatformModel from '../../models/api/Platform';
import PerformingRightsOrganizationModel from '../../models/api/PerformingRightsOrganization';
import PublisherModel from '../../models/api/Publisher';
import UserModel from '../../models/api/User';
import UserAccountModel from '../../models/api/UserAccount';
import InvitationModel from '../../models/api/Invitation';
import ArtistModel from '../../models/api/Artist';
import RecordLabelModel from '../../models/api/RecordLabel';
import UserType from '../../enums/UserType';
import UserTypes from '../../models/local/UserTypes';
import UserRoles from '../../models/local/UserRoles';
import useVuelidate from '@vuelidate/core';
import { required, email, minLength } from '@vuelidate/validators';
import { mapState } from "vuex";
import appConfig from '../../appConfig';

export default {
  name: "Users",

  setup () {
    return { v$: useVuelidate() }
  },

  data: () => ({
    inviteDialog: false,
    editDialog: false,
    editTab: 0,
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    platforms: [],
    selectedAccountPlatform: null,
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
    userAccounts: [],
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
    addedInvitation: {
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
    editedIndex: null,
    editedUserData: null,
    editedUser: {
      id: -1,
      authenticationId: '',
      type: 0,
      roles: 0,
      person: {
        id: -1,
        firstName: '',
        middleName: null,
        lastName: '',
        nameSuffix: null,
        email: null,
        phone: null,
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
        },
        socialSecurityNumber: null,
        publisher: null,
        recordLabel: null,
        performingRightsOrganization: null,
        performingRightsOrganizationMemberNumber: null,
        soundExchangeAccountNumber: null
      }
    },
    defaultUser: {
      authenticationId: '',
      type: 0,
      roles: 0,
      person: {
        firstName: '',
        middleName: null,
        lastName: '',
        nameSuffix: null,
        email: null,
        phone: null,
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
        },
        socialSecurityNumber: null,
        publisher: null,
        recordLabel: null,
        performingRightsOrganization: null,
        performingRightsOrganizationMemberNumber: null,
        soundExchangeAccountNumber: null
      }
    },
    editedUserAccountIndex: -1,
    editedUserAccount: {
      id: -1,
      platform: null,
      username: null,
      isPreferred: false
    },
    defaultUserAccount: {
      platform: null,
      username: null,
      isPreferred: false
    },
    disableSave: false,
    showInvitedUserAlert: false,
    error: null
  }),

  computed: {
    ...mapState(["Authentication"]),
    RequestHeaders: {
      get () { return new ApiRequestHeaders(this.Authentication.authenticationToken); }
    },

    ...mapState(["User"]),

    headers() {
      return [
        { text: this.$tc('User', 1), value: "name" },
        { text: this.$tc('Type', 1), value: "typeName" },
        { text: this.$tc('Role', 2), value: "roles" },
        { text: this.$t('AffiliatedPro'), value: "performingRightsOrganization.name" },
        { text: this.$t('ProIdentifier'), value: "performingRightsOrganizationMemberNumber" },
        { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
      ];
    },

    userAccountHeaders() {
      return [
        { text: this.$tc('Platform', 1), value: "platform.name", width: "37%" },
        { text: this.$t('Username'), value: "username" },
        { text: this.$t('Preferred'), value: "isPreferred", width: "120px" },
        { text: '', value: 'actions', sortable: false, align: "right", width: "80px" },
      ];
    },
  },

  validations () {
    return {
      selectedUserType: { required },
      editedInvitation: {
        name: { required },
        email: { required, email },
      },
      editedUser: {
        authenticationId: { required, email },
        socialSecurityNumber: { minLengthValue: minLength(9) },
        person: {
          firstName: { required },
          lastName: { required },
          phone: { minLengthValue: minLength(10) },
          address: {
            postalCode: { minLengthValue: minLength(5) }
          }
        }
      }
    }
  },

  watch: {
    inviteDialog(val) {
      if (val) {
        this.loadCountries();
      }
      val || this.closeInvite();
    },
    editDialog(val) {
      if (val) {
        this.loadCountries();
        this.loadPerformingRightsOrganizations();
        this.loadPublishers();
      }
      val || this.closeEdit();
    },
    editTab(val) {
      switch (val) {
        case 1:
          this.load
          this.loadPlatforms();
          break;
      }
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
      this.userTypes = UserTypes;
      this.userTypes.forEach(userType => {
        userType.name = this.$t(userType.key);
      });
      this.userRoles = UserRoles;
      this.userRoles.forEach(userRole => {
        userRole.name = this.$tc(userRole.key, 1);
      });
      this.users = await UserModel.config(this.RequestHeaders).all();
      this.users.forEach(user => {
        user.typeName = this.getUserType(user.type).name;
      });
    },

    async loadCountries() {
      this.countries = await CountryModel.config(this.RequestHeaders).all();
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
      this.platforms = await PlatformModel.config(this.RequestHeaders).all();
    },

    async loadPerformingRightsOrganizations() {
      this.performingRightsOrganizations = await PerformingRightsOrganizationModel.config(this.RequestHeaders).all();
    },

    async loadPublishers() {
      this.publishers = await PublisherModel.config(this.RequestHeaders).all();
    },

    async loadUserAccounts() {
      this.userAccounts = await UserAccountModel.config(this.RequestHeaders).custom(this.editedUserData, 'accounts').get();
    },

    async inviteUser() {
      this.artists = await ArtistModel.config(this.RequestHeaders).all();
      this.recordLabels = await RecordLabelModel.config(this.RequestHeaders).all();
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
        this.v$.$reset();
      });
    },

    async sendInvite() {
      const formIsValid = await this.v$.$validate();
      if (!formIsValid) 
        return;

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
        const invitationModel = new InvitationModel(this.editedInvitation);
        invitationModel.config(this.RequestHeaders).save()
          .then (() => {
            this.addedInvitation = Object.assign({}, this.editedInvitation);
            this.showInvitedUserAlert = true;
            this.initialize();
          })
          .catch(error => this.handleError(error));
      }
      this.inviteDialog = false;
    },

    async editUser(user) {
      this.showInvitedUserAlert = false;
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
        this.editedUserData = await UserModel.config(this.RequestHeaders).find(user.id);
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

    addNewUserAccount() {
      const newUserAccount = Object.assign({}, this.defaultUserAccount);
      // TODO: API Call
      this.userAccounts.unshift(newUserAccount);
    },

    async editUserAccount(userAccount) {
      this.editedUserAccountIndex = this.artistAccounts.indexOf(userAccount);
      this.editedUserccount = Object.assign({}, userAccount);
      // TODO: The following code does not work.
      const user = await UserModel.config(this.RequestHeaders).find(this.editedUserData.id);
      await user.accounts().sync(this.editedUserAccount);
      // --------------------------------------
      this.closeEditUserAccount();
    },

    closeEditUserAccount() {
      setTimeout(() => {
        this.editedArtistAccount = Object.assign({}, this.defaultArtistAccount);
        this.editedArtistAccountIndex = -1;
      }, 300)
    },

    closeEdit() {
      this.editDialog = false;
      this.$nextTick(() => {
        this.editedIndex = -1;
        this.editedUserData = null;
        this.selectedCountry = null;
        this.selectedCountryRegion = null;
        this.selectedPerformingRightsOrganization = null;
        this.selectedPublisher = null;
        this.selectedUserRoles = [];
        this.selectedUserType = {};
        this.disableSave = false;
        this.editedUser = Object.assign({}, this.defaultUser);
        this.v$.$reset();
      });
    },

    async save() {
      const formIsValid = await this.v$.$validate();
      if (!formIsValid) 
        return;

      if (this.editedUser) {
        this.editedUser.person.address.country = this.selectedCountry;
        if (this.selectedCountryRegion)
          this.editedUser.person.address.region = this.selectedCountryRegion.code;
        this.editedUser.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        this.editedUser.publisher = this.selectedPublisher;
        let userRoles = 0;
        this.selectedUserRoles.forEach(userRole => userRoles = userRoles | userRole);
        this.editedUser.roles = userRoles;
        const userModel = new UserModel(this.editedUser);
        userModel.config(this.RequestHeaders).save()
          .then (() => {
            this.initialize();
          })
          .catch(error => this.handleError(error));
      }
      this.closeEdit();
    },

    validationMessages(errors) {
      let messages = '';
      errors.forEach(error => {
        messages += error.$message + ' ';
      });
      return messages.trim();
    },

    handleError(error) {
      console.log(error);
      //this.$router.push("/login");
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
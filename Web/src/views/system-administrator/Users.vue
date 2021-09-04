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
    <v-row v-if="showEditedUserAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showEditedUserAlert" type="success" dismissible>{{ lastEditedUserName }} {{ $t('Updated') }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="users">

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('User', 2) }}</h2></v-toolbar-title>
          <v-spacer></v-spacer>
          <!-- Invite Dialog -->
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
                    <span class="validation-error" v-if="v$.editedInvitation.name.$error">{{ validationMessages(v$.editedInvitation.name.$errors) }}</span>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field hide-details="true" :label="$t('Email')" v-model="editedInvitation.email"></v-text-field>
                    <span class="validation-error" v-if="v$.editedInvitation.email.$error">{{ validationMessages(v$.editedInvitation.email.$errors) }}</span>
                  </v-col>
                </v-row>
                <v-row v-if="showPublisherFields">
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object></v-select>
                    <span class="validation-error" v-if="showInvitePublisherValidation">{{ $t('ValueIsRequired') }}</span>
                  </v-col>
                </v-row>
                <v-row v-if="showLabelFields">
                  <v-col cols="5">
                    <v-select hide-details="true" :label="$tc('RecordLabel', 1)" :items="recordLabels" v-model="selectedRecordLabel" item-text="name" item-value="id" return-object></v-select>
                    <span class="validation-error" v-if="showInviteLabelValidation">{{ $t('ValueIsRequired') }}</span>
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
                      <span class="validation-error" v-if="showInviteRolesValidation">{{ $t('ValueIsRequired') }}</span>
                    </v-col>
                  </v-row>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button mr-5 rounded" @click="closeInvite">{{ $t('Cancel') }}</v-btn>
                <v-btn class="v-button mr-4 rounded" @click="sendInvite">{{ $t('Invite') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
          <!-- Edit Dialog -->
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
                      <v-text-field hide-details="true" :label="$t('First')" v-model="editedUser.person.firstName" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.firstName.$error">{{ validationMessages(v$.editedUser.person.firstName.$errors) }}</span>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field hide-details="true" :label="$t('MiddleAbbreviation')" v-model="editedUser.person.middleName" :disabled="disableSave"></v-text-field>
                    </v-col>
                    <v-col cols="2">
                      <v-text-field hide-details="true" :label="$t('Last')" v-model="editedUser.person.lastName" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.lastName.$error">{{ validationMessages(v$.editedUser.person.lastName.$errors) }}</span>
                    </v-col>
                    <v-col cols="1">
                      <v-text-field hide-details="true" :label="$t('Suffix')" v-model="editedUser.person.nameSuffix" :disabled="disableSave"></v-text-field>
                    </v-col>
                    <v-col cols="3" >
                      <v-text-field hide-details="true" :label="$t('PhoneNumber')" v-model="editedUser.person.phone" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.person.phone.$error">{{ validationMessages(v$.editedUser.person.phone.$errors) }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0" v-if="showUserFields">
                      <v-text-field hide-details="true" :label="$t('SSN')" v-model="editedUser.socialSecurityNumber" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.socialSecurityNumber.$error">{{ validationMessages(v$.editedUser.socialSecurityNumber.$errors) }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="6" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('Email')" v-model="editedUser.authenticationId" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="v$.editedUser.authenticationId.$error">{{ validationMessages(v$.editedUser.authenticationId.$errors) }}</span>
                    </v-col>
                    <v-col cols="6" class="pr-0">
                      <v-text-field hide-details="true" :label="$t('Address')" v-model="editedUser.person.address.street" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="showStreetValidation">{{ $t('ValueIsRequired') }}</span>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="3" class="pl-0">
                      <v-text-field hide-details="true" :label="$t('City')" v-model="editedUser.person.address.city" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="showCityValidation">{{ $t('ValueIsRequired') }}</span>
                    </v-col>
                    <v-col cols="3" >
                      <v-text-field hide-details="true" :label="$t('PostalCode')" v-model="editedUser.person.address.postalCode" :disabled="disableSave"></v-text-field>
                      <span class="validation-error" v-if="showPostalCodeValidation">{{ $t('ValueIsRequired') }}&nbsp;</span>
                      <span class="validation-error" v-if="v$.editedUser.person.address.postalCode.$error">{{ validationMessages(v$.editedUser.person.address.postalCode.$errors) }}</span>
                    </v-col>
                    <v-col cols="3">
                      <v-select hide-details="true" :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object :disabled="disableSave"></v-select>
                      <span class="validation-error" v-if="showCountryValidataion">{{ $t('ValueIsRequired') }}</span>
                    </v-col>
                    <v-col cols="3" class="pr-0">
                      <v-select hide-details="true" :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object :disabled="disableSave"></v-select>
                      <span class="validation-error" v-if="showCountryRegionValidataion">{{ $t('ValueIsRequired') }}</span>
                    </v-col>
                  </v-row>
                  <v-row v-if="showUserFields || showPublisherFields">
                    <v-col cols="4" class="pl-0">
                      <v-select hide-details="true" :label="$tc('PublishingCompany', 1)" :items="publishers" v-model="selectedPublisher" item-text="name" item-value="id" return-object :disabled="disableSave"></v-select>
                    </v-col>
                    <v-col cols="2">
                      <v-select hide-details="true" :label="$t('AffiliatedPro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object :disabled="disableSave"></v-select>
                    </v-col>
                    <v-col cols="3">
                      <v-text-field hide-details="true" :label="$t('ProIdentifier')" v-model="editedUser.performingRightsOrganizationMemberNumber" :disabled="disableSave"></v-text-field>
                    </v-col>
                    <v-col cols="3" class="pr-0" v-if="showUserFields">
                      <v-text-field hide-details="true" :label="$t('SoundExchangeId')" v-model="editedUser.soundExchangeAccountNumber" :disabled="disableSave"></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row v-if="showUserFields">
                    <v-col cols="12" class="pl-0 pr-0">
                      <v-select hide-details="true" :label="$tc('Role', 2)" :items="userRoles" v-model="selectedUserRoles" item-text="name" item-value="value" multiple :disabled="disableSave"></v-select>
                    </v-col>
                  </v-row>
                </v-container>
                <!-- Accounts Tab -->
                <v-container v-if="editTab == 1">
                  <div class="datatable-toolbar">
                    <v-toolbar flat dense>
                      <v-row>
                        <v-col cols="4">
                          <v-select style="margin-bottom: -7px;" dense :hide-details="true" class="small-font pl-2 pr-1" :label="$tc('Platform', 1)" :items="platforms" v-model="newAccountPlatform" item-text="name" item-value="id" return-object :disabled="disableSave"></v-select>
                          <div style="padding: 8px 0 0 8px;" class="validation-error" v-if="showAccountPlatformValidation">{{ $t('ValueIsRequired') }}</div>
                        </v-col>
                        <v-col cols="4">
                          <v-text-field style="margin-bottom: -7px;" dense :hide-details="true" class="small-font pl-2" :label="$t('Username')" v-model="newAccountUsername" :disabled="disableSave"></v-text-field>
                          <div style="padding: 8px 0 0 8px;" class="validation-error" v-if="showAccountUsernameValidation">{{ $t('ValueIsRequired') }}</div>
                        </v-col>
                        <v-col cols="3">
                          <v-checkbox style="padding-top: 5px;" dense :hide-details="true" class="small-font pl-8" :label="$t('Preferred')" v-model="newAccountIsPreferred" :disabled="disableSave"></v-checkbox>
                        </v-col>
                        <v-col cols="1" class="pr-0">
                          <v-spacer></v-spacer>
                          <v-icon style="margin-bottom: -10px" @click="addNewUserAccount" :disabled="disableSave">mdi-plus</v-icon>
                        </v-col>
                      </v-row>
                    </v-toolbar>
                  </div>
                  <v-data-table dense hide-default-footer :headers="userAccountHeaders" :items="userAccounts">

                    <template v-slot:[`item.platform.name`]="{ item }">
                      <v-select dense style="margin-top: 0;" :hide-details="true" class="small-font" :items="platforms" v-model="editedUserAccount.platform" item-text="name" item-value="id" return-object v-if="item.id === editedUserAccount.id"></v-select>
                      <span v-else>{{ item.platform.name }}</span>
                    </template>
                    
                    <template v-slot:[`item.username`]="{ item }">
                      <v-text-field single-line dense style="margin-top: 0;" :hide-details="true" class="small-font" v-model="editedUserAccount.username" v-if="item.id === editedUserAccount.id"></v-text-field>
                      <span v-else>{{ item.username }}</span>
                    </template>

                    <template v-slot:[`item.isPreferred`]="{ item }">
                      <v-simple-checkbox v-model="editedUserAccount.isPreferred" v-if="item.id === editedUserAccount.id" :ripple="false"></v-simple-checkbox>
                      <v-simple-checkbox v-else v-model="item.isPreferred" disabled></v-simple-checkbox>
                    </template>

                    <template v-slot:[`item.actions`]="{ item }">
                      <div v-if="item.id === editedUserAccount.id">
                        <v-icon small color="red" class="mr-3" @click="closeEditUserAccount">mdi-window-close</v-icon>
                        <v-icon small style="margin-right: -5px;" color="green" @click="updateUserAccount()">mdi-content-save</v-icon>
                      </div>
                      <div v-else>
                        <v-icon small class="mr-3" @click="editUserAccount(item)">mdi-pencil</v-icon>
                        <v-icon small style="margin-right: -5px;" @click="deleteUserAccount(item)">mdi-delete</v-icon>
                      </div>
                    </template>

                  </v-data-table>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn class="v-cancel-button mr-5 rounded" @click="closeEdit">{{ $t('Cancel') }}</v-btn>
                <v-btn v-if="editTab == 0" class="v-button mr-4 rounded" @click="save" :disabled="disableSave">{{ $t('Save') }}</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <template v-slot:[`item.roles`]="{ item }">
        <v-icon class="role-icon role-songwriter" v-if="userInRole(item, systemUserRoles.Songwriter)">mdi-rectangle</v-icon>
        <v-icon class="role-icon role-artistMember" v-if="userInRole(item, systemUserRoles.ArtistMember)">mdi-rectangle</v-icon>
        <v-icon class="role-icon role-artistManager" v-if="userInRole(item, systemUserRoles.ArtistManager)">mdi-rectangle</v-icon>
        <v-icon class="role-icon role-producer" v-if="userInRole(item, systemUserRoles.Producer)">mdi-rectangle</v-icon>
        <v-icon class="role-icon role-visualArtist" v-if="userInRole(item, systemUserRoles.VisualArtist)">mdi-rectangle</v-icon>
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small @click="editUser(item)">mdi-pencil</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ErrorHandler from '../../models/local/ErrorHandler';
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
import SystemUserRoles from '../../enums/SystemUserRoles';
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
    newAccountPlatform: null,
    newAccountUsername: null,
    newAccountIsPreferred: false,
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
    systemUserRoles: SystemUserRoles,
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
    editedUserHasNullAddress: false,
    lastEditedUserName: null,
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
    showStreetValidation: false,
    showCityValidation: false,
    showPostalCodeValidation: false,
    showCountryValidataion: false,
    showCountryRegionValidataion: false,
    showAccountPlatformValidation: false,
    showAccountUsernameValidation: false,
    showInviteRolesValidation: false,
    showInvitePublisherValidation: false,
    showInviteLabelValidation: false,
    hasTriggeredValidation: false,
    showInvitedUserAlert: false,
    showEditedUserAlert: false,
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
        { text: this.$tc('Platform', 1), value: "platform.name", width: "34%" },
        { text: this.$t('Username'), value: "username" },
        { text: this.$t('Preferred'), value: "isPreferred", width: "125px" },
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
          this.loadUserAccounts();
          this.loadPlatforms();
          break;
      }
    },

    'editedUser.person.address.street'(val) {
        const valIsNull = val == null || val.trim() == '';
        this.showStreetValidation = this.hasTriggeredValidation && valIsNull;
    },

    'editedUser.person.address.city'(val) {
        const valIsNull = val == null || val.trim() == '';
        this.showCityValidation = this.hasTriggeredValidation && valIsNull;
    },

    'editedUser.person.address.postalCode'(val) {
        const valIsNull = val == null || val.trim() == '';
        this.showPostalCodeValidation = this.hasTriggeredValidation && valIsNull;
    },

    newAccountPlatform(val) {
      if (val != null && this.showAccountPlatformValidation)
        this.showAccountPlatformValidation = false;
    },

    newAccountUsername(val) {
      if (val != null && this.showAccountUsernameValidation)
        this.showAccountUsernameValidation = false;
    },

    selectedCountry(val) {
      const valIsNull = val == null;
      this.showCountryValidataion = this.hasTriggeredValidation && valIsNull;
      if (!valIsNull)
        this.loadCountryRegions();
    },

    selectedCountryRegion(val) {
        const valIsNull = val == null || val.code.trim() == '';
        this.showCountryRegionValidataion = this.hasTriggeredValidation && valIsNull;
    },

    selectedPublisher(val) {
      if (val != null && this.showInvitePublisherValidation)
        this.showInvitePublisherValidation = false;
    },

    selectedRecordLabel(val) {
      if (val != null && this.showInviteLabelValidation)
        this.showInviteLabelValidation = false;
    },

    selectedUserType(val) {
      switch(val.value) {
        case UserType.PublisherAdministrator:
          this.loadPublishers();
          this.showPublisherFields = true;
          this.showLabelFields = false;
          this.showUserFields = false;
          break;
        case UserType.LabelAdministrator:
          this.loadRecordLabels();
          this.showPublisherFields = false;
          this.showLabelFields = true;
          this.showUserFields = false;
          break;
        case UserType.SystemUser:
          this.loadPublishers();
          this.loadArtists();
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
    },

    selectedUserRoles(val) {
      if (val != null && val.length > 0 && this.showInviteRolesValidation)
        this.showInviteRolesValidation = false;
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
      this.users = await UserModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
      this.users.forEach(user => {
        user.typeName = this.getUserType(user.type).name;
      });
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

    async loadPlatforms() {
      const allPlatforms = await PlatformModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
      this.platforms = [];
      allPlatforms.forEach(platform => {
        platform.services.forEach(service => {
          if (service.name.toLowerCase() == 'payment') {
            this.platforms.push(platform);
          }
        });
      });
    },

    async loadPerformingRightsOrganizations() {
      this.performingRightsOrganizations = await PerformingRightsOrganizationModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    async loadPublishers() {
      this.publishers = await PublisherModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    async loadRecordLabels() {
      this.recordLabels = await RecordLabelModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    async loadArtists() {
      this.artists = await ArtistModel.config(this.RequestHeaders).all()
        .catch(error => this.handleError(error));
    },

    async loadUserAccounts() {
      this.userAccounts = await UserAccountModel.config(this.RequestHeaders).custom(this.editedUserData, 'accounts').get()
        .catch(error => this.handleError(error));
    },

    async inviteUser() {
      let emptyInvitation = JSON.parse(JSON.stringify(this.defaultInvitation));
      this.editedInvitation = Object.assign(emptyInvitation, this.defaultInvitation);
      this.inviteDialog = true;
    },

    userInRole(user, role) {
      return (user.roles & role) == role;
    },

    getUserRole(systemUserRole) {
      this.userRoles.forEach(userRole => {
        if (userRole.value == systemUserRole)
          return userRole;    
      });
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
      let formIsValid = await this.v$.$validate();
      let userType = this.selectedUserType.value;

      if (userType == UserType.SystemUser && (this.selectedUserRoles == null || this.selectedUserRoles.length == 0)) {
        this.showInviteRolesValidation = true;
        formIsValid = false;
      }

      if (userType == UserType.PublisherAdministrator && this.selectedPublisher == null) {
        this.showInvitePublisherValidation = true;
        formIsValid = false;
      }

      if (userType == UserType.LabelAdministrator && this.selectedRecordLabel == null) {
        this.showInviteLabelValidation = true;
        formIsValid = false;
      }

      if (!formIsValid) 
        return;

      if (this.editedInvitation) {
        this.editedInvitation.invitedByUserId = this.User.id;
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
            this.showEditedUserAlert = false;
            this.showInvitedUserAlert = true;
            this.initialize();
          })
          .catch(error => this.handleError(error));
      }
      this.inviteDialog = false;
    },

    async editUser(user) {      
      if (user) {
        for (let i = 0; i < UserRoles.length; i++) {
          if (user.roles & UserRoles[i].value)
            this.selectedUserRoles.push(UserRoles[i].value);
        }

        this.editedIndex = this.users.indexOf(user);
        this.editedUserData = await UserModel.config(this.RequestHeaders).find(user.id)
          .catch(error => this.handleError(error));

        if (!this.editedUserData.person) {
          this.disableSave = true; // Initial superuser has no associated person. Don't allow update on this user.
          this.editedUserData.person = Object.assign({}, this.defaultUser.person);
        }

        if (!this.editedUserData.person.address)
          this.editedUserData.person.address = Object.assign({}, this.defaultUser.person.address);

        this.editedUser = Object.assign({}, this.editedUserData);
        this.selectedUserType = this.getUserType(this.editedUser.type);
        if (this.editedUser.person.address) {
          this.selectedCountry = this.editedUser.person.address.country;
          this.loadCountryRegions();
          this.selectedCountryRegion = this.getCountryRegion(this.editedUser.person.address.region);
        }
        this.selectedPerformingRightsOrganization = this.editedUser.performingRightsOrganization;
        this.selectedPublisher = this.editedUser.publisher;
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

    editedAddressIsValid() {
      const address = this.editedUser.person.address;
      if (!address) {
        this.editedUserHasNullAddress = true;
        return false;
      }
      let streetIsNull = !address.street || address.street.trim() == '';
      let cityIsNull = !address.city || address.city.trim() == '';
      let postalCodeIsNull = !address.postalCode || address.postalCode.trim() == '';
      let countryIsNull = !this.selectedCountry || !this.selectedCountry.isoCode;
      let regionIsNull = !this.selectedCountryRegion || this.selectedCountryRegion.code.trim() == '';
      let addressIsNull = streetIsNull && cityIsNull && regionIsNull && postalCodeIsNull && countryIsNull;
      let addressHasNull = streetIsNull || cityIsNull || regionIsNull || postalCodeIsNull || countryIsNull;

      if (!addressIsNull && addressHasNull) {
        this.showStreetValidation = streetIsNull;
        this.showCityValidation = cityIsNull;
        this.showPostalCodeValidation = postalCodeIsNull;
        this.showCountryValidataion = countryIsNull;
        this.showCountryRegionValidataion = regionIsNull;
        return false;
      }

      if (addressIsNull)
        this.editedUserHasNullAddress = true;

      return true;
    },

    async addNewUserAccount() {
      const newUserAccount = Object.assign({}, this.defaultUserAccount);
      let accountInformationIsValid = true;

      if (this.newAccountPlatform)
        newUserAccount.platform = this.newAccountPlatform;
      else {
        this.showAccountPlatformValidation = true;
        accountInformationIsValid = false;
      }

      if (this.newAccountUsername)
        newUserAccount.username = this.newAccountUsername;
      else {
        this.showAccountUsernameValidation = true;
        accountInformationIsValid = false;
      }

      newUserAccount.isPreferred = this.newAccountIsPreferred;
      if (!accountInformationIsValid)
        return;
      
      const userModel = await UserModel.config(this.RequestHeaders).find(this.editedUserData.id)
        .catch(error => this.handleError(error));
      const userAccountModel = new UserAccountModel(newUserAccount).config(this.RequestHeaders).for(userModel);
      await userAccountModel.save()
        .then (() => {
          this.loadUserAccounts();
          this.newAccountPlatform = null;
          this.newAccountUsername = null;
          this.newAccountIsPreferred = false;
        })
        .catch(error => this.handleError(error));
    },

    async editUserAccount(userAccount) {
      this.editedUserAccountIndex = this.userAccounts.indexOf(userAccount);
      this.editedUserAccount = Object.assign({}, userAccount);
    },

    async updateUserAccount() {
      if (this.editedUserAccount.username == null || this.editedUserAccount.username.trim() == '')
        return;
      
      const userModel = await UserModel.config(this.RequestHeaders).find(this.editedUserData.id)
        .catch(error => this.handleError(error));
      const userAccountModel = new UserAccountModel(this.editedUserAccount).config(this.RequestHeaders).for(userModel);
      await userAccountModel.save()
        .then (() => {
          this.closeEditUserAccount();
          this.loadUserAccounts();
        })
        .catch(error => this.handleError(error));
    },

    closeEditUserAccount() {
      setTimeout(() => {
        this.editedUserAccount = Object.assign({}, this.defaultUserAccount);
        this.editedUserAccountIndex = -1;
      }, 300)
    },

    async deleteUserAccount(userAccount) {
      const userModel = await UserModel.config(this.RequestHeaders).find(this.editedUserData.id)
        .catch(error => this.handleError(error));
      const userAccountModel = new UserAccountModel(userAccount).config(this.RequestHeaders).for(userModel);
      await userAccountModel.delete()
        .then (() => {
          this.loadUserAccounts();
        })
        .catch(error => this.handleError(error));
    },

    closeEdit() {
      this.editDialog = false;
      this.$nextTick(() => {
        this.editTab = 0;
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
        this.editedUserHasNullAddress = false;
        this.showStreetValidation = false;
        this.showCityValidation = false;
        this.showPostalCodeValidation = false;
        this.showCountryValidataion = false;
        this.showCountryRegionValidataion = false;
        this.showAccountPlatformValidation = false;
        this.showAccountUsernameValidation = false;
        this.hasTriggeredValidation = false;
        this.v$.$reset();
      });
    },

    async save() {
      await this.v$.$validate();
      let formIsValid = !this.v$.editedUser.$error && this.editedAddressIsValid();
      this.hasTriggeredValidation = true;
      if (!formIsValid) 
        return;

      if (this.editedUser) {
        let emptyUser = JSON.parse(JSON.stringify(this.defaultUser));
        let editedUser = JSON.parse(JSON.stringify(this.editedUser));
        let userToSave = Object.assign(emptyUser, editedUser);

        if (this.editedUserHasNullAddress)
          userToSave.person.address = null;
         else {
          userToSave.person.address.country = this.selectedCountry;
          if (this.selectedCountryRegion) 
            userToSave.person.address.region = this.selectedCountryRegion.code;
        }

        userToSave.performingRightsOrganization = this.selectedPerformingRightsOrganization;
        userToSave.publisher = this.selectedPublisher;
        let userRoles = 0;
        this.selectedUserRoles.forEach(userRole => userRoles = userRoles | userRole);
        userToSave.roles = userRoles;
        const userModel = new UserModel(userToSave);
        userModel.config(this.RequestHeaders).save()
          .then (() => {
            this.lastEditedUserName = userToSave.person.firstName + ' ' + userToSave.person.lastName;
            this.showInvitedUserAlert = false;
            this.showEditedUserAlert = true;
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
      this.error = new ErrorHandler(error).handleError(this.$router);
    },
  },
  
  async mounted() {
    this.initialize();
  }
};
</script>

<style lang="scss">
  .user-modal-content {
    min-height: 300px;
  }
  .datatable-toolbar {
    .v-toolbar__content {
      padding: 0;
    }
  }
</style>
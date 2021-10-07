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
    <v-row v-if="showInviteResentAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showInviteResentAlert" type="success" dismissible>{{ $t('InvitationSentTo') }} {{ resentToEmail }}</v-alert>
      </v-col>
    </v-row>
    <v-row v-if="showInviteDeletedAlert" justify="center">
      <v-col cols="12">
        <v-alert v-model="showInviteDeletedAlert" type="error" dismissible>{{ $tc('Invitation', 1) }} {{ $t('To') }} {{ deletedName }} {{ $t('Deleted').toLowerCase() }}</v-alert>
      </v-col>
    </v-row>
    <v-data-table :headers="headers" :items="invitations">

      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title class="mt-2"><h2>{{ $tc('Invitation', 2) }}</h2></v-toolbar-title>
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
                    <span class="validation-error" v-if="v$.editedInvitation.name.$error">{{ validationMessages(v$.viewingInvitation.name.$errors) }}</span>
                  </v-col>
                  <v-col cols="4">
                    <v-text-field hide-details="true" :label="$t('Email')" v-model="editedInvitation.email"></v-text-field>
                    <span class="validation-error" v-if="v$.editedInvitation.email.$error">{{ validationMessages(v$.viewingInvitation.email.$errors) }}</span>
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
          <!-- View Dialog -->
          <v-dialog v-model="viewDialog" max-width="800px">
            <v-card>
              <v-card-title class="modal-title pt-2">
                <v-icon v-if="!viewingInvitation.acceptedOn" color="white" class="pr-3">mdi-email</v-icon>
                <v-icon v-if="viewingInvitation.acceptedOn" color="white" class="pr-3">mdi-email-check</v-icon>
                <span>{{ $tc('Invitation', 1) }}</span>
              </v-card-title>
              <v-divider />
              <v-card-text>
                <v-container class="app-form">
                   <v-row>
                      <v-col cols="12">
                        <p class="large-font">
                          <strong>{{ viewingInvitation.name }}&nbsp;</strong>
                          <span>{{ $t('Was') }} {{ $t('Invited').toLowerCase() }}&nbsp;</span>
                          <span v-if="viewingInvitation.userType != userType.SystemUser" >{{ $t('ToBeA') }} {{ viewingInvitation.typeName.toLowerCase() }}&nbsp;</span>
                          <span>{{ $t('By') }} {{ viewingInvitation.invitedByUser.name }}&nbsp;</span>
                          <span>{{ $t('On') }} {{ $d(new Date(viewingInvitation.sentOn), 'long') }}.</span>
                        </p>
                        <p class="large-font" v-if="viewingInvitation.acceptedOn">
                          <span>{{ $t('They') }} {{ $t('Accepted').toLowerCase() }} {{ $t('On') }}&nbsp;</span>
                          <span>{{ $d(new Date(viewingInvitation.acceptedOn), 'long') }}&nbsp;</span>
                          <span>{{ $t('And') }} {{ $t('Created').toLowerCase() }} {{ $tc('User', 1).toLowerCase() }}&nbsp;</span>
                          <strong>{{ viewingInvitation.createdUser.name }}.</strong>
                        </p>
                      </v-col>
                   </v-row>
                   <v-row>
                      <v-col cols="12">
                        <p class="large-font label-paragraph"><label>{{ $t('Email') }}:&nbsp;</label><span>{{ viewingInvitation.email }}</span></p>
                        <p v-if="viewingInvitation.userType == userType.SystemUser" class="large-font label-paragraph"><label>{{ $tc('Role', 2) }}:&nbsp;</label><span>{{ getUserRoles(viewingInvitation.roles) }}</span></p>
                        <p v-if="viewingInvitation.artist.id > 0" class="large-font label-paragraph"><label>{{ $tc('Artist', 1) }}:&nbsp;</label><span>{{ viewingInvitation.artist.name }}</span></p>
                        <p v-if="viewingInvitation.recordLabel.id > 0" class="large-font label-paragraph"><label>{{ $tc('RecordLabel', 1) }}:&nbsp;</label><span>{{ viewingInvitation.recordLabel.name }}</span></p>
                        <p v-if="viewingInvitation.publisher.id > 0" class="large-font label-paragraph"><label>{{ $tc('Publisher', 1) }}:&nbsp;</label><span>{{ viewingInvitation.publisher.name }}</span></p>
                      </v-col>
                   </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn v-if="!viewingInvitation.acceptedOn" class="v-danger-button mr-4 rounded" @click="deleteInvitation">{{ $t('Delete') }}</v-btn>
                <v-btn v-if="!viewingInvitation.acceptedOn" class="v-button mr-4 rounded" @click="resendInvitation">{{ $t('Resend') }}</v-btn>
                <v-btn class="v-cancel-button mr-5 rounded" @click="closeView">{{ $t('Close') }}</v-btn>
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

      <template v-slot:[`item.sentOn`]="{ item }">
        <span v-if="item.sentOn">{{ $d(new Date(item.sentOn), 'long') }}</span>
      </template>

      <template v-slot:[`item.acceptedOn`]="{ item }">
        <span v-if="item.acceptedOn">{{ $d(new Date(item.acceptedOn), 'long') }}</span>
      </template>

      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small @click="viewInvitation(item)">mdi-eye</v-icon>
      </template>

    </v-data-table>
  </v-container>
</template>

<script>
import ErrorHandler from '../../models/local/ErrorHandler';
import ApiRequestHeaders from '../../models/local/ApiRequestHeaders';
import PublisherModel from '../../models/api/Publisher';
import ArtistModel from '../../models/api/Artist';
import RecordLabelModel from '../../models/api/RecordLabel';
import InvitationModel from '../../models/api/Invitation';
import UserType from '../../enums/UserType';
import UserTypes from '../../models/local/UserTypes';
import UserRoles from '../../models/local/UserRoles';
import SystemUserRoles from '../../enums/SystemUserRoles';
import useVuelidate from '@vuelidate/core';
import { required, email } from '@vuelidate/validators';
import { mapState } from "vuex";

export default {
    name: "Invitations",

    setup () {
      return { v$: useVuelidate() }
    },

    data: () => ({
        inviteDialog: false,
        viewDialog: false,
        invitations: [],
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
        showPublisherFields: false,
        showLabelFields: false,
        showUserFields: false,
        viewingInvitation: {
           uuid: '',
           name: '',
           email: '',
           userType: 0,
           typeName: '',
           roles: 0,
           invitedByUserId: -1,
           invitedByUser: {
             id: -1,
             name: '',
           },
           sentOn: null,
           acceptedOn: null,
           createdUser: {
             id: -1,
             name: '',
             userType: ''
           },
           publisher: {
             id: -1,
             name: '',
             email: '',
           },
           recordLabel: {
             id: -1,
             name: '',
             email: '',
           },
           artist: {
             id: -1,
             name: ''
           }
        },
        defaultViewingInvitation: {
           uuid: '',
           name: '',
           email: '',
           userType: 0,
           typeName: '',
           roles: 0,
           invitedByUserId: -1,
           invitedByUser: {
             id: -1,
             name: '',
           },
           sentOn: null,
           acceptedOn: null,
           createdUser: {
             id: -1,
             name: '',
             userType: ''
           },
           publisher: {
             id: -1,
             name: '',
             email: '',
           },
           recordLabel: {
             id: -1,
             name: '',
             email: '',
           },
           artist: {
             id: -1,
             name: ''
           }
        },
        editedInvitation: {
          uuid: '',
          invitedByUserId: -1,
          name: '',
          email: '',
          userType: 0,
          roles: 0,
          publisher: null,
          recordLabel: null,
          artist: null
        },
        defaultInvitation: {
          invitedByUserId: -1,
          name: '',
          email: '',
          userType: 0,
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
          userType: 0,
          roles: 0,
          publisher: null,
          recordLabel: null,
          artist: null
        },
        showInvitedUserAlert: false,
        showInviteResentAlert: false,
        showInviteDeletedAlert: false,
        resentToEmail: null,
        deletedName: null,
        showInviteRolesValidation: false,
        showInvitePublisherValidation: false,
        showInviteLabelValidation: false,
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
            { text: this.$t('Invited'), value: "name" },
            { text: this.$tc('Type', 1), value: "typeName" },
            { text: this.$tc('Role', 2), value: "roles" },
            { text: this.$t('Invited') + ' ' + this.$t('By'), value: "invitedByUser.name" },
            { text: this.$t('Invited') + ' ' + this.$t('On'), value: "sentOn" },
            { text: this.$t('Accepted') + ' ' + this.$t('On'), value: "acceptedOn" },
            { text: '', value: 'actions', sortable: false, align: "center", width: "50px" },
            ];}
    },

    validations() {
      return {
        selectedUserType: { required },
        editedInvitation: {
          name: { required },
          email: { required, email },
        },
      }
    },

    watch: {
        inviteDialog(val) {
          val || this.closeInvite();
        },

        viewDialog(val) {
            val || this.closeView();
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
          await this.loadInvitations();
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

        async loadInvitations() {
          this.invitations = await InvitationModel.config(this.RequestHeaders).all()
            .catch(error => this.handleError(error));
          this.invitations.forEach(invitation => {
            invitation.typeName = this.getUserType(invitation.userType).name;
          });   
        },

        async inviteUser() {
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
          try {
            await this.v$.$validate();
            let formIsValid = !this.v$.editedInvitation.$error;
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
              this.editedInvitation.userType = userType;
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
              
              this.addedInvitation = Object.assign({}, this.editedInvitation);
              const invitationModel = new InvitationModel(this.editedInvitation);
              invitationModel.config(this.RequestHeaders).save()
                .then(() => {
                  this.loadInvitations();
                  this.showInviteResentAlert = false;
                  this.showInviteDeletedAlert = false;
                  this.showInvitedUserAlert = true;
                })
                .catch(error => this.handleError(error));
            }
            this.inviteDialog = false;
          }
          catch (error) {
            this.handleError(error);
          }
        },

        getUserType(typeValue) {
          console.log(typeValue);
          let userType = null;
          this.userTypes.forEach(type => {
            if (type.value == typeValue)
            userType = type;
          });
          return userType;
        },

        getUserRoles(rolesValue) {
          let userRoles = '';
          for (let i = 0; i < UserRoles.length; i++) {
            if (rolesValue & UserRoles[i].value)
              userRoles = userRoles + UserRoles[i].name + ", ";
          }

          if (userRoles.length < 2)
            return userRoles;

          return userRoles.substring(0, userRoles.length - 2);
        },

        userInRole(user, role) {
          return (user.roles & role) == role;
        },

        closeView() {
          this.viewDialog = false;
          this.$nextTick(() => {
            this.viewingInvitation = Object.assign({}, this.defaultViewingInvitation);
          });
        },

        async viewInvitation(invitation) {
          let emptyViewingInvitation = JSON.parse(JSON.stringify(this.defaultViewingInvitation));
          this.viewingInvitation = Object.assign(emptyViewingInvitation, invitation);
          if (!this.viewingInvitation.publisher)
            this.viewingInvitation.publisher = Object.assign({}, this.defaultViewingInvitation.publisher);
          if (!this.viewingInvitation.recordLabel)
            this.viewingInvitation.recordLabel = Object.assign({}, this.defaultViewingInvitation.recordLabel);
          if (!this.viewingInvitation.artist)
            this.viewingInvitation.artist = Object.assign({}, this.defaultViewingInvitation.artist);

          this.viewDialog = true;
        },

        async resendInvitation() {
          try {
            const invitationModel = new InvitationModel(this.viewingInvitation);
            let requestConfig = this.RequestHeaders;
            requestConfig.method = 'POST';
            invitationModel.config(requestConfig).save()
              .then(() => {
                this.resentToEmail = this.viewingInvitation.email;
                this.showInviteResentAlert = true;
                this.showInviteDeletedAlert = false;
                this.showInvitedUserAlert = false;
                this.closeView();
              })
              .catch(error => this.handleError(error));
          }
          catch (error) {
            this.handleError(error);
          }
        },

        async deleteInvitation() {
          try {
            const invitationModel = new InvitationModel(this.viewingInvitation);
            invitationModel.config(this.RequestHeaders).delete()
              .then(() => {
                this.deletedName = this.viewingInvitation.name;
                this.showInviteDeletedAlert = true;
                this.showInviteResentAlert = false;
                this.showInvitedUserAlert = false;
                this.loadInvitations();
                this.closeView();
              })
              .catch(error => this.handleError(error));
          }
          catch (error) {
            this.handleError(error);
          }
        },

        handleError(error) {
            this.error = new ErrorHandler(error).handleError(this.$router);
        },
    },

    async mounted() {
        this.initialize();
    }
}
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
  .label-paragraph {
    margin-bottom: 6px !important;
  }
</style>
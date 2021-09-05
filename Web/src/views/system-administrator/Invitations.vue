<template>
    
  <v-container fluid class="down-top-padding pt-0">
    <v-row v-if="error" justify="center">
      <v-col cols="12">
        <v-alert type="error">{{ error }}</v-alert>
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
          <!-- Dialog -->
          <v-dialog v-model="dialog" max-width="800px">
            <v-card>
              <v-card-title class="modal-title pt-2">
                <v-icon v-if="!selectedInvitation.acceptedOn" color="white" class="pr-3">mdi-email</v-icon>
                <v-icon v-if="selectedInvitation.acceptedOn" color="white" class="pr-3">mdi-email-check</v-icon>
                <span>{{ $tc('Invitation', 1) }}</span>
              </v-card-title>
              <v-divider />
              <v-card-text>
                <v-container class="app-form">
                   <v-row>
                      <v-col cols="12">
                        <p class="large-font">
                          <strong>{{ selectedInvitation.name }}&nbsp;</strong>
                          <span>{{ $t('Was') }} {{ $t('Invited').toLowerCase() }}&nbsp;</span>
                          <span v-if="selectedInvitation.type != userType.SystemUser" >{{ $t('ToBeA') }} {{ selectedInvitation.typeName.toLowerCase() }}&nbsp;</span>
                          <span>{{ $t('By') }} {{ selectedInvitation.invitedByUser.name }}&nbsp;</span>
                          <span>{{ $t('On') }} {{ $d(new Date(selectedInvitation.sentOn), 'long') }}.</span>
                        </p>
                        <p class="large-font" v-if="selectedInvitation.acceptedOn">
                          <span>{{ $t('They') }} {{ $t('Accepted').toLowerCase() }} {{ $t('On') }}&nbsp;</span>
                          <span>{{ $d(new Date(selectedInvitation.acceptedOn), 'long') }}&nbsp;</span>
                          <span>{{ $t('And') }} {{ $t('Created').toLowerCase() }} {{ $tc('User', 1).toLowerCase() }}&nbsp;</span>
                          <strong>{{ selectedInvitation.createdUser.name }}.</strong>
                        </p>
                      </v-col>
                   </v-row>
                   <v-row>
                      <v-col cols="12">
                        <p class="large-font label-paragraph"><label>{{ $t('Email') }}:&nbsp;</label><span>{{ selectedInvitation.email }}</span></p>
                        <p v-if="selectedInvitation.type == userType.SystemUser" class="large-font label-paragraph"><label>{{ $tc('Role', 2) }}:&nbsp;</label><span>{{ getUserRoles(selectedInvitation.roles) }}</span></p>
                        <p v-if="selectedInvitation.artist.id > 0" class="large-font label-paragraph"><label>{{ $tc('Artist', 1) }}:&nbsp;</label><span>{{ selectedInvitation.artist.name }}</span></p>
                        <p v-if="selectedInvitation.recordLabel.id > 0" class="large-font label-paragraph"><label>{{ $tc('RecordLabel', 1) }}:&nbsp;</label><span>{{ selectedInvitation.recordLabel.name }}</span></p>
                        <p v-if="selectedInvitation.publisher.id > 0" class="large-font label-paragraph"><label>{{ $tc('Publisher', 1) }}:&nbsp;</label><span>{{ selectedInvitation.publisher.name }}</span></p>
                      </v-col>
                   </v-row>
                </v-container>
              </v-card-text>
              <v-card-actions class="pb-6">
                <v-spacer></v-spacer>
                <v-btn v-if="!selectedInvitation.acceptedOn" class="v-danger-button mr-4 rounded" @click="deleteInvitation">{{ $t('Delete') }}</v-btn>
                <v-btn v-if="!selectedInvitation.acceptedOn" class="v-button mr-4 rounded" @click="resendInvitation">{{ $t('Resend') }}</v-btn>
                <v-btn class="v-cancel-button mr-5 rounded" @click="close">{{ $t('Close') }}</v-btn>
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
import InvitationModel from '../../models/api/Invitation';
import UserType from '../../enums/UserType';
import UserTypes from '../../models/local/UserTypes';
import UserRoles from '../../models/local/UserRoles';
import SystemUserRoles from '../../enums/SystemUserRoles';
import { mapState } from "vuex";

export default {
    name: "Invitations",

    data: () => ({
        dialog: false,
        invitations: [],
        userType: UserType,
        userTypes: [],
        defaultInvitation: {
           uuid: '',
           name: '',
           email: '',
           type: 0,
           typeName: '',
           roles: 0,
           invitedByUser: {
             id: -1,
             name: '',
           },
           sentOn: null,
           acceptedOn: null,
           createdUser: {
             id: -1,
             name: '',
             type: ''
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
        selectedInvitation: {
           uuid: '',
           name: '',
           email: '',
           type: 0,
           typeName: '',
           roles: 0,
           invitedByUser: {
             id: -1,
             name: '',
           },
           sentOn: null,
           acceptedOn: null,
           createdUser: {
             id: -1,
             name: '',
             type: ''
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
        userRoles: [],
        systemUserRoles: SystemUserRoles,
        showInviteResentAlert: false,
        showInviteDeletedAlert: false,
        resentToEmail: null,
        deletedName: null,
        error: null
    }),

    computed: {
        ...mapState(["Authentication"]),
        RequestHeaders: {
            get () { return new ApiRequestHeaders(this.Authentication.authenticationToken); }
        },

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

    watch: {
        dialog(val) {
            val || this.close();
        },
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

        async loadInvitations() {
          this.invitations = await InvitationModel.config(this.RequestHeaders).all()
            .catch(error => this.handleError(error));
          this.invitations.forEach(invitation => {
            invitation.typeName = this.getUserType(invitation.type).name;
          });   
        },

        getUserType(typeValue) {
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

        close() {
          this.dialog = false;
          this.$nextTick(() => {
            this.selectedInvitation = Object.assign({}, this.defaultInvitation);
          });
        },

        async viewInvitation(invitation) {
          let emptyInvitation = JSON.parse(JSON.stringify(this.defaultInvitation));
          this.selectedInvitation = Object.assign(emptyInvitation, invitation);
          if (!this.selectedInvitation.publisher)
            this.selectedInvitation.publisher = Object.assign({}, this.defaultInvitation.publisher);
          if (!this.selectedInvitation.recordLabel)
            this.selectedInvitation.recordLabel = Object.assign({}, this.defaultInvitation.recordLabel);
          if (!this.selectedInvitation.artist)
            this.selectedInvitation.artist = Object.assign({}, this.defaultInvitation.artist);
          this.dialog = true;
        },

        async resendInvitation() {
          const invitationModel = new InvitationModel(this.selectedInvitation);
          let requestConfig = this.RequestHeaders;
          requestConfig.method = 'POST';
          invitationModel.config(requestConfig).save()
            .then(() => {
              this.resentToEmail = this.selectedInvitation.email;
              this.showInviteResentAlert = true;
              this.showInviteDeletedAlert = false;
              this.close();
            })
            .catch(error => this.handleError(error));
        },

        async deleteInvitation() {
          const invitationModel = new InvitationModel(this.selectedInvitation);
          invitationModel.config(this.RequestHeaders).delete()
            .then(() => {
              this.deletedName = this.selectedInvitation.name;
              this.showInviteDeletedAlert = true;
              this.showInviteResentAlert = false;
              this.loadInvitations();
              this.close();
            })
            .catch(error => this.handleError(error));
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
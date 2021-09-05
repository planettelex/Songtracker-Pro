<template>
  <v-container id="join" class="justify-center" tag="section">
    <v-row v-if="error" justify="center">
      <v-col cols="12" sm="10" md="7" lg="6" xl="4">
        <v-alert type="error">{{ error }}</v-alert>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col cols="12" sm="12" md="11" lg="10" xl="9">
        
        <v-card class="join-card elevation-4">
          <v-container>
            <v-row>
              <v-col class="join-logo-icon d-flex justify-center pl-0" cols="1">
                <img src="../assets/images/logo-light.svg"/> 
              </v-col>
              <v-col cols="10">
                <div class="join-header">
                  <h2 class="join-app-name">{{ applicationInfo.name }}</h2>
                  <span style="display:none;">v {{ applicationInfo.version }}</span>
                  <em class="join-tagline">{{ applicationInfo.tagline }}</em>
                </div>
              </v-col>
            </v-row>
            <v-row>
              <v-col class="join-form pr-8" cols="12">
                <v-container>
                  <v-row v-if="!hasInvitationCode">
                    <v-col cols="12" class="pl-6">
                      <strong>{{ $t('InvitationOnly') }}</strong>
                      <p>{{ $t('InvitationInstructions') }}</p>
                    </v-col>
                  </v-row>
                  <v-row v-if="hasInvitationCode && !invitationCodeValid">
                    <v-col cols="12" class="pl-6">
                      <strong>{{ $t('InvitationInvalid') }}</strong>
                      <p>{{ $t('InvitationResend') }}</p>
                    </v-col>
                  </v-row>
                  <v-row v-if="hasInvitationCode && invitationCodeValid">
                    <v-col cols="12">
                      <v-form>
                        <v-container>
                          <v-row>
                            <v-col cols="12">
                              <p>{{ $t('Hello') }} {{ invitation.name }},</p>
                              <p>
                                <span v-if="isSystemAdministrator">
                                  {{ $t('InvitedToJoin') }} <b>{{ applicationInfo.name }}</b> {{ $tc('AsA', 1) }} <em>{{ userTypeName }}</em>.<br/>
                                </span>
                                <span v-if="isPublisherAdministrator">
                                  {{ $t('InvitedToJoin') }} <b>{{ applicationInfo.name }}</b> {{ $tc('AsA', 2) }} {{ $t('AdministratorFor') }} <em>{{ invitation.publisher.name }}</em>.<br/>
                                </span>
                                <span v-if="isLabelAdministrator">
                                  {{ $t('InvitedToJoin') }} <b>{{ applicationInfo.name }}</b> {{ $tc('AsA', 2) }} {{ $t('AdministratorFor') }} <em>{{ invitation.recordLabel.name }}</em>.<br/>
                                </span>
                                <span v-if="isSystemUser">
                                  {{ $t('InvitedToJoin') }} <b>{{ applicationInfo.name }}</b> {{ $t('By') }} {{ invitation.invitedByUser.name }}.<br/>
                                </span>
                                {{ $t('FillFormToComplete') }}
                              </p>
                            </v-col>
                            <v-col cols="4">
                              <v-text-field hide-details="true" :label="$t('Email')" v-model="invitation.email" disabled></v-text-field>
                            </v-col>
                            <v-col cols="2">
                              <v-text-field hide-details="true" :label="$t('First')" v-model="invitedUser.person.firstName"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.firstName.$error">{{ validationMessages(v$.invitedUser.person.firstName.$errors) }}</span>
                            </v-col>
                            <v-col cols="2">
                              <v-text-field hide-details="true" :label="$t('Middle')" v-model="invitedUser.person.middleName"></v-text-field>
                            </v-col>
                            <v-col cols="2">
                              <v-text-field hide-details="true" :label="$t('Last')" v-model="invitedUser.person.lastName"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.lastName.$error">{{ validationMessages(v$.invitedUser.person.lastName.$errors) }}</span>
                            </v-col>
                            <v-col cols="2">
                              <v-text-field hide-details="true" :label="$t('Suffix')" v-model="invitedUser.person.nameSuffix"></v-text-field>
                            </v-col>
                          </v-row>
                          <v-row>
                            <v-col cols="4">
                              <v-text-field hide-details="true" :label="$t('Address')" v-model="invitedUser.person.address.street"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.address.street.$error">{{ validationMessages(v$.invitedUser.person.address.street.$errors) }}</span>
                            </v-col>
                            <v-col cols="2">
                              <v-text-field hide-details="true" :label="$t('City')" v-model="invitedUser.person.address.city"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.address.city.$error">{{ validationMessages(v$.invitedUser.person.address.city.$errors) }}</span>
                            </v-col>
                            <v-col cols="2" >
                              <v-text-field hide-details="true" :label="$t('PostalCode')" v-model="invitedUser.person.address.postalCode"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.address.postalCode.$error">{{ validationMessages(v$.invitedUser.person.address.postalCode.$errors) }}</span>
                            </v-col>
                            <v-col cols="2">
                              <v-select hide-details="true" :label="$t('Country')" :items="countries" v-model="selectedCountry" item-text="name" item-value="isoCode" return-object></v-select>
                              <span class="validation-error" v-if="v$.selectedCountry.isoCode.$error">{{ $t('ValueIsRequired') }}</span>
                            </v-col>
                            <v-col cols="2">
                              <v-select hide-details="true" :label="$t('CountryRegion')" :items="countryRegions" v-model="selectedCountryRegion" item-text="name" item-value="code" return-object></v-select>
                              <span class="validation-error" v-if="v$.selectedCountryRegion.$error">{{ $t('ValueIsRequired') }}</span>
                            </v-col>
                          </v-row>
                          <v-row>
                            <v-col cols="3">
                              <v-text-field hide-details="true" :label="$t('Phone')" v-model="invitedUser.person.phone"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.person.phone.$error">{{ validationMessages(v$.invitedUser.person.phone.$errors) }}</span>
                            </v-col>
                            <v-col cols="2" v-if="isSystemUser">
                              <v-text-field hide-details="true" :label="$t('SSN')" v-model="invitedUser.socialSecurityNumber"></v-text-field>
                              <span class="validation-error" v-if="v$.invitedUser.socialSecurityNumber.$error">{{ validationMessages(v$.invitedUser.socialSecurityNumber.$errors) }}</span>
                            </v-col>
                            <v-col cols="2" v-if="isSystemUser">
                              <v-select hide-details="true" :label="$t('Pro')" :items="performingRightsOrganizations" v-model="selectedPerformingRightsOrganization" item-text="name" item-value="id" return-object></v-select>
                            </v-col>
                            <v-col cols="2" v-if="isSystemUser">
                              <v-text-field hide-details="true" :label="$t('ProId')" v-model="invitedUser.performingRightsOrganizationMemberNumber"></v-text-field>
                            </v-col>
                            <v-col cols="3" v-if="isSystemUser">
                              <v-text-field hide-details="true" :label="$t('SoundExchangeId')" v-model="invitedUser.soundExchangeAccountNumber"></v-text-field>
                            </v-col>
                          </v-row>
                          <v-row>
                            <v-spacer></v-spacer>
                            <v-col class="mr-5" cols="1">
                              <v-btn class="v-button mr-4 rounded" @click="join">{{ $t('Join') }}</v-btn>
                            </v-col>
                          </v-row>
                        </v-container>
                      </v-form>
                    </v-col>
                  </v-row>
                </v-container>
              </v-col>
            </v-row>
          </v-container>
        </v-card>
        
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import ErrorHandler from '../models/local/ErrorHandler';
import ApiRequestHeaders from '../models/local/ApiRequestHeaders';
import ApplicationModel from '../models/api/Application';
import CountryModel from '../models/api/Country';
import CountryRegions from '../resources/countryRegions';
import InvitationModel from '../models/api/Invitation';
import UserType from '../enums/UserType';
import UserTypes from '../models/local/UserTypes';
import useVuelidate from '@vuelidate/core';
import { required, minLength } from '@vuelidate/validators';
import { mapState } from "vuex";
import appConfig from '../appConfig';

export default {
  name: "Join",

  setup () {
    return { v$: useVuelidate() }
  },

  data: () => ({
    applicationInfo: {
      name: null,
      tagline: null,
      version: null
    },
    countries: [],
    selectedCountry: null,
    countryRegions: [],
    selectedCountryRegion: null,
    hasInvitationCode: false,
    invitationCode: null,
    invitationCodeValid: false,
    invitation: {
      invitedByUserId: -1,
      invitedByUser: {
        name: ''
      },
      name: '',
      email: '',
      type: 0,
      roles: 0,
      createdUser : null, 
      publisher: null,
      recordLabel: null,
      artist: null
    },
    invitedUser: {
      authenticationId: '',
      socialSecurityNumber: null,
      person: {
        firstName: '',
        middleName: null,
        lastName: '',
        nameSuffix: null,
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
        }
      },
      performingRightsOrganization: null,
      performingRightsOrganizationMemberNumber: null,
      soundExchangeAccountNumber: null
    },
    userTypes: [],
    userTypeName: '',
    isSystemUser: false,
    isSystemAdministrator: false,
    isPublisherAdministrator: false,
    isLabelAdministrator: false,
    performingRightsOrganizations: [],
    selectedPerformingRightsOrganization: null,
    error: null
  }),

  computed: {
    ...mapState(["Application"]),
    Application: {
      get() { return this.$store.state.Application; },
      set(val) { this.$store.commit("SET_APPLICATION", val); }
    },

    UnauthenticatedRequestHeaders: {
      get () { return new ApiRequestHeaders(); }
    }
  },

  validations () {
    return {
      selectedCountry: {
        isoCode: { required }
      },
      selectedCountryRegion: { required },
      invitedUser: {
        person: {
          firstName: { required },
          lastName: { required },
          phone: { minLengthValue: minLength(10) },
          address: {
            street: { required },
            city: { required },
            postalCode: { required, minLengthValue: minLength(5) }
          }
        }
      }
    }
  },

  watch: {
    hasInvitationCode(val) {
      if (val)
        this.validateInvitationCode();
    },

    selectedCountry(val) {
      if (val)
        this.loadCountryRegions();
    }
  },

  methods: {
    async validateInvitationCode() {
      InvitationModel.config(this.UnauthenticatedRequestHeaders).find(this.invitationCode)
        .then(response => {
          this.invitation = Object.assign({}, response);
          this.invitationCodeValid = response.uuid.toLowerCase() == this.invitationCode.toLowerCase();
          if (this.invitationCodeValid) {
            this.userTypes.forEach(userType => {
              if (userType.value == this.invitation.type) {
                this.userTypeName = userType.name;
                this.isSystemUser = userType.value == UserType.SystemUser;
                this.isPublisherAdministrator = userType.value == UserType.PublisherAdministrator;
                this.isLabelAdministrator = userType.value == UserType.LabelAdministrator;
                this.isSystemAdministrator = userType.value == UserType.SystemAdministrator;
              }
              this.loadCountries();
            });
          }
        })
        .catch(error => this.handleError(error));
    },

    async loadCountries() {
      this.countries = await CountryModel.config(this.UnauthenticatedRequestHeaders).all()
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
    
    async join() {
      try {
        const formIsValid = await this.v$.$validate();
        if (!formIsValid) 
          return;

        this.invitation.createdUser = this.invitedUser;
        this.invitation.createdUser.authenticationId = this.invitation.email;
        this.invitation.createdUser.person.email = this.invitation.email;
        this.invitation.createdUser.person.address.country = this.selectedCountry;
        if (this.selectedCountryRegion)
          this.invitation.createdUser.person.address.region = this.selectedCountryRegion.code;

        const invitationModel = new InvitationModel(this.invitation);
        invitationModel.config(this.UnauthenticatedRequestHeaders).save()
          .then (() => {
            this.$router.push("/login?logout=true");
          })
          .catch(error => this.handleError(error));   
      } 
      catch (error) {
        this.handleError(error);
      }
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
    }
  },

  async mounted() {
    try {
      if (this.Application == null || this.Application.name == null) {
        this.Application = await ApplicationModel.first().catch(error => this.handleError(error));
      }
      Object.assign(this.applicationInfo, this.Application);

      document.title = this.applicationInfo.name + ' - ' + this.applicationInfo.tagline;
      this.userTypes = UserTypes;
      this.userTypes.forEach(userType => {
        userType.name = this.$t(userType.key);
      });
      if (this.$route.query.invitation) {
        this.invitationCode = this.$route.query.invitation;
        this.hasInvitationCode = true;
      }
    } 
    catch (error) {
        this.handleError(error);
    }
  }
};
</script>

<style lang="scss">
  .join-app-name {
    color: $light-primary;
  }
  .join-tagline {
    color: $gray-250;
  }
  .theme--light.v-application {
    background-image: url('../assets/images/concert.jpg');
    background-repeat: no-repeat;
    background-size: cover;
  }
  #join .theme--light.v-card {
    background-color: transparent;
    background-image: url('../assets/images/dark-transparent.png');
    background-repeat: repeat;
    border-top-left-radius: $border-radius-root;
    border-top-right-radius: $border-radius-root;
  }
  .join-card {
    padding-left: 25px;
  }
  .join-logo-icon {
    height: 85px;
  }
  .join-form > .container {
    background-color: transparent;
    background-image: url('../assets/images/light-transparent.png');
    background-repeat: repeat;
    border-radius: $border-radius-root;
    padding-left: 15px;
  }
</style>

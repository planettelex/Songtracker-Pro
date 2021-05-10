import Vue from 'vue';
import Vuex from 'vuex';
import createPersistedState from "vuex-persistedstate";

Vue.use(Vuex);

export default new Vuex.Store({
    plugins: [createPersistedState()],

    state: {
        ProfileImage: null,
        SidebarDrawer: null,
        SidebarColor: '#1d2228',
        User: null,
        UserAuthenticated: false
    },

    mutations: {
        SET_PROFILE_IMAGE(state, payload) {
            state.ProfileImage = payload;
        },
        SET_SIDEBAR_DRAWER(state, payload) {
            state.SidebarDrawer = payload;
        },
        SET_SIDEBAR_COLOR(state, payload) {
            state.SidebarColor = payload;
        },
        SET_USER(state, payload) {
            state.User = payload;
        },
        SET_USER_AUTHENTICATED(state, payload) {
            state.UserAuthenticated = payload;
        }
    },

    actions: {
    },

    getters: {
    }
})
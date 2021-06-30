import Vue from 'vue';
import Vuex from 'vuex';
import VuexPersistence from 'vuex-persist';

Vue.use(Vuex);

export default new Vuex.Store({
    plugins: [new VuexPersistence().plugin],

    state: {
        Application: null,
        Authentication: null,
        ProfileImage: null,
        SidebarDrawer: null,
        SidebarColor: '#1d2228',
        User: null,
    },

    mutations: {
        SET_APPLICATION(state, payload) {
            state.Application = payload;
        },
        SET_AUTHENTICATION(state, payload) {
            state.Authentication = payload;
        },
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
        }
    },

    actions: {
    },

    getters: {
    }
})
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        Login: null,
        SidebarDrawer: null,
        SidebarColor: '#1d2228',
        User: null
    },
    mutations: {
        SET_LOGIN(state, payload) {
            state.Login = payload
        },
        SET_SIDEBAR_DRAWER(state, payload) {
            state.SidebarDrawer = payload
        },
        SET_SIDEBAR_COLOR(state, payload) {
            state.SidebarColor = payload
        },
        SET_USER(state, payload) {
            state.User = payload
        }
    },
    actions: {
    },
    getters: {
    }
})
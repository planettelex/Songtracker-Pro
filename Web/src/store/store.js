import Vue from 'vue'
import Vuex from 'vuex'
import UserType from "../enums/user-type";
Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        SidebarDrawer: null,
        SidebarColor: '#1d2228',
        UserType: UserType.Unsassigned
    },
    mutations: {
        SET_SIDEBAR_DRAWER(state, payload) {
            state.SidebarDrawer = payload
        },
        SET_SIDEBAR_COLOR(state, payload) {
            state.SidebarColor = payload
        },
        SET_USER_TYPE(state, payload) {
            state.UserType = payload
        }
    },
    actions: {
    },
    getters: {
    }
})
import Vue from "vue";
import Router from "vue-router";
import goTo from "vuetify/es5/services/goto";
import store from "./store/store";
import UserType from "./enums/UserType"

Vue.use(Router);

const router = new Router({
  mode: "history",
  base: process.env.BASE_URL,
  // This is for the scroll top when click on any router link
  scrollBehavior: (to, from, savedPosition) => {
    let scrollTo = 0;

    if (to.hash) {
      scrollTo = to.hash;
    } else if (savedPosition) {
      scrollTo = savedPosition.y;
    }

    return goTo(scrollTo);
  },
  // This is for the scroll top when click on any router link
  routes: [
    {
      path: "/",
      redirect: "login",
      component: () => import("@/layouts/full-layout/Layout"),
      children: [
        // System Administrator
        {
          name: "SystemInformation",
          path: "system-information",
          component: () => import("@/views/system-administrator/SystemInformation"),
          meta: { titleKey: "SystemInformation", userType: UserType.SystemAdministrator }
        },
        {
          name: "PublishingCompanies",
          path: "publishing-companies",
          component: () => import("@/views/system-administrator/PublishingCompanies"),
          meta: { titleKey: "PublishingCompany", userType: UserType.SystemAdministrator }
        },
        {
          name: "RecordLabels",
          path: "record-labels",
          component: () => import("@/views/system-administrator/RecordLabels"),
          meta: { titleKey: "RecordLabel", userType: UserType.SystemAdministrator }
        },
        {
          name: "Platforms",
          path: "platforms",
          component: () => import("@/views/system-administrator/Platforms"),
          meta: { titleKey: "Platform", userType: UserType.SystemAdministrator }
        },
        {
          name: "Artists",
          path: "artists",
          component: () => import("@/views/system-administrator/Artists"),
          meta: { titleKey: "Artist", userType: UserType.SystemAdministrator }
        },
        {
          name: "Users",
          path: "users",
          component: () => import("@/views/system-administrator/Users"),
          meta: { titleKey: "User", userType: UserType.SystemAdministrator }
        },
        // Label Administrator
        {
          name: "LabelEarnings",
          path: "label-earnings",
          component: () => import("@/views/label-administrator/LabelEarnings"),
          meta: { titleKey: "Earnings", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelDocuments",
          path: "label-documents",
          component: () => import("@/views/label-administrator/LabelDocuments"),
          meta: { titleKey: "Document", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelRecordings",
          path: "label-recordings",
          component: () => import("@/views/label-administrator/LabelRecordings"),
          meta: { titleKey: "Recording", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelReleases",
          path: "label-releases",
          component: () => import("@/views/label-administrator/LabelReleases"),
          meta: { titleKey: "Release", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelArtists",
          path: "label-artists",
          component: () => import("@/views/label-administrator/LabelArtists"),
          meta: { titleKey: "Artist", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelUsers",
          path: "label-users",
          component: () => import("@/views/label-administrator/LabelUsers"),
          meta: { titleKey: "User", userType: UserType.LabelAdministrator }
        },
        {
          name: "LabelInformation",
          path: "label-information",
          component: () => import("@/views/label-administrator/LabelInformation"),
          meta: { titleKey: "Information", userType: UserType.LabelAdministrator }
        },
        // Publisher Administrator
        {
          name: "PublisherEarnings",
          path: "publisher-earnings",
          component: () => import("@/views/publisher-administrator/PublisherEarnings"),
          meta: { titleKey: "Earnings", userType: UserType.PublisherAdministrator }
        },
        {
          name: "PublisherDocuments",
          path: "publisher-documents",
          component: () => import("@/views/publisher-administrator/PublisherDocuments"),
          meta: { titleKey: "Document", userType: UserType.PublisherAdministrator }
        },
        {
          name: "PublisherCompositions",
          path: "publisher-compositions",
          component: () => import("@/views/publisher-administrator/PublisherCompositions"),
          meta: { titleKey: "Composition", userType: UserType.PublisherAdministrator }
        },
        {
          name: "PublisherUsers",
          path: "publisher-users",
          component: () => import("@/views/publisher-administrator/PublisherUsers"),
          meta: { titleKey: "User", userType: UserType.PublisherAdministrator }
        },
        {
          name: "PublisherInformation",
          path: "publisher-information",
          component: () => import("@/views/publisher-administrator/PublisherInformation"),
          meta: { titleKey: "Information", userType: UserType.PublisherAdministrator }
        },
        // System User
        {
          name: "UserProfile",
          path: "my-profile",
          component: () => import("@/views/system-user/UserProfile"),
          meta: { titleKey: "MyProfile", userType: UserType.Unsassigned }
        },
        {
          name: "UserEarnings",
          path: "my-earnings",
          component: () => import("@/views/system-user/UserEarnings"),
          meta: { titleKey: "MyEarnings", userType: UserType.SystemUser }
        },
        {
          name: "UserDocuments",
          path: "my-documents",
          component: () => import("@/views/system-user/UserDocuments"),
          meta: { titleKey: "MyDocuments", userType: UserType.SystemUser }
        },
        {
          name: "UserCompositions",
          path: "my-compositions",
          component: () => import("@/views/system-user/UserCompositions"),
          meta: { titleKey: "MyCompositions", userType: UserType.SystemUser }
        },
        {
          name: "UserRecordings",
          path: "my-recordings",
          component: () => import("@/views/system-user/UserRecordings"),
          meta: { titleKey: "MyRecordings", userType: UserType.SystemUser }
        },
        {
          name: "UserReleases",
          path: "my-releases",
          component: () => import("@/views/system-user/UserReleases"),
          meta: { titleKey: "MyReleases", userType: UserType.SystemUser }
        },
        // Error
        {
          name: "403",
          path: "403",
          component: () => import("@/views/FourOhThree"),
          meta: { unauthenticatedOk: true, title: "403", userType: UserType.Unsassigned  }
        },
      ],
    },

    {
      path: "/",
      component: () => import("@/layouts/blank-layout/Blanklayout"),
      children: [
        {
          name: "Login",
          path: "login",
          component: () => import("@/views/Login"),
          meta: { unauthenticatedOk: true, titleKey: "Login", userType: UserType.Unsassigned }
        },
        {
          name: "Join",
          path: "join",
          component: () => import("@/views/Join"),
          meta: { unauthenticatedOk: true, titleKey: "Join", userType: UserType.Unsassigned }
        },
        {
          name: "404",
          path: "*",
          component: () => import("@/views/FourOhFour"),
          meta: { unauthenticatedOk: true, title: "404", userType: UserType.Unsassigned  }
        },
      ],
    },
  ],
});

import NProgress from "nprogress";

router.beforeResolve((to, from, next) => {
  // If this isn't an initial page load.
  if (to.name) {
    // Start the route progress bar.
    NProgress.start(800);
  }
  next();
});

router.beforeEach((to, from, next) => {
  if (to.meta.unauthenticatedOk) {
    next();
  }
  else if (store.state.Login !== null) {
    let now = new Date(Date.now());
    let tokenExpiration = new Date(store.state.Login.tokenExpiration);
    let userType = store.state.User ? store.state.User.type : UserType.Unsassigned;
    if (tokenExpiration < now) 
      next("/login?logout=true");
    else if (to.meta.userType != UserType.Unsassigned && to.meta.userType != userType)
      next("/403");
    else {
      next();
    }
  }
  else {
    next("/login");
  }
});

router.afterEach(() => {
  // Complete the animation of the route progress bar.
  NProgress.done();
});

export default router;

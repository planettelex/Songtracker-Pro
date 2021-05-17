import Vue from "vue";
import Router from "vue-router";
import goTo from "vuetify/es5/services/goto";
import store from "./store/store";

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
          meta: { title: "System Information" }
        },
        {
          name: "PublishingCompanies",
          path: "publishing-companies",
          component: () => import("@/views/system-administrator/PublishingCompanies"),
          meta: { title: "Publishing Companies" }
        },
        {
          name: "RecordLabels",
          path: "record-labels",
          component: () => import("@/views/system-administrator/RecordLabels"),
          meta: { title: "Record Labels" }
        },
        {
          name: "Platforms",
          path: "platforms",
          component: () => import("@/views/system-administrator/Platforms"),
          meta: { title: "Platforms" }
        },
        {
          name: "Artists",
          path: "artists",
          component: () => import("@/views/system-administrator/Artists"),
          meta: { title: "Artists" }
        },
        {
          name: "Users",
          path: "users",
          component: () => import("@/views/system-administrator/Users"),
          meta: { title: "Users" }
        },
        // Label Administrator
        {
          name: "LabelEarnings",
          path: "label-earnings",
          component: () => import("@/views/label-administrator/LabelEarnings"),
          meta: { title: "Label Earnings" }
        },
        {
          name: "LabelDocuments",
          path: "label-documents",
          component: () => import("@/views/label-administrator/LabelDocuments"),
          meta: { title: "Label Documents" }
        },
        {
          name: "LabelReleases",
          path: "label-releases",
          component: () => import("@/views/label-administrator/LabelReleases"),
          meta: { title: "Label Releases" }
        },
        {
          name: "LabelArtists",
          path: "label-artists",
          component: () => import("@/views/label-administrator/LabelArtists"),
          meta: { title: "Label Artists" }
        },
        {
          name: "LabelUsers",
          path: "label-users",
          component: () => import("@/views/label-administrator/LabelUsers"),
          meta: { title: "Label Users" }
        },
        {
          name: "LabelInformation",
          path: "label-information",
          component: () => import("@/views/label-administrator/LabelInformation"),
          meta: { title: "Label Information" }
        },
        // Publisher Administrator
        {
          name: "PublisherEarnings",
          path: "publisher-earnings",
          component: () => import("@/views/publisher-administrator/PublisherEarnings"),
          meta: { title: "Publisher Earnings" }
        },
        {
          name: "PublisherDocuments",
          path: "publisher-documents",
          component: () => import("@/views/publisher-administrator/PublisherDocuments"),
          meta: { title: "Publisher Documents" }
        },
        {
          name: "PublisherCompositions",
          path: "publisher-compositions",
          component: () => import("@/views/publisher-administrator/PublisherCompositions"),
          meta: { title: "Publisher Compositions" }
        },
        {
          name: "PublisherUsers",
          path: "publisher-users",
          component: () => import("@/views/publisher-administrator/PublisherUsers"),
          meta: { title: "Publisher Users" }
        },
        {
          name: "PublisherInformation",
          path: "publisher-information",
          component: () => import("@/views/publisher-administrator/PublisherInformation"),
          meta: { title: "Publisher Information" }
        },
        // System User
        {
          name: "UserProfile",
          path: "my-profile",
          component: () => import("@/views/system-user/UserProfile"),
          meta: { title: "My Profile" }
        },
        {
          name: "UserEarnings",
          path: "my-earnings",
          component: () => import("@/views/system-user/UserEarnings"),
          meta: { title: "My Earnings" }
        },
        {
          name: "UserDocuments",
          path: "my-documents",
          component: () => import("@/views/system-user/UserDocuments"),
          meta: { title: "My Documents" }
        },
        {
          name: "UserCompositions",
          path: "my-compositions",
          component: () => import("@/views/system-user/UserCompositions"),
          meta: { title: "My Compositions" }
        },
        {
          name: "UserRecordings",
          path: "my-recordings",
          component: () => import("@/views/system-user/UserRecordings"),
          meta: { title: "My Recordings" }
        },
        {
          name: "UserReleases",
          path: "my-releases",
          component: () => import("@/views/system-user/UserReleases"),
          meta: { title: "My Releases" }
        }
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
          meta: { unauthenticatedOk: true, title: "Login" }
        },
        {
          name: "404",
          path: "*",
          component: () => import("@/views/FourOhFour"),
          meta: { unauthenticatedOk: true, title: "404"  }
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
    if (tokenExpiration < now) next("/login");
    else next();
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

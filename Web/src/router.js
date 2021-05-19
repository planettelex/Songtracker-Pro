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
          meta: { titleKey: "SystemInformation" }
        },
        {
          name: "PublishingCompanies",
          path: "publishing-companies",
          component: () => import("@/views/system-administrator/PublishingCompanies"),
          meta: { titleKey: "PublishingCompanies" }
        },
        {
          name: "RecordLabels",
          path: "record-labels",
          component: () => import("@/views/system-administrator/RecordLabels"),
          meta: { titleKey: "RecordLabels" }
        },
        {
          name: "Platforms",
          path: "platforms",
          component: () => import("@/views/system-administrator/Platforms"),
          meta: { titleKey: "Platforms" }
        },
        {
          name: "Artists",
          path: "artists",
          component: () => import("@/views/system-administrator/Artists"),
          meta: { titleKey: "Artists" }
        },
        {
          name: "Users",
          path: "users",
          component: () => import("@/views/system-administrator/Users"),
          meta: { titleKey: "Users" }
        },
        // Label Administrator
        {
          name: "LabelEarnings",
          path: "label-earnings",
          component: () => import("@/views/label-administrator/LabelEarnings"),
          meta: { titleKey: "Earnings" }
        },
        {
          name: "LabelDocuments",
          path: "label-documents",
          component: () => import("@/views/label-administrator/LabelDocuments"),
          meta: { titleKey: "Documents" }
        },
        {
          name: "LabelRecordings",
          path: "label-recordings",
          component: () => import("@/views/label-administrator/LabelRecordings"),
          meta: { titleKey: "Releases" }
        },
        {
          name: "LabelReleases",
          path: "label-releases",
          component: () => import("@/views/label-administrator/LabelReleases"),
          meta: { titleKey: "Recordings" }
        },
        {
          name: "LabelArtists",
          path: "label-artists",
          component: () => import("@/views/label-administrator/LabelArtists"),
          meta: { titleKey: "Artists" }
        },
        {
          name: "LabelUsers",
          path: "label-users",
          component: () => import("@/views/label-administrator/LabelUsers"),
          meta: { titleKey: "Users" }
        },
        {
          name: "LabelInformation",
          path: "label-information",
          component: () => import("@/views/label-administrator/LabelInformation"),
          meta: { titleKey: "Information" }
        },
        // Publisher Administrator
        {
          name: "PublisherEarnings",
          path: "publisher-earnings",
          component: () => import("@/views/publisher-administrator/PublisherEarnings"),
          meta: { titleKey: "Earnings" }
        },
        {
          name: "PublisherDocuments",
          path: "publisher-documents",
          component: () => import("@/views/publisher-administrator/PublisherDocuments"),
          meta: { titleKey: "Documents" }
        },
        {
          name: "PublisherCompositions",
          path: "publisher-compositions",
          component: () => import("@/views/publisher-administrator/PublisherCompositions"),
          meta: { titleKey: "Compositions" }
        },
        {
          name: "PublisherUsers",
          path: "publisher-users",
          component: () => import("@/views/publisher-administrator/PublisherUsers"),
          meta: { titleKey: "Users" }
        },
        {
          name: "PublisherInformation",
          path: "publisher-information",
          component: () => import("@/views/publisher-administrator/PublisherInformation"),
          meta: { titleKey: "Information" }
        },
        // System User
        {
          name: "UserProfile",
          path: "my-profile",
          component: () => import("@/views/system-user/UserProfile"),
          meta: { titleKey: "MyProfile" }
        },
        {
          name: "UserEarnings",
          path: "my-earnings",
          component: () => import("@/views/system-user/UserEarnings"),
          meta: { titleKey: "MyEarnings" }
        },
        {
          name: "UserDocuments",
          path: "my-documents",
          component: () => import("@/views/system-user/UserDocuments"),
          meta: { titleKey: "MyDocuments" }
        },
        {
          name: "UserCompositions",
          path: "my-compositions",
          component: () => import("@/views/system-user/UserCompositions"),
          meta: { titleKey: "MyCompositions" }
        },
        {
          name: "UserRecordings",
          path: "my-recordings",
          component: () => import("@/views/system-user/UserRecordings"),
          meta: { titleKey: "MyRecordings" }
        },
        {
          name: "UserReleases",
          path: "my-releases",
          component: () => import("@/views/system-user/UserReleases"),
          meta: { titleKey: "MyReleases" }
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
          meta: { unauthenticatedOk: true, titleKey: "Login" }
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

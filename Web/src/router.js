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
          component: () => import("@/views/system-administrator/SystemInformation")
        },
        {
          name: "PublishingCompanies",
          path: "publishing-companies",
          component: () => import("@/views/system-administrator/PublishingCompanies"),
        },
        {
          name: "RecordLabels",
          path: "record-labels",
          component: () => import("@/views/system-administrator/RecordLabels"),
        },
        {
          name: "Platforms",
          path: "platforms",
          component: () => import("@/views/system-administrator/Platforms"),
        },
        {
          name: "Artists",
          path: "artists",
          component: () => import("@/views/system-administrator/Artists"),
        },
        {
          name: "Users",
          path: "users",
          component: () => import("@/views/system-administrator/Users"),
        },
        // Label Administrator
        {
          name: "LabelEarnings",
          path: "label-earnings",
          component: () => import("@/views/label-administrator/Earnings"),
        },
        // Publisher Administrator
        {
          name: "PublisherEarnings",
          path: "publisher-earnings",
          component: () => import("@/views/publisher-administrator/Earnings"),
        },
        // System User
        {
          name: "UserEarnings",
          path: "my-earnings",
          component: () => import("@/views/system-user/Earnings"),
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
          component: () => import("@/views/authentication/Login"),
          meta: { unauthenticatedOk: true }
        },
        {
          name: "404",
          path: "*",
          component: () => import("@/views/authentication/FourOhFour"),
          meta: { unauthenticatedOk: true }
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
  else if (store.state.UserAuthenticated) {
    next();
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

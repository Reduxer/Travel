import Vue from "vue";
import VueRouter from "vue-router";
import Home from "../views/Home.vue";
import TourLists from "@/views/AdminDashboard/TourLists";
import TourPackages from "@/views/AdminDashboard/TourPackages";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/about",
    name: "About",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
  },
  {
    path: "/admin-dashboard",
    component: () => import("@/views/AdminDashboard"),
    children: [
      {
        path: "",
        component: () => import("@/views/AdminDashboard/DefaultContent"),
      },
      {
        path: "weather-forecast",
        component: () => import("@/views/AdminDashboard/WeatherForecast"),
      },
      /* eager loading through static import statement */
      {
        path: "tour-lists",
        component: TourLists,
      },
      {
        path: "tour-packages",
        component: TourPackages,
      },
    ],
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;

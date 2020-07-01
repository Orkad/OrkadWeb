<template>
  <v-app v-if="isAuthenticated">
    <v-app-bar app>
      <!-- <v-app-bar-nav-icon></v-app-bar-nav-icon> -->

      <v-toolbar-title>Orkad Web Vue</v-toolbar-title>

      <template v-slot:extension>
        <app-bar> </app-bar>
      </template>

      <v-spacer></v-spacer>

      <v-btn icon to="/login">
        <v-icon>mdi-account</v-icon>
      </v-btn>
    </v-app-bar>

    <v-content>
      <v-container>
        <router-view></router-view>
      </v-container>
    </v-content>

    <v-footer app> </v-footer>
  </v-app>
  <app-login v-else> </app-login>
</template>

<script>
import { mapGetters, mapState, mapActions } from "vuex";
import AppBar from "@/components/AppBar.vue";
import AppLogin from "@/components/AppLogin.vue";

export default {
  name: "App",
  components: { AppBar, AppLogin },
  data: () => ({
    menuVisible: false,
  }),
  computed: {
    ...mapState("context", ["profile"]),
    ...mapGetters("context", ["isAuthenticated"]),
  },
  created() {
    this.refresh();
  },
  methods: {
    ...mapActions("context", ["logout"]),
    ...mapActions("context", ["refresh"]),
  },
};
</script>

<style scoped>
.container {
  max-width: 1280px;
}
</style>

<style>
.right-input input {
  text-align: right;
}
</style>

<template>
  <md-app md-mode="reveal">
    <md-app-toolbar class="md-primary">
      <md-button class="md-icon-button" @click="menuVisible = !menuVisible" v-if="isAuthenticated">
        <md-icon>menu</md-icon>
      </md-button>
      <span class="md-title md-layout-item">Orkad Web Vue</span>
      <span v-if="isAuthenticated">Connecté en tant que {{profile.name}}</span>
      <md-button v-if="isAuthenticated" @click="logout()" class="md-raised md-accent">Se déconnecter</md-button>
      <md-button v-if="!isAuthenticated" class="md-raised" to="/login" exact>Se connecter</md-button>
    </md-app-toolbar>
    <md-app-drawer :md-active.sync="menuVisible">
      <md-toolbar class="md-transparent" md-elevation="0">Navigation</md-toolbar>
      <md-list>
        <md-list-item to="/" exact @click="menuVisible = false">
          <md-icon>home</md-icon>
          <span class="md-list-item-text">Accueil</span>
        </md-list-item>
        <md-list-item to="/shares" exact @click="menuVisible = false">
          <md-icon>share</md-icon>
          <span class="md-list-item-text">Partages</span>
        </md-list-item>
      </md-list>
    </md-app-drawer>
    <md-app-content>
      <router-view></router-view>
    </md-app-content>
  </md-app>
</template>

<script>
import { mapGetters, mapState, mapActions } from "vuex";

export default {
  name: "App",
  data: () => ({
    menuVisible: false
  }),
  computed: {
    ...mapState("context", ["profile"]),
    ...mapGetters("context", ["isAuthenticated"])
  },
  methods: {
    ...mapActions("context", ["logout"]),
    ...mapActions("context", ["refresh"])
  },
  created() {
    this.refresh().then(() => {
      // if(this.isAuthenticated){
      //   this.menuVisible = true;
      // }
      // this.$router.push('/login');
    });
  },
};
</script>

<style scoped>
.md-app {
  min-height: 100vh;
}

.md-app-content {
  /* On centre */
  margin: 0 auto;

  /* On limite la largeur pour les grands écrans */
  max-width: 960px;

  /* On gère la présence d'une marge */
  width: 90%;

  border: 0px;
}

/*
* Comme la dite marge est exprimée en pourcentage,
* on l'augmente sur les devices moins larges.
*/
@media only screen and (min-width: 993px) {
  md-app-content {
    width: 85%;
  }
}
</style>

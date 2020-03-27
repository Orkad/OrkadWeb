<template>
  <md-app md-mode="reveal" style="min-height: 100vh;">
    <md-app-toolbar class="md-primary">
      <md-button class="md-icon-button" @click="menuVisible = !menuVisible">
        <md-icon>menu</md-icon>
      </md-button>
      <span class="md-title md-layout-item">
        Orkad Web Vue
      </span>
      <login></login>
      <md-button to="/login" exact class="md-icon-button">
        <md-icon>account_circle</md-icon>
      </md-button>
    </md-app-toolbar>
    <md-app-drawer :md-active.sync="menuVisible">
      <md-toolbar class="md-transparent" md-elevation="0">Navigation</md-toolbar>
      <md-list>
        <md-list-item to="/" exact @click="menuVisible = false">
          <md-icon>home</md-icon>
          <span class="md-list-item-text">Accueil</span>
        </md-list-item>
        <md-list-item to="/login" exact @click="menuVisible = false">
          <md-icon>send</md-icon>
          <span class="md-list-item-text">Se connecter</span>
        </md-list-item>
      </md-list>
    </md-app-drawer>
    <md-app-content>
      <router-view class="main-container"></router-view>
    </md-app-content>
  </md-app>
</template>

<script>

export default {
  name: "App",
  data: () => ({
    menuVisible: false
  })
};
</script>

<style scoped>
md-app {
  min-height: 100vh;
}

.main-container {
    /* On centre */
    margin: 0 auto;

    /* On limite la largeur pour les grands écrans */
    max-width: 1280px;

    /* On gère la présence d'une marge */
    width: 90%;
}

/*
* Comme la dite marge est exprimée en pourcentage,
* on l'augmente sur les devices moins larges.
*/
@media only screen and (min-width: 993px){
    .main-container {
        width: 85%;
    }
}
</style>

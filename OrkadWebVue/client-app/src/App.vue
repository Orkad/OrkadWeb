<template>
  <md-app md-waterfall md-mode="fixed">
    <md-app-toolbar class="md-primary">
      <i class="brand-logo"></i>
      <span class="md-title">Orkad</span>
    </md-app-toolbar>
    <md-app-drawer md-permanent="full">
      <md-toolbar class="md-transparent" md-elevation="0">Navigation</md-toolbar>

      <md-list>
        <md-list-item to="/" exact>
          <md-icon>home</md-icon>
          <span class="md-list-item-text">Accueil</span>
        </md-list-item>

        <md-list-item to="/login" exact>
          <md-icon>send</md-icon>
          <span class="md-list-item-text">Se connecter</span>
        </md-list-item>
      </md-list>
    </md-app-drawer>
    <md-app-content>
      <router-view></router-view>
    </md-app-content>
    <!-- <main-menu />
    -->
  </md-app>
</template>

<script>
import HelloWorld from "./components/HelloWorld";
import MainMenu from "./components/MainMenu";
import Loading from "./components/Loading";

export default {
  name: "App",
  components: {
    HelloWorld,
    MainMenu,
    Loading
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 10px;
}
</style>

<template>
  <md-app md-mode="reveal" style="min-height: 100vh;">
    <md-app-toolbar class="md-primary">
      <md-button class="md-icon-button" @click="menuVisible = !menuVisible">
        <md-icon>menu</md-icon>
      </md-button>
      <span class="md-title"></span>
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
      <router-view></router-view>
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
</style>

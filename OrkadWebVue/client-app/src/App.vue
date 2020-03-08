<template>
  <div id="app">
    <main-menu />
    <router-view></router-view>
  </div>
</template>

<script>
import HelloWorld from './components/HelloWorld'
import MainMenu from './components/MainMenu'
import Loading from './components/Loading'

export default {
  name: 'App',
  components: {
    HelloWorld, MainMenu, Loading
  }
}
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

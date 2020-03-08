<template>
  <nav>
    <div class="nav-wrapper">
      <i class="brand-logo right"></i>
      <ul id="nav-mobile" class="left hide-on-med-and-down">
        <li><router-link to="/">Accueil</router-link></li>
        <li><router-link to="/login">Se connecter</router-link></li>
        <li><router-link to="/about">A Propos</router-link></li>
      </ul>
    </div>
  </nav>
</template>

<script>
  export default {
    name: 'MainMenu'
  }
</script>

<style scoped>
.brand-logo{
  background-image: url("../assets/logo.png");
  background-size: contain;
  height: 50px;
  width: 50px;
}
</style>
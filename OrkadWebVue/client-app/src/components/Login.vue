<template>
  <div class="container">
    <div class="row">
      <div class="input-field col m6">
        <input id="password" type="text" class="validate">
        <label for="password">Nom d'utilisateur</label>
      </div>
      <div class="input-field col m6">
        <input id="password" type="password" class="validate">
        <label for="password">Mot de passe</label>
      </div>
    </div>
  </div>
  
</template>

<script>
  export default {
    name: 'Login'
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
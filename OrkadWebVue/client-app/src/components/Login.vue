<template>
  <md-field>
    <label>Initial Value</label>
    <md-input v-model="initial"></md-input>
  </md-field>
  
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
<template>
  <div v-if="spin" class="preloader-wrapper big active">
    <div class="spinner-layer spinner-blue-only">
      <div class="circle-clipper left">
        <div class="circle"></div>
      </div><div class="gap-patch">
        <div class="circle"></div>
      </div><div class="circle-clipper right">
        <div class="circle"></div>
      </div>
    </div>
  </div>
  <div v-else class="progress">
    <md-progress-bar md-mode="indeterminate"></md-progress-bar>
  </div>
</template>

<script>
  export default {
    name: 'Loading',
    props:{
      spin: Boolean
    }
  }
</script>
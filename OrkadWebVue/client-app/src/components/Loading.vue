<template>
  <div class="preloader-wrapper big active" v-if="spin">
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
  <div class="progress" v-else>
    <div class="indeterminate"></div>
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
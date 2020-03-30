<template>
  <span>Share Component (id:{{share.id}})</span>
</template>

<script>
export default {
  name: 'Share',
  data: () => ({
    share: null,
  }),
  mounted(){
    this.share.id = this.$route.params.id
  }
}
</script>
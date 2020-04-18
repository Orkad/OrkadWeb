<template>
  <div v-if="spin" class="text-center">
    <v-progress-circular
      :size="50"
      color="primary"
      indeterminate
    ></v-progress-circular>
  </div>
  <div v-else>
    <v-progress-linear indeterminate color="cyan"></v-progress-linear>
  </div>
</template>

<script>
export default {
  name: "Loading",
  props: {
    spin: Boolean,
  },
};
</script>

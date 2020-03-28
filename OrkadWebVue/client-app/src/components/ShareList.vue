<template>
  <div>
    <h2>Partages en cours</h2>
    <md-list>
      <md-list-item v-for="share in shares" :key="share.id" @click="menuVisible = false">
        <md-icon>payment</md-icon>
        <span class="md-list-item-text">{{share.name}}</span>
        <md-badge class="md-primary md-square" :md-content="share.attendeeCount + ' participants'" />
      </md-list-item>
    </md-list>
  </div>
</template>

<script>
import ShareApi from "@/services/api/ShareApi";

export default {
  name: "ShareList",
  data: function() {
    return {
      shares: []
    };
  },
  mounted: function() {
    ShareApi.getShares().then(s => this.shares = s);
  }
};
</script>
<template>
  <div>
    <h2>Partages en cours</h2>
    <v-list>
      <v-list-item v-for="share in shares" :key="share.id" :to="{ path: '/shares/'+share.id}" exact @click="menuVisible = false">
        <v-list-item-icon>
          <v-icon color="primary">payment</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{share.name}}</v-list-item-title>
          <v-list-item-subtitle>{{share.attendeeCount}} participants</v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-list>
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
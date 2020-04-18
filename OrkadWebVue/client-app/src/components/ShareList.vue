<template>
  <loading v-if="loading" spin />
  <div v-else>
    <h2>Partages en cours</h2>
    <v-list>
      <v-list-item
        v-for="share in shares"
        :key="share.id"
        :to="{ path: '/shares/' + share.id }"
        exact
        @click="menuVisible = false"
      >
        <v-list-item-icon>
          <v-icon color="primary">payment</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{ share.name }}</v-list-item-title>
          <v-list-item-subtitle
            >{{ share.attendeeCount }} participants</v-list-item-subtitle
          >
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-btn to="create/share">Créer un partage</v-btn>
      </v-list-item>
    </v-list>
  </div>
</template>

<script>
import Axios from "axios";
import Loading from "@/components/Loading.vue";

export default {
  name: "ShareList",
  components: { Loading },
  data: () => ({
    loading: false,
    shares: [],
  }),
  mounted: function () {
    this.loading = true;
    Axios.get("api/shares").then((res) => {
      this.shares = res.data;
      this.loading = false;
    });
  },
};
</script>

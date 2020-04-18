<template>
  <v-form ref="form" v-model="f" lazy-validation @submit.prevent="addShare">
    <v-card>
      <v-card-title>
        Créer un partage
      </v-card-title>
      <v-card-text>
        <v-text-field
          v-model="form.name"
          :rules="form.nameRules"
          label="Nom du partage"
          type="text"
        >
          <v-icon slot="prepend">mdi-comment-text-outline</v-icon>
        </v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          :disabled="loading || !f"
          color="success"
          class="mr-4"
          type="submit"
        >
          Créer
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-form>
</template>

<script>
import Axios from "axios";

export default {
  name: "ShareAdd",
  data: () => ({
    loading: false,
    f: null,
    form: {
      name: null,
    },
  }),
  methods: {
    addShare() {
      this.loading = true;
      Axios.post("api/shares", this.form)
      .then((res) => {
        this.loading = false;
        this.$router.push('/shares/'+res.data.id);
      });
    },
  },
};
</script>

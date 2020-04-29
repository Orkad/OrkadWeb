<template>
  <loading v-if="loading" spin />
  <v-form v-else ref="form" v-model="valid" @submit.prevent="submit">
    <v-card outlined tile>
      <v-card-title>Déclarer un remboursement</v-card-title>
      <v-card-text>
        <v-row dense align="center" justify="center">
          <v-col cols="8">
            <v-select
              v-model="form.receiver"
              :items="others"
              :rules="form.receiverRules"
              label="Destinataire"
            ></v-select>
          </v-col>
          <v-col cols="3">
            <v-text-field
              v-model="form.amount"
              :rules="form.amountRules"
              label="Montant"
              type="text"
              class="right-input"
              suffix="€"
            >
            </v-text-field>
          </v-col>
          <v-col cols="1">
            <v-btn icon color="blue" type="submit" cols="2" :disabled="!valid">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-form>
</template>

<script>
import axios from "axios";
import Loading from "@/components/Loading.vue";

export default {
  name: "RefundAdd",
  components: { Loading },
  props: {
    shareId: {
      type: Number,
      required: true,
    },
  },
  data: () => ({
    loading: true,
    valid: false,
    others: [],
    form: {
      receiver: null,
      receiverRules: [(v) => !!v || "requis"],
      amount: null,
      amountRules: [
        (v) => !!v || "requis",
        (v) => /^\d*\.?\d*$/.test(v) || "nombre",
        (v) => v > 0 || "négatif interdit",
      ],
    },
  }),
  mounted() {
    this.loading = true;
    axios
      .get("api/shares/" + this.shareId + "/others")
      .then((r) => (this.others = r.data))
      .finally(() => (this.loading = false));
  },
  methods: {
    submit() {
      this.loading = true;
      axios
        .post("api/shares/" + this.shareId + "/refunds", this.form)
        .then((r) => {
          this.$emit("created", r.data);
        })
        .finally(() => {
          this.loading = false;
        });
    },
  },
};
</script>

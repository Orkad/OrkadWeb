<template>
  <v-form
    ref="form"
    v-model="valid"
    lazy-validation
    @submit.prevent="addExpense"
  >
    <v-row dense align="center" justify="center">
      <v-col cols="8">
        <v-text-field
          v-model="form.name"
          :rules="form.nameRules"
          label="Nom de la dépense"
        >
        </v-text-field>
      </v-col>
      <v-col cols="3">
        <currency-input
          v-model="form.amount"
          :rules="form.amountRules"
          label="Montant"
        >
        </currency-input>
      </v-col>
      <v-col cols="1">
        <v-btn icon color="blue" type="submit" cols="2" :disabled="!valid">
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-form>
</template>

<script>
import Axios from "axios";
import CurrencyInput from "@/components/shared/CurrencyInput.vue";

export default {
  name: "ExpenseAdd",
  components: { CurrencyInput },
  props: {
    shareId: {
      type: Number,
      required: true,
    },
  },
  data: () => ({
    valid: false,
    form: {
      name: null,
      nameRules: [(v) => !!v || "requis"],
      amount: null,
      amountRules: [
        (v) => !!v || "requis",
        (v) => /^\d*\.?\d*$/.test(v) || "nombre",
        (v) => v > 0 || "négatif interdit",
      ],
    },
  }),
  methods: {
    addExpense() {
      this.$refs.form.validate();
      Axios.post("api/shares/" + this.shareId + "/expenses", {
        name: this.form.name,
        amount: this.form.amount,
      }).then((res) => {
        this.$emit("created", res.data);
      });
    },
  },
};
</script>

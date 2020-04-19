<template>
  <v-form ref="form" v-model="valid" lazy-validation @submit.prevent="addExpense">
    <v-card outlined tile>
      <v-card-title>
        Ajouter une dépense
      </v-card-title>
      <v-card-text>
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
            <v-btn
              icon
              color="blue"
              type="submit"
              cols="2"
              :disabled="!valid"
            >
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-form>
</template>

<script>
import Axios from "axios";
export default {
  name: "ExpenseAdd",
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
      amountRules: [(v) => !!v || "requis", 
      (v) => /^\d*\.?\d*$/.test(v) || "nombre",
      (v) => v > 0 || "négatif interdit"],
    },
  }),
  methods: {
    addExpense() {
      this.$refs.form.validate();
      Axios.post("api/shares/" + this.shareId + "/expenses", {
        name: this.form.name,
        amount: this.form.amount,
      }).then((res) => {
        this.$emit("expense-created", res.data);
        this.$refs.form.reset();
      });
    },
  },
};
</script>

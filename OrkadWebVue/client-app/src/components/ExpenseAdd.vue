<template>
  <v-form ref="form" v-model="f" lazy-validation @submit.prevent="addExpense">
    <v-card outlined tile>
      <v-card-title>
        Ajouter une dépense
      </v-card-title>
      <v-card-text>
        <v-text-field
          v-model="form.name"
          :rules="form.nameRules"
          label="Nom de la dépense"
          type="text"
        >
          <v-icon slot="prepend" color="orange"
            >mdi-comment-text-outline</v-icon
          >
        </v-text-field>
        <v-text-field
          v-model="form.amount"
          :rules="form.amountRules"
          label="Montant de la dépense"
          type="text"
        >
          <v-icon slot="prepend" color="orange">mdi-currency-eur</v-icon>
        </v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          :disabled="sending || !f"
          color="success"
          class="mr-4"
          type="submit"
        >
          Ajouter
        </v-btn>
      </v-card-actions>
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
    sending: false,
    f: null,
    dp: null,
    form: {
      name: null,
      nameRules: [(v) => !!v || "Le nom de la dépense est obligatoire"],
      amount: null,
      amountRules: [
        (v) => !!v || "Le montant de la dépense est obligatoire",
        (v) => v > 0 || "Le montant de la dépense doit être un nombre positif",
      ],
    },
  }),
  methods: {
    addExpense() {
      this.sending = true;
      Axios.post("api/shares/" + this.shareId + "/expenses", {
        name: this.form.name,
        amount: this.form.amount,
      }).then((res) => {
        this.$emit("expense-created", res.data);
        this.$refs.form.validate();
        this.clearForm();
      });
    },
    clearForm() {
      this.form.name = null;
      this.form.amount = null;
    },
  },
};
</script>

<template>
  <v-card outlined tile :loading="loading">
    <v-card-title>
      Dépenses
    </v-card-title>
    <v-card-text>
      <v-list dense>
        <v-list-item v-for="expense in expenses" :key="expense.id">
          <v-list-item-content>
            <v-list-item-title>
              {{ expense.name }}
            </v-list-item-title>
            <v-list-item-subtitle>
              {{ expense.date | moment("DD/MM/YYYY") }}
            </v-list-item-subtitle>
          </v-list-item-content>
          {{ expense.amount }}€
          <v-list-item-action v-if="mine">
            <v-btn x-small icon @click="deleteExpenseConfirm(expense)">
              <v-icon color="red"
                >mdi-delete</v-icon
              >
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
    </v-card-text>
    <!-- Boite de confirmation dynamique -->
    <confirm-dialog 
    ref="confirm"
    :title="confirm.title"
    :message="confirm.message"
    :confirm-text="confirm.confirmText"
    :cancel-text="confirm.cancelText"
    @confirm="confirm.action"></confirm-dialog>
  </v-card>
</template>

<script>
import { mapState } from "vuex";
import axios from "axios";

import ConfirmDialog from "@/components/shared/ConfirmDialog.vue";
export default {
  components: { ConfirmDialog },
  props: {
    shareId: {
      type: Number,
      required: true,
    },
    userId: {
      type: Number,
      required: true,
    },
    expenses: {
      type: Array,
      required: true,
    },
  },
  data: () => ({
    loading: false,
    mine: false,
    confirm: {
      title: "Confirmation",
      message: "Êtes vous sûr ?",
      confirmText: "Confirmer",
      cancelText: "Annuler",
      action: () => {},
    },
  }),
  computed: {
    ...mapState("context", ["profile"]),
  },
  mounted() {
    this.mine = this.profile.id === this.userId.toString();
  },
  methods: {
    deleteExpenseConfirm(expense) {
      this.confirm.title = "Suppression de la dépense";
      this.confirm.message = "Souhaitez vous vraiment supprimer la dépense " +
          expense.name +
          " d'un montant de " +
          expense.amount +
          "€ ?";
      this.confirm.action = () => this.deleteExpense(expense.id);
      this.confirm.confirmText = "Supprimer";
      this.confirm.cancelText = "Annuler"
      this.$refs["confirm"].show();
    },
    deleteExpense(expenseId) {
      this.loading = true;
      axios
        .delete("api/shares/" + this.shareId + "/expenses/" + expenseId)
        .then(() => this.$emit("deleted", expenseId))
        .finally(() => (this.loading = false));
    },
  },
};
</script>

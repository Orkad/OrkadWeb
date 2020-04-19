<template>
  <loading v-if="loading" spin />
  <div v-else>
    <v-row>
      <v-col cols="12">
        <v-card outlined tile>
          <v-card-title>
            Partage : {{ share.name }}
            <v-spacer></v-spacer>
            <v-btn v-if="mine" icon color="red">
              <v-icon @click="deleteShareConfirm">mdi-delete</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-subtitle
            >Total des dépenses : {{ getTotalExpenses() }}€</v-card-subtitle
          >
        </v-card>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="6">
        <expense-add
          :share-id="share.id"
          @expense-created="addExpenseItem"
        ></expense-add>
      </v-col>
      <v-col cols="6">
        <refund-add
          :share-id="share.id"
          @refund-created="addRefundItem"
        ></refund-add>
      </v-col>
    </v-row>
    <v-row>
      <v-col v-for="user in share.users" :key="user.id" cols="4">
        <v-card outlined tile>
          <v-card-title
            >{{ user.name }}
            <span v-if="owned(user.id)"> (vous)</span>
          </v-card-title>

          <v-card-text>
            <v-list dense>
              <v-list-item v-for="expense in user.expenses" :key="expense.id">
                <v-list-item-content>
                  <v-list-item-title>
                    {{ expense.name }}
                  </v-list-item-title>
                  <v-list-item-subtitle>
                    {{
                      expense.date | moment("DD/MM/YYYY")
                    }}</v-list-item-subtitle
                  >
                </v-list-item-content>

                {{ expense.amount }}€
                <v-list-item-action v-if="owned(user.id)">
                  <v-btn x-small icon>
                    <v-icon
                      color="red"
                      @click="deleteExpenseConfirm(expense.id, expense.name)"
                      >mdi-delete</v-icon
                    >
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
          </v-card-text>

          <v-card-actions> </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <confirm-dialog
      ref="deleteShareConfirmDialog"
      title="Suppression du partage"
      message="Souhaitez vous vraiment supprimer le partage ?"
      confirm-text="Supprimer"
      @confirm="deleteShare"
    ></confirm-dialog>

    <!-- Boite de confirmation dynamique -->
    <confirm-dialog
      ref="confirm"
      :title="confirm.title"
      :message="confirm.message"
      :confirm-text="confirm.confirmText"
      :cancel-text="confirm.cancelText"
      @confirm="confirm.action"
    ></confirm-dialog>
  </div>
</template>

<script>
import Loading from "@/components/Loading.vue";
import ConfirmDialog from "@/components/shared/ConfirmDialog.vue";
import ExpenseAdd from "@/components/ExpenseAdd.vue";
import RefundAdd from "@/components/RefundAdd.vue";
import axios from "axios";
import { mapState } from "vuex";

export default {
  name: "Share",
  components: { Loading, ExpenseAdd, RefundAdd, ConfirmDialog },
  data: () => ({
    loading: true,
    mine: false,
    share: {},
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
  created() {
    this.loading = true;
    var id = this.$route.params.id;
    axios.get("/api/shares/" + id).then((r) => {
      this.share = r.data;
      this.mine = this.profile.id === r.data.ownerId.toString();
      this.loading = false;
    });
  },
  mounted() {
    this.$on("expense-deleted", this.deleteExpenseItem);
  },
  methods: {
    deleteShareConfirm() {
      this.$refs.deleteShareConfirmDialog.show();
    },
    deleteShare() {
      var id = this.$route.params.id;
      axios.delete("/api/shares/" + id).then(() => {
        this.$router.push("/shares");
      });
    },
    addExpenseItem(expenseItem) {
      var user = this.share?.users?.filter(
        (u) => u.id.toString() == this.profile.id
      )[0];
      if (user) {
        user.expenses.unshift(expenseItem);
      }
    },
    deleteExpenseItem(expenseId) {
      var user = this.share?.users?.filter(
        (u) => u.id.toString() == this.profile.id
      )[0];
      if (user) {
        var expenseIndex = user.expenses.findIndex((e) => e.id === expenseId);
        if (expenseIndex !== -1) {
          user.expenses.splice(expenseIndex, 1);
        }
      }
    },
    deleteExpenseConfirm(id, display) {
      this.confirm.title = "Suppression de la dépense";
      this.confirm.message =
        "Souhaitez vous vraiment supprimer la dépense " + display + " ?";
      this.confirm.confirmText = "Supprimer";
      this.confirm.cancelText = "Annuler";
      this.confirm.action = () => this.deleteExpense(id);
      this.$refs.confirm.show();
    },
    deleteExpense(id) {
      var shareId = this.$route.params.id;
      axios.delete("/api/shares/" + shareId + "/expenses/" + id).then(() => {
        this.$emit("expense-deleted", id);
      });
    },
    getTotalExpenses() {
      const sum = (a, b) => a + b;
      return this.share.users
        .map((u) => u.expenses.map((e) => e.amount).reduce(sum, 0))
        .reduce(sum, 0);
    },
    addRefundItem(refundItem) {
      console.log(refundItem);
    },
    owned(id) {
      return id.toString() === this.profile.id;
    },
  },
};
</script>

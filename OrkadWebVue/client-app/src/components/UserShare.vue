<template>
  <div>
    <v-card outlined tile>
      <v-card-title
        >{{ user.name }}
        <span v-if="owned(user.id)"> (vous)</span>
        <v-spacer></v-spacer>
        <v-chip class="ma-2" :color="getUserBalanceColor(user.id)">
          {{ getUserBalance(user.id) }}€
        </v-chip>
      </v-card-title>
    </v-card>
    <refund-list
      :share-id="share.id"
      :user-id="user.id"
      :refunds="user.refunds"
      @deleted="removeRefund"
    ></refund-list>
    <expense-list
      :share-id="share.id"
      :user-id="user.id"
      :expenses="user.expenses"
      @deleted="removeExpense(user, $event)"
    >
    </expense-list>
  </div>
</template>

<script>
import ExpenseList from "@/components/ExpenseList.vue";
import RefundList from "@/components/RefundList.vue";
import axios from "axios";
import _ from "lodash";
import { mapState } from "vuex";

export default {
  name: "UserShare",
  components: {
    ExpenseList,
    RefundList,
  },
  data: () => ({
    user: null,
    share: null,
  }),
  computed: {
    ...mapState("context", ["profile"]),
  },
  created() {
    axios.get("/api/shares/" + this.id).then((r) => {
      this.share = r.data;
      this.mine = this.profile.id === r.data.ownerId.toString();
      this.loading = false;
    });
    this.refreshAvailableUsers();
  },
  methods: {
    addUser() {
      if (this.userToAdd) {
        axios
          .post("api/shares/" + this.share.id + "/users/add/" + this.userToAdd)
          .then(() => {
            axios.get("/api/shares/" + this.id).then((r) => {
              this.share = r.data;
              this.mine = this.profile.id === r.data.ownerId.toString();
            });
            this.refreshAvailableUsers();
            this.toogleAdd = false;
          });
      }
    },
    refreshAvailableUsers() {
      axios.get("/api/shares/" + this.id + "/users/available").then((r) => {
        this.availableUsers = r.data;
      });
    },
    deleteShareConfirm() {
      this.$refs.deleteShareConfirmDialog.show();
    },
    deleteShare() {
      var id = this.$route.params.id;
      axios.delete("/api/shares/" + id).then(() => {
        this.$router.push("/shares");
      });
    },
    // enlève la dépense du tableau
    removeExpense(user, expenseId) {
      var expenseIndex = user.expenses.findIndex((e) => e.id === expenseId);
      if (expenseIndex !== -1) {
        user.expenses.splice(expenseIndex, 1);
      }
    },
    // enlève le remboursement de plusieurs tableaux
    removeRefund(refund) {
      var user = this.getLoggedUser();
      if (user) {
        let index = user.refunds.findIndex((r) => r.id === refund.id);
        if (index !== -1) {
          user.refunds.splice(index, 1);
        }
      }
      var otherUser = this.getUser(refund.receiverId);
      if (otherUser) {
        let index = otherUser.refunds.findIndex((r) => r.id === refund.id);
        if (index !== -1) {
          otherUser.refunds.splice(index, 1);
        }
      }
    },
    getTotalExpenses() {
      const sum = (a, b) => a + b;
      return this.share.users
        .map((u) => u.expenses.map((e) => e.amount).reduce(sum, 0))
        .reduce(sum, 0);
    },
    /** Récupère la moyenne des dépenses du partage*/
    getAverageExpenses() {
      return _.round(this.getTotalExpenses() / this.share.users.length, 2);
    },
    /** Récupère le total des opérations d'un utilisateur (dépenses et remboursements) */
    getUserTotalOperations(userId) {
      const sum = (a, b) => a + b;
      var user = this.getUser(userId);
      var totalExpenses = user.expenses.map((e) => e.amount).reduce(sum, 0);
      var totalEmittedRefund = user.refunds
        .filter((r) => r.emitterId === userId)
        .map((e) => e.amount)
        .reduce(sum, 0);
      var totalReceivedRefund = user.refunds
        .filter((r) => r.receiverId === userId)
        .map((e) => e.amount)
        .reduce(sum, 0);
      return _.round(
        totalExpenses + totalEmittedRefund - totalReceivedRefund,
        2
      );
    },
    getUserBalance(userId) {
      return _.round(
        this.getUserTotalOperations(userId) - this.getAverageExpenses(),
        2
      );
    },
    getUserBalanceColor(userId) {
      var balance = this.getUserBalance(userId);
      if (Math.abs(balance) < 10) {
        return "yellow";
      }
      if (balance > 0) {
        return "green";
      }
      return "red";
    },
    getLoggedUser() {
      var userId = parseInt(this.profile.id);
      return this.getUser(userId);
    },
    // récupère l'utilisateur dans le tableau
    getUser(userId) {
      return this.share?.users?.filter((u) => u.id === userId)[0];
    },
    // ajoute un remboursement à la liste existante
    addRefund(refund) {
      var user = this.getLoggedUser();
      user.refunds.unshift(refund);
      var otherUser = this.getUser(refund.receiverId);
      otherUser.refunds.unshift(refund);
    },
    owned(id) {
      return id.toString() === this.profile.id;
    },
  },
};
</script>

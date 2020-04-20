<template>
  <loading v-if="loading" spin />
  <div v-else>
    <v-row>
      <v-col cols="4">
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
      <v-col cols="4">
        <expense-add
          :share-id="share.id"
          @expense-created="addExpenseItem"
        ></expense-add>
      </v-col>
      <v-col cols="4">
        <refund-add
          :share-id="share.id"
          @refund-created="addRefundItem"
        ></refund-add>
      </v-col>
    </v-row>
    <v-row align="top" justify="center">
      <v-col v-for="user in share.users" :key="user.id" cols="4">
        <v-card outlined tile>
          <v-card-title
            >{{ user.name }}
            <span v-if="owned(user.id)"> (vous)</span>
            <v-spacer></v-spacer>
            <v-chip class="ma-2" color="red">
              -20.30€
            </v-chip>
          </v-card-title>
        </v-card>
        <refund-list
          :share-id="share.id"
          :user-id="user.id"
          :operations="user.operations"
        ></refund-list>
        <expense-list 
        :share-id="share.id" 
        :user-id="user.id"
        :expenses="user.expenses"
         @deleted="removeExpense(user, $event)"> </expense-list>
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
import ExpenseList from "@/components/ExpenseList.vue";
import RefundAdd from "@/components/RefundAdd.vue";
import RefundList from "@/components/RefundList.vue";
import axios from "axios";
import { mapState } from "vuex";

export default {
  name: "Share",
  components: {
    Loading,
    ExpenseAdd,
    ExpenseList,
    RefundAdd,
    ConfirmDialog,
    RefundList,
  },
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
    // enlève la dépense du tableau
    removeExpense(user, expenseId) {
      var expenseIndex = user.expenses.findIndex((e) => e.id === expenseId);
      if (expenseIndex !== -1) {
        user.expenses.splice(expenseIndex, 1);
      }
    },
    // enlève le remboursement de plusieurs tableaux
    removeRefund(user, receiverId, refundId){
      var emitterIndex = user.refunds.findIndex((r) => r.id === refundId);
      if (emitterIndex !== -1) {
        user.refunds.splice(emitterIndex, 1);
      }
      var receiver = this.share?.users?.filter(
        (u) => u.id === receiverId
      )[0];
      var receiverIndex = receiver.refunds.findIndex((r) => r.id === refundId);
      if (receiverIndex !== -1){
        user.refunds.splice(receiverIndex, 1);
      }
    },
    getTotalExpenses() {
      const sum = (a, b) => a + b;
      return this.share.users
        .map((u) => u.expenses.map((e) => e.amount).reduce(sum, 0))
        .reduce(sum, 0);
    },
    getUserTotal(){

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

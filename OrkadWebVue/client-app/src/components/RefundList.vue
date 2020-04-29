<template>
  <v-card outlined tile>
    <v-card-title>
      Remboursements
    </v-card-title>
    <v-card-text>
      <v-list dense>
        <v-list-item v-for="refund in refunds" :key="refund.id">
          <v-list-item-content>
            <v-list-item-title>
              {{ refund.emitterName }}
              <v-icon small color="orange">mdi-arrow-right-bold-circle</v-icon>
              {{ refund.receiverName }}
            </v-list-item-title>
            <v-list-item-subtitle>
              {{ refund.date | moment("DD/MM/YYYY") }}
            </v-list-item-subtitle>
          </v-list-item-content>
          {{ refund.amount }}€
          <v-list-item-action v-if="mine && owner(refund)">
            <v-btn x-small icon @click="deleteRefundConfirm(refund)">
              <v-icon color="red">mdi-delete </v-icon>
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
      @confirm="confirm.action"
    ></confirm-dialog>
  </v-card>
</template>

<script>
import { mapState } from "vuex";
import axios from "axios";

import ConfirmDialog from "@/components/shared/ConfirmDialog.vue";

export default {
  components: { ConfirmDialog },
  props: {
    userId: {
      type: Number,
      required: true,
    },
    shareId: {
      type: Number,
      required: true,
    },
    refunds: {
      type: Array,
      required: true,
    },
  },
  data: () => ({
    mine: false, // détermine si la liste de remboursement appartient a l'utilisateur courant
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
    deleteRefundConfirm(refund) {
      this.confirm.title = "Suppression du remboursement";
      this.confirm.message =
        "Souhaitez vous vraiment supprimer le remboursement pour " +
        refund.receiverName +
        " d'un montant de " +
        refund.amount +
        "€ ?";
      this.confirm.action = () => this.deleteRefund(refund);
      this.confirm.confirmText = "Supprimer";
      this.confirm.cancelText = "Annuler";
      this.$refs["confirm"].show();
    },
    deleteRefund(refund) {
      axios.delete("api/shares/" + this.shareId + "/refunds/" + refund.id).then(() => this.$emit("deleted", refund));
    },
    // vérifie si l'utilisateur courant est bien propriétaire du remboursement
    owner(refund) {
      return refund.emitterId === this.userId;
    },
  },
};
</script>

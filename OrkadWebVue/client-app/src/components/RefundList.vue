<template>
  <v-card outlined tile>
    <v-card-text>
      <v-list dense>
        <v-list-item v-for="operation in operations" :key="operation.id">
          <v-list-item-content>
            <v-list-item-title>
              {{ operation.name }}
            </v-list-item-title>
            <v-list-item-subtitle>
              {{ operation.date | moment("DD/MM/YYYY") }}
            </v-list-item-subtitle>
          </v-list-item-content>
          {{ operation.amount }}€
          <v-list-item-action v-if="mine">
            <v-btn x-small icon>
              <v-icon color="red">mdi-delete </v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script>
import { mapState } from "vuex";

export default {
  props: {
    userId: {
      type: Number,
      required: true,
    },
    shareId: {
      type: Number,
      required: true,
    },
    operations: {
      type: Array,
      required: true,
    },
  },
  data: () => ({
    mine: false, // détermine si la liste de transaction appartient a l'utilisateur courant
    transactions: [], // liste des transaction de l'utilisateur, une transaction peut être une dépense ou un remboursement
  }),
  computed: {
    ...mapState("context", ["profile"]),
  },
  mounted() {
    this.mine = this.profile.id === this.userId.toString()
  },
};
</script>

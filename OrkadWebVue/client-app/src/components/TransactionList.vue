<template>
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
              {{ expense.date | moment("DD/MM/YYYY") }}
            </v-list-item-subtitle>
          </v-list-item-content>

          {{ expense.amount }}€
          <v-list-item-action v-if="owned(user.id)">
            <v-btn x-small icon>
              <v-icon
                color="red"
                @click="deleteExpense(expense.id, expense.name)"
                >mdi-delete
              </v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script>
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
  },
  data: () => ({
    mine: false,
  }),
};
</script>

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
              <v-icon @click="confirmDeleteDialog = true">mdi-delete</v-icon>
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
        <expense-add :share-id="share.id"></expense-add>
      </v-col>
    </v-row>
    <v-row>
      <v-col v-for="user in share.users" :key="user.id" cols="6">
        <v-card outlined tile>
          <v-card-title
            >{{ user.name }}
            <span v-if="owned(user.name)"> (vous)</span>
          </v-card-title>

          <v-card-text>
            <v-list>
              <v-list-item v-for="expense in user.expenses" :key="expense.id">
                <v-list-item-content>
                  <v-list-item-title>{{ expense.name }}</v-list-item-title>
                  <v-list-item-subtitle
                    >{{ expense.amount }}€ ({{
                      expense.date | $moment("DD/MM/YYYY")
                    }})</v-list-item-subtitle
                  >
                </v-list-item-content>
                <v-list-item-icon>
                  <v-icon>chat_bubble</v-icon>
                </v-list-item-icon>
              </v-list-item>
            </v-list>
          </v-card-text>

          <v-card-actions> </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="confirmDeleteDialog" width="500">
      <v-card>
        <v-card-title>Suppression</v-card-title>
        <v-card-text>Souhaitez vous vraiment supprimer le partage ? Il sera impossible de revenir en arrière</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red" @click="deleteShare">Supprimer</v-btn>
          <v-btn @click="confirmDeleteDialog = false">Annuler</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import Loading from "@/components/Loading.vue";
import ExpenseAdd from "@/components/ExpenseAdd.vue";
import axios from "axios";
import { mapState } from "vuex";

export default {
  name: "Share",
  components: { Loading, ExpenseAdd },
  data: () => ({
    loading: true,
    mine: false,
    share: {},
    newExpense: {
      amount: null,
      name: null,
      date: null,
    },
    confirmDeleteDialog: false
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
    deleteShare() {
      this.loading = true;
      var id = this.$route.params.id;
      axios.delete("/api/shares/" + id).then(() =>{
        this.$router.push('/shares');
      }).finally(() => {
        this.loading = false;
      });
    },
    getTotalExpenses() {
      const sum = (a, b) => a + b;
      return this.share.users
        .map((u) => u.expenses.map((e) => e.amount).reduce(sum, 0))
        .reduce(sum, 0);
    },
    owned(name) {
      return name === this.profile.name;
    },
  },
};
</script>

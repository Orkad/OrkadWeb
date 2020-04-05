<template>
  <loading v-if="loading" />
  <div v-else>
    <div>
      <h2>Partage : {{ share.name }}</h2>
      <span>Total des dépenses : {{ getTotalExpenses() }}€</span>

      <div class="md-layout">
        <div class="md-layout-item md-size-50">
          <v-field>
            <label>Montant de la dépense</label>
            <v-input v-model="newExpense.amount"></v-input>
            <md-icon>euro_symbol</md-icon>
          </v-field>
          <md-datepicker v-model="newExpense.date">
            <label>Date de la dépense</label>
          </md-datepicker>
        </div>
        <div class="md-layout-item md-size-50">
          <md-card>
            <md-card-header>
              <span class="md-title">Ajouter une dépense</span>
            </md-card-header>
            <md-card-content>
              <div class="md-layout">
                <div class="md-layout-item md-size-75">
                  <md-field>
                    <label>Nom de la dépense</label>
                    <md-input v-model="newExpense.name"></md-input>
                  </md-field>
                </div>
                <div class="md-layout-item md-size-25">
                  <md-field>
                    <label>Montant</label>
                    <span class="md-prefix">€</span>
                    <md-input v-model="newExpense.amount"></md-input>
                  </md-field>
                </div>
              </div>
              
            </md-card-content>
            <md-card-action>
              <md-button class="md-primary md-raised">Ajouter</md-button>
            </md-card-action>
          </md-card>
        </div>
      </div>
    </div>
    <div class="md-layout">
      <md-card
        v-for="user in share.users"
        :key="user.id"
        class="md-layout-item"
      >
        <md-card-header>
          <span class="md-title">{{ user.name }}</span>
          <span v-if="owned(user.name)"> (vous)</span>
        </md-card-header>

        <md-card-content>
          <md-list>
            <md-list-item v-for="expense in user.expenses" :key="expense.id">
              <md-icon class="md-accent">credit_card</md-icon>
              <div class="md-list-item-text">
                <span>{{ expense.name }}</span>
                <span>01/04/2020</span>
              </div>
              <div class="md-list-item-text">{{ expense.amount }}€</div>
            </md-list-item>
          </md-list>
        </md-card-content>

        <md-card-actions> </md-card-actions>
      </md-card>
    </div>
  </div>
</template>

<script>
import Loading from "@/components/Loading.vue";
import axios from "axios";
import { mapState } from "vuex";

export default {
  name: "Share",
  components: { Loading },
  data: () => ({
    loading: true,
    share: {},
    newExpense: {
      amount: null,
      name: null,
      date: null
    }
  }),
  computed: {
    ...mapState("context", ["profile"])
  },
  created() {
    this.loading = true;
    var id = this.$route.params.id;
    axios.get("/api/shares/" + id).then(r => {
      this.share = r.data;
      this.loading = false;
    });
  },
  methods: {
    getTotalExpenses() {
      const sum = (a, b) => a + b;
      return this.share.users
        .map(u => u.expenses.map(e => e.amount).reduce(sum, 0))
        .reduce(sum, 0);
    },
    owned(name) {
      return name === this.profile.name;
    }
  }
};
</script>

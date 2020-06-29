<template>
  <v-form v-if="!isAuthenticated" :lazy-validation="lazy" @submit.prevent="authenticate">
    <v-text-field
      v-model="form.username"
      :rules="form.usernameRules"
      label="Nom d'utilisateur"
      required
    >
    </v-text-field>
    <v-text-field
      v-model="form.password"
      :rules="form.passwordRules"
      label="Mot de passe"
      type="password"
      required
    ></v-text-field>
    <v-btn :disabled="sending" color="success" class="mr-4" type="submit">
      Se connecter
    </v-btn>
  </v-form>
  <div v-else>
    <div>Connecté en tant que {{ profile.name }}</div>
    <v-btn :disabled="sending" color="error" class="mr-4" @click="disconnect"
      >Se déconnecter
    </v-btn>
  </div>
</template>

<script>
import { mapGetters, mapState, mapActions } from "vuex";

export default {
  name: "Login",
  data: () => ({
    form: {
      username: null,
      usernameRules: [
        v => !!v || 'Le nom d\'utilisateur est requis',
        v => (v && v.length >= 3) || 'Le nom d\'utilisateur doit faire plus de 3 caractères',
      ],
      password: null,
      passwordRules: [
        v => !!v || 'Le mot de passe est requis',
        v => (v && v.length >= 6) || 'Le mot de passe doit faire plus de 6 caractères'
      ]
    },
    connected: false,
    sending: false,
    errorDialog: false,
    errorMessage: "error"
  }),
  computed: {
    ...mapState("context", ["profile"]),
    ...mapGetters("context", ["isAuthenticated"])
  },
  methods: {
    ...mapActions("context", ["login", "logout"]),
    clearForm() {
      this.form.password = null;
    },
    showError(msg) {
      this.errorMessage = msg;
      this.errorDialog = true;
    },
    authenticate() {
      this.sending = true;
      this.login({
        username: this.form.username,
        password: this.form.password
      }).then(data => {
        if (data.error) {
          this.showError(data.error);
        }
        this.sending = false;
      }).error(e => {
        this.showError('impossible de contacter le serveur d\'authentification');
        console.error(e);
      });
    },
    disconnect() {
      this.sending = true;
      this.logout().then(() => {
        this.sending = false;
      });
    }
  }
};
</script>

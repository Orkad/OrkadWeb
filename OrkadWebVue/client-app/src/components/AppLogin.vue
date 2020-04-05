<template>
  <v-app>
    <v-content>
      <v-container class="fill-height" >
        <v-row align="center" justify="center">
          <v-col cols="12" sm="8" md="4">
            <v-form v-model="loginForm"
              @submit.prevent="authenticate"
            >
              <v-card class="elevation-12">
                <v-toolbar color="primary" dark flat>
                  <v-toolbar-title>Connexion</v-toolbar-title>
                </v-toolbar>
                <v-card-text>
                  <v-text-field
                    v-model="form.username"
                    :rules="form.usernameRules"
                    label="Nom d'utilisateur"
                    required
                    prepend-icon="person"
                    type="text"
                  />

                  <v-text-field
                    v-model="form.password"
                    :rules="form.passwordRules"
                    label="Mot de passe"
                    type="password"
                    required
                    prepend-icon="lock"
                  />
                </v-card-text>
                <v-card-actions
                  ><v-spacer />
                  <v-btn
                    :disabled="sending || !loginForm"
                    color="success"
                    class="mr-4"
                    type="submit"
                  >
                    Se connecter
                  </v-btn></v-card-actions
                >
              </v-card>
            </v-form>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
    <v-dialog v-model="errorDialog" persistent max-width="500">
      <v-card>
        <v-card-title color="red darken-1" class="headline">Erreur de connexion</v-card-title>
        <v-card-text>La combinaison nom d'utilisateur / mot de passe fournie semble incorrecte, êtes vous sûr de l'avoir renseignée correctement</v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="red darken-1" text @click="errorDialog = false">Fermer</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-app>
</template>

<script>
import { mapGetters, mapState, mapActions } from "vuex";

export default {
  name: "AppLogin",
  data: () => ({
    loginForm: false,
    form: {
      username: '',
      usernameRules: [
        (v) => !!v || "Le nom d'utilisateur est requis",
        (v) =>
          (v && v.length >= 3) ||
          "Le nom d'utilisateur doit faire plus de 3 caractères",
      ],
      password: '',
      passwordRules: [
        (v) => !!v || "Le mot de passe est requis",
        (v) =>
          (v && v.length >= 6) ||
          "Le mot de passe doit faire plus de 6 caractères",
      ],
    },
    connected: false,
    sending: false,
    errorDialog: false,
    errorMessage: "error",
  }),
  computed: {
    ...mapState("context", ["profile"]),
    ...mapGetters("context", ["isAuthenticated"]),
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
        password: this.form.password,
      }).then((data) => {
        if (data.error) {
          this.showError(data.error);
        }
        this.sending = false;
      });
    },
    disconnect() {
      this.sending = true;
      this.logout().then(() => {
        this.sending = false;
      });
    },
  },
};
</script>

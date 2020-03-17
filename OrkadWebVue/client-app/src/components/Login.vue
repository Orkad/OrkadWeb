<template>
  <div>
    <form novalidate class="md-layout" @submit.prevent="validateUser">
      <md-card class="md-layout-item md-size-50 md-small-size-100">
        <md-card-header>
          <div class="md-title">Connexion</div>
        </md-card-header>

        <md-card-content>
          <md-field :class="getValidationClass('username')">
            <label for="username">Nom d'utilisateur</label>
            <md-input name="username" id="username" v-model="form.username" :disabled="sending" />
            <span
              class="md-error"
              v-if="!$v.form.username.required"
            >Le nom d'utilisateur est obligatoire</span>
            <span
              class="md-error"
              v-else-if="!$v.form.username.minlength"
            >Le nom d'utilisateur est trop court</span>
          </md-field>
          <md-field :class="getValidationClass('password')">
            <label for="password">Mot de passe</label>
            <md-input
              name="password"
              id="password"
              type="password"
              v-model="form.password"
              :disabled="sending"
            />
            <span class="md-error" v-if="!$v.form.password.required">Le mot de passe est obligatoire</span>
            <span
              class="md-error"
              v-else-if="!$v.form.password.minlength"
            >Le mot de passe est trop court</span>
          </md-field>
        </md-card-content>

        <md-progress-bar md-mode="indeterminate" v-if="sending" />

        <md-card-actions>
          <md-button type="submit" class="md-primary" :disabled="sending">Se connecter</md-button>
        </md-card-actions>
      </md-card>

      <md-snackbar :md-active.sync="userSaved">Connecté</md-snackbar>
    </form>
  </div>
</template>

<script>
import { validationMixin } from "vuelidate";
import AccountApi from "../services/api/AccountApi";
import {
  required,
  email,
  minLength,
  maxLength
} from "vuelidate/lib/validators";

export default {
  name: "Login",
  mixins: [validationMixin],
  data: () => ({
    form: {
      username: null,
      password: null
    },
    userSaved: false,
    sending: false
  }),
  validations: {
    form: {
      username: {
        required,
        minLength: minLength(3)
      },
      password: {
        required,
        minLength: minLength(3)
      }
    }
  },
  methods: {
    getValidationClass(fieldName) {
      const field = this.$v.form[fieldName];

      if (field) {
        return {
          "md-invalid": field.$invalid && field.$dirty
        };
      }
    },
    clearForm() {
      this.$v.$reset();
      this.form.username = null;
      this.form.password = null;
    },
    saveUser() {
      this.sending = true;
      AccountApi.login(this.form.username, this.form.password);
      // Instead of this timeout, here you can call your API
      window.setTimeout(() => {
        this.userSaved = true;
        this.sending = false;
        this.clearForm();
      }, 1500);
    },
    validateUser() {
      this.$v.$touch();

      if (!this.$v.$invalid) {
        this.saveUser();
      }
    }
  }
};
</script>

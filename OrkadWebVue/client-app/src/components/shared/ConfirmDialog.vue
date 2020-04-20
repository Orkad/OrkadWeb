<template>
  <v-dialog v-model="toogle" width="500">
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>{{ message }}</v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="primary" depressed @click="confirm">{{
          confirmText
        }}</v-btn>
        <v-btn depressed @click="hide">{{ cancelText }}</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "ConfirmDialog",
  props: {
    title: {
      type: String,
      required: false,
      default: "Confirmation",
    },
    message: {
      type: String,
      required: false,
      default: "En êtes vous sûr ?",
    },
    confirmText: {
      type: String,
      required: false,
      default: "Confirmer",
    },
    cancelText: {
      type: String,
      required: false,
      default: "Annuler",
    },
  },
  data: () => ({
    toogle: false,
    action: () => {},
  }),
  methods: {
    confirm() {
      this.action();
      this.$emit("confirm");
      this.hide();
    },
    show(title, message, action, confirmText, cancelText) {
      this.toogle = true;
      if (title) {
        this.title = title;
      }
      if (message) {
        this.message = message;
      }
      if (action) {
        this.action = action;
      }
      if (confirmText) {
        this.confirmText = confirmText;
      }
      if (cancelText) {
        this.cancelText = cancelText;
      }
    },
    hide() {
      this.toogle = false;
    },
  },
};
</script>

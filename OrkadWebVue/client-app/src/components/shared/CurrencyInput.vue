<template>
  <v-text-field
    v-model="displayValue"
    :rules="rules"
    label="Montant"
    type="text"
    class="right-input"
    suffix="€"
    @blur="focusOut"
    @focus="focusIn"
    @input="input"
  >
  </v-text-field>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true,
      default: "Montant",
    },
    rules: {
      type: Array,
      required: false,
      default: function () {
        return [];
      },
    },
  },
  data: () => ({
    currencyValue: 0,
    displayValue: "",
  }),
  methods: {
    computeCurrency() {
      // Recalculate the currencyValue after ignoring "$" and "," in user input
      this.currencyValue = parseFloat(this.displayValue.replace(/[^\d.]/g, ""));
      // Ensure that it is not NaN. If so, initialize it to zero.
      // This happens if user provides a blank input or non-numeric input like "abc"
      if (isNaN(this.currencyValue)) {
        this.currencyValue = 0;
      }
    },
    input() {
      if (this.displayValue) {
        this.displayValue = this.displayValue
          .replace(",", ".")
          .replace("€", "");
        this.computeCurrency();
      }
      this.$emit("input", this.currencyValue);
    },
    focusOut() {
      this.computeCurrency();
      if (this.currencyValue === 0) {
        this.displayValue = "";
        return;
      }
      // Format display value based on calculated currencyValue
      this.displayValue = this.currencyValue
        .toFixed(2)
        .replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1,");
    },
    focusIn() {
      if (this.currencyValue === 0) {
        this.displayValue = "";
        return;
      }
      // Unformat display value before user starts modifying it
      this.displayValue = this.currencyValue.toString();
    },
  },
};
</script>

<style></style>

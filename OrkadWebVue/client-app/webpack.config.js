// enable vue in eslint

module.exports = {
  devServer: {
    public: "0.0.0.0",
    host: "0.0.0.0",
    disableHostCheck: true,
  },
  parser: "vue-eslint-parser",
  parserOptions: {
    parser: "babel-eslint",
  },
  plugins: ["vue"],
  extends: [
    "eslint:recommended",
    "plugin:vue/essential",
    "plugin:prettier/recommended",
  ],
};

<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <Loading v-if="loading" :spin="true" />
    <ul v-else class="collection">
      <li
        v-for="item in items"
        :key="item.date"
        class="collection-item"
      >{{ item.date }} / {{ item.temperatureC }}C° / {{ item.temperatureF}}F° / {{ item.summary }}</li>
    </ul>
  </div>
</template>

<script>
import WeatherForecastApi from "@/services/api/WeatherForecastApi";
import Loading from "./Loading";
export default {
  name: "HelloWorld",
  components: { Loading },
  props: {
    msg: String
  },
  data() {
    return {
      loading: true,
      items: []
    };
  },
  created() {
    WeatherForecastApi.get()
      .then(d => {
        this.items = d;
      })
      .finally(() => {
        this.loading = false;
      });
  }
};
</script>

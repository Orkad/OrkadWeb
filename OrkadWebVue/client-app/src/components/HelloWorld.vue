<template>
  <div class="hello">
    <Loading v-if="loading" :spin="true" />
    <ul v-else class="collection">
      <li
        v-for="item in items"
        :key="item.date"
        class="collection-item"
      >{{ item.date }} / {{ item.temperatureC }}C° / {{ item.temperatureF}}F° / {{ item.summary }}</li>
    </ul>
    <span>Temperature : {{temperature}}C°</span>
  </div>
</template>

<script>
import WeatherForecastApi from "@/services/api/WeatherForecastApi";
import Loading from "./Loading";
import Axios from 'axios';

export default {
  name: "HelloWorld",
  components: { Loading },
  data: () => ({
    temperature: null,
    loading: true,
    items: []
  }),
  created() {
    WeatherForecastApi.get()
      .then(d => {
        this.items = d;
      })
      .finally(() => {
        this.loading = false;
      });
    Axios.get('api/supervision/cpu/temp')
    .then((res) => this.temperature = res.data);
  }
};
</script>

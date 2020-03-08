<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <div class="loading" v-if="loading">chargement</div>
    <ul v-for="item in items" :key="item.date">
      <li>{{ item.date }} / {{ item.temperatureC }}C° / {{ item.temperatureF}}F° / {{ item.summary }} </li>
    </ul>
  </div>
</template>

<script>
  import WeatherForecastApi from '@/services/api/WeatherForecastApi'
  export default {
    name: 'HelloWorld',
    props: {
      msg: String
    },
    data() {
      return {
        loading: true,
        items: [],
      }
    },
    created() {
      WeatherForecastApi.get()
        .then(d => { this.items = d })
        .finally(() => { this.loading = false })
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
  h3 {
    margin: 40px 0 0;
  }

  ul {
    list-style-type: none;
    padding: 0;
  }

  li {
    display: inline-block;
    margin: 0 10px;
  }

  a {
    color: #42b983;
  }
</style>

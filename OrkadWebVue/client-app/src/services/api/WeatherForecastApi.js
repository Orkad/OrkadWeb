import axios from 'axios';

export default {
  get() {
    return axios.get("/weatherforecast").then(r => r.data);
  }
}
import axios from 'axios';

export default {
  getShares() { return axios.get("/api/share").then(r => r.data); },
}
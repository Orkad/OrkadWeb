import axios from "axios";

export default {
  getShares() {
    return axios.get("/api/shares").then(r => r.data);
  },
};

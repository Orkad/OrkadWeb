import axios from 'axios';

export default {
  refresh() {
    return axios.get("/account/context")
    .then(r => r.data);
  },

  login(username, password) {
    return axios.post("/account/login", {
      username: username,
      password: password
    }).then(r => r.data);
  },

  logout(){
    return axios.post("/account/logout");
  }
}
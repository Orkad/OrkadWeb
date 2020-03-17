import axios from 'axios';

export default {
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
import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import Login from '@/components/Login'
import ShareList from '@/components/ShareList'
import Share from '@/components/Share'
import ShareAdd from '@/components/ShareAdd'

Vue.use(Router)

export default new Router({
  routes: [
    { path: '/', component: HelloWorld },
    { path: '/login', component: Login },
    { path: '/shares', component: ShareList},
    { path: '/shares/:id', component: Share},
    { path: '/create/share', component: ShareAdd},
  ]
})
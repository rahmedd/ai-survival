import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { VueSignalR } from '@dreamonkey/vue-signalr'
import { HubConnectionBuilder } from '@microsoft/signalr'

import App from './App.vue'
import router from './router'

const connection = new HubConnectionBuilder().withUrl('/api/hub').build()

const app = createApp(App)

app.use(VueSignalR, {
	connection,
	failFn: () => {
		console.log('signalr conn failed.')
	},
})
app.use(createPinia())
app.use(router)

app.mount('#app')

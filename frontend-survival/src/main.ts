import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { VueSignalR } from '@dreamonkey/vue-signalr'
import { HubConnectionBuilder } from '@microsoft/signalr'

import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';

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

app.use(PrimeVue, {
	theme: {
		preset: Aura,
	},
});

app.component('Button', Button);
app.component('InputText', InputText);
app.component('FloatLabel', FloatLabel);

app.mount('#app')

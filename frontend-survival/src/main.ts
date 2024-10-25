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
import Chip from 'primevue/chip';
import Avatar from 'primevue/avatar';
import ProgressBar from 'primevue/progressbar';
import ProgressSpinner from "primevue/progressspinner";
import Textarea from "primevue/textarea";
import Card from "primevue/card";

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

app.component('Button', Button)
app.component('InputText', InputText)
app.component('FloatLabel', FloatLabel)
app.component('Chip', Chip)
app.component('Avatar', Avatar)
app.component('ProgressBar', ProgressBar)
app.component('ProgressSpinner', ProgressSpinner)
app.component('Textarea', Textarea)
app.component('Card', Card)

app.mount('#app')

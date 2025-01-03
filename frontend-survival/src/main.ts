/* eslint-disable vue/no-reserved-component-names */

import './styles/main.scss'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { VueSignalR } from '@dreamonkey/vue-signalr'
import { HubConnectionBuilder } from '@microsoft/signalr'

import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura'
import { Form, FormField } from '@primevue/forms'
import { updatePrimaryPalette } from '@primevue/themes'
import ToastService from 'primevue/toastservice'
import {
	Button,
	InputText,
	FloatLabel,
	Chip,
	Avatar,
	ProgressBar,
	ProgressSpinner,
	Textarea,
	Card,
	Message,
	Toast
} from 'primevue'


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
		prefix: 'p',
		preset: Aura,
		order: 'primevue',
		options: {
			darkModeSelector: false,
			cssLayer: {
				name: 'primevue',
				order: 'tailwind-base, primevue, tailwind-utilities',
			}
		}
	}
})

app.use(ToastService)

updatePrimaryPalette({
	50: '{indigo.50}',
	100: '{indigo.100}',
	200: '{indigo.200}',
	300: '{indigo.300}',
	400: '{indigo.400}',
	500: '{indigo.500}',
	600: '{indigo.600}',
	700: '{indigo.700}',
	800: '{indigo.800}',
	900: '{indigo.900}',
	950: '{indigo.950}'
})

app.component('Button', Button)
app.component('InputText', InputText)
app.component('FloatLabel', FloatLabel)
app.component('Chip', Chip)
app.component('Avatar', Avatar)
app.component('ProgressBar', ProgressBar)
app.component('ProgressSpinner', ProgressSpinner)
app.component('Textarea', Textarea)
app.component('Card', Card)
app.component('Message', Message)
app.component('Form', Form)
app.component('FormField', FormField)
app.component('Toast', Toast)

app.mount('#app')

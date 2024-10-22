<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import axios from 'axios';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Button from 'primevue/button';
import Aura from '@primevue/themes/aura';

const signalr = useSignalR()

// signalr.on('MessageReceived', message => {
// 	console.log(`MessageReceived: ${message}`)
// })

signalr.on('Send', message => {
	console.log(`Send: ${message}`)
})



async function send() {
	console.log('trying to send')
	try {
		const username = new Date().getTime()

		const res = await signalr.invoke(
			'newMessage',
			username,
			'this is my message to earth',
		)
		// const res = await signalr.invoke('newMessage', {
		// 	username: username,
		// 	message: 'this is my message to earth',
		// })
	}
	catch (ex) {
		console.log(ex)
	}
}



const roomCode = '';
const username = '';

async function createGame() {
	console.log(username);
}
function joinGame() {
// async function joinGame(roomCode: string, username: string) { 
	// try {
	// 	// await axios.post('/api/Room/CreateGame')
	// 	try {
	// 	// const username = new Date().getTime()

	// 		const res = await signalr.invoke(
	// 			'addToGroup',
	// 			'abc-def-ghi',
	// 		)

	// 		console.log(res)
	// 	}
	// 	catch (ex) {
	// 		console.log(ex)
	// 	}
	// }
	// catch (ex) {
	// 	console.log(ex)
	// }
	
	console.log(roomCode + username);
}
</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
		<Button label="Send Msg"></Button>
	</header>
	<main>
		<div>
			<FloatLabel variant="on">
				<InputText id="on_label" v-model="username" required />
				<label for="on_label">Username</label>
			</FloatLabel>
			<FloatLabel variant="on">
				<InputText id="on_label" v-model="roomCode" />
				<label for="on_label">Room Code</label>
			</FloatLabel>
			<Button label="Create Game" @click="createGame"></Button>
			<Button label="Join Game" @click="joinGame()"></Button>
			<p>{{username}}</p>
		</div>
	</main>

</template>

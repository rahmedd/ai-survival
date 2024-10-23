<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Button from 'primevue/button';
import { ref } from 'vue';

const signalr = useSignalR()

// signalr.on('MessageReceived', message => {
// 	console.log(`MessageReceived: ${message}`)
// })

signalr.on('Send', message => {
	console.log(`Send: ${message}`)
})

const roomCode = ref('');
const username = ref('');

function createGame() {
	console.log(username.value);
}

async function send() {
	console.log('trying to send')
	try {
		const username = new Date().getTime()

		const res = await signalr.invoke(
			'SendMessageToGroup',
			'abc-def-ghi',
			'this is my message to earth',
		)

	} catch (ex) {
		console.log(ex)
	}
}

async function joinGame() {
	try {
		const res = await signalr.invoke(
			'addToGroup',
			'abc-def-ghi',
			`user-${new Date().toISOString()}`
		)
	}
	catch (ex) {
		console.log(ex)
	}
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

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

function joinGame() {	
	console.log(roomCode.value + username.value);
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

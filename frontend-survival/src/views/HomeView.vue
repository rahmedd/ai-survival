<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Button from 'primevue/button';
import { ref } from 'vue';

const signalr = useSignalR()

signalr.on('Send', message => {
	console.log(`Send: ${JSON.stringify(message)}`)
})

const roomCode = ref('');
const username = ref('');

async function createGame() {
	await joinGame()
}

signalr.on('JSON-room', message => {
	console.log(JSON.parse(message))
})

signalr.on('JSON-timer-start', message => {
	console.log(message)
})

signalr.on('JSON-timer-end', message => {
	console.log(message)
})

async function send() {
	console.log('trying to send')
	try {
		const username = new Date().getTime()

		const res = await signalr.invoke(
			'SendMessageToGroup',
			roomName.value,
			'this is my message to earth',
		)

	}
	catch (ex) {
		console.log(ex)
	}
}

async function createGame() {
	try {
		const res = await signalr.invoke(
			'createRoom',
			roomName.value,
			username.value,
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

async function joinGame() {
	try {
		const res = await signalr.invoke(
			'joinRoom',
			roomName.value,
			username.value,
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

async function startGame() {
	try {
		const res = await signalr.invoke(
			'startGameLoop',
			10,
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

async function stopGame() {
	try {
		const res = await signalr.invoke(
			'stopGameLoop',
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

// DEV: REMOVE THIS
async function getRoom() {
	try {
		const res = await signalr.invoke(
			'getRoom',
			roomName.value,
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

// TODO: REMOVE
</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
		<Button label="Send Msg" @click="send"></Button>
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
			<Button label="Join Game" @click="joinGame"></Button>
		</div>
		<a href="lobby">LobbyView</a>
		<a href="prompt">PromptView</a>
		<a href="answer">AnswerView</a>
		<a href="judge">JudgeView</a>
		<a href="leaderboard">LeaderboardView</a>
	</main>
</template>

<style lang="scss" scoped>
.btn-container {
	button {
		padding: 10px;
		margin: 10px;
	}
}
</style>

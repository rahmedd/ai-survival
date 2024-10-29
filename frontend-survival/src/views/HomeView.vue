<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import axios from 'axios'
import { ref } from 'vue'

const signalr = useSignalR()

// signalr.on('MessageReceived', message => {
// 	console.log(`MessageReceived: ${message}`)
// })

const roomName = ref('abc-def-ghi')
const username = ref(`user-${new Date().toISOString()}`)

signalr.on('Send', message => {
	console.log(`Send: ${JSON.stringify(message)}`)
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
	<div>
		<div class="input-container">
			<label>Room name</label>
			<input v-model="roomName"></input>
			<label>Username</label>
			<input v-model="username"></input>
		</div>
		<div class="btn-container">
			<button @click="send">send msg</button>
			<button @click="getRoom">Get</button>
			<button @click="createGame">Create</button>
			<button @click="joinGame">Join</button>
		</div>
	</div>
</template>

<style lang="scss" scoped>
.btn-container {
	button {
		padding: 10px;
		margin: 10px;
	}
}

.input-container {
	display: flex;
	flex-direction: column;
	input {
		margin-bottom: 10px;
	}
}
</style>

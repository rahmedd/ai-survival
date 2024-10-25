<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import axios from 'axios';

const signalr = useSignalR()

// signalr.on('MessageReceived', message => {
// 	console.log(`MessageReceived: ${message}`)
// })

signalr.on('Send', message => {
	console.log(`Send: ${JSON.stringify(message)}`)
})

async function send() {
	console.log('trying to send')
	try {
		const username = new Date().getTime()

		const res = await signalr.invoke(
			'SendMessageToGroup',
			'abc-def-ghi',
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
			'abc-def-ghi',
			`user-${new Date().toISOString()}`
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
			'abc-def-ghi',
			`user-${new Date().toISOString()}`
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
			'abc-def-ghi',
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

// TODO: REMOVE
</script>

<template>
	<div class="btn-container">
		<button @click="send">send msg</button>
		<button @click="getRoom">Get</button>
		<button @click="createGame">Create</button>
		<button @click="joinGame">Join</button>
	</div>
</template>

<style lang="scss" scoped>
.btn-container {
	button {
		padding: 10px;
		margin: 10px;
	}
}
</style>

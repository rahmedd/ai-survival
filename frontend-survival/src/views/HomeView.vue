<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import axios from 'axios';

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
	} catch (ex) {
		console.log(ex)
	}
}

async function joinGame() { 
	try {
		// await axios.post('/api/Room/CreateGame')
		try {
		// const username = new Date().getTime()

		const res = await signalr.invoke(
			'addToGroup',
			'abc-def-ghi',
		)

		console.log(res)
	} catch (ex) {
		console.log(ex)
	}
	}
	catch (ex) {
		console.log(ex)
	}
}
</script>

<template>
	<main>
		<button @click="send">send msg</button>
		<button @click="createGame">create game</button>
		<button @click="joinGame">join game</button>
	</main>
</template>

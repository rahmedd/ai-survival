<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'

const signalr = useSignalR()

signalr.on('MessageReceived', message => {
	console.log(message)
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

		console.log(res)
	} catch (ex) {
		console.log(ex)
	}
}
</script>

<template>
	<main>
		<button @click="send">send msg</button>
	</main>
</template>

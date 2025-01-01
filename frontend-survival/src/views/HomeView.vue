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
			roomCode.value,
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
			roomCode.value,
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
			roomCode.value,
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

async function getRoom() {
	try {
		const res = await signalr.invoke(
			'getRoom',
			roomCode.value,
		)
	}
	catch (ex) {
		console.log(ex)
	}
}

</script>

<template>
	<div class="home-view">
		<div class="hero-horizontal">
			<div class="hero-vertical">
				<h1>SignalR Game</h1>
				<Card>
					<template #content>
						<div class="home-form">
							<div class="home-fields">
								<InputText v-model="roomCode" placeholder="Room Code" />
								<InputText v-model="username" placeholder="Username" />
							</div>
							<div class="home-buttons">
								<Button label="Create Game" @click="createGame" />
								<span class="or"> or </span>
								<Button label="Join Game" @click="joinGame" />
							</div>					
						</div>
					</template>
				</Card>
			</div>
		</div>
	</div>
</template>

<style lang="scss" scoped>
.hero-horizontal {
	display: flex;
	justify-content: center;
	align-items: center;
	margin: 0 auto;

	h1 {
		font-size: 3.5rem;
		background: -webkit-linear-gradient(#977ffd, #333);
		-webkit-background-clip: text;
		-webkit-text-fill-color: transparent;
		font-weight: bold;
	}
}

.hero-vertical {
	// display: flex;
	// flex-direction: row;
}

.home-form {
	display: flex;
	flex-direction: column;
	margin: 1rem 0;

	> * {
		margin: 0.5rem 0;
	}
}

.home-fields {
	display: flex;
	flex-direction: column;
	margin: 1rem 0 1rem 0;

	> * {
		margin: 0.5rem 0;
	}
}

.home-buttons {
	width: 100%;
	display: flex;
	justify-content: space-evenly;
	align-items: center;
}

.or {
	font-size: 1.25rem;
	font-weight: bold;
	color: var(--p-	text-color);
}

</style>

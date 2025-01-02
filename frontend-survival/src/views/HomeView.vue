<script setup lang="ts">
import { useSignalR } from '@dreamonkey/vue-signalr'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import { reactive, ref } from 'vue'
import { valibotResolver } from '@primevue/forms/resolvers/valibot'
import * as v from 'valibot'

const signalr = useSignalR()

signalr.on('Send', message => {
	console.log(`Send: ${JSON.stringify(message)}`)
})

signalr.on('JSON-room', message => {
	console.log(JSON.parse(message))
})

signalr.on('JSON-timer-start', message => {
	console.log(message)
})

signalr.on('JSON-timer-end', message => {
	console.log(message)
})

const resolver = valibotResolver(
	v.object({
		roomCode: v.pipe(v.string(), v.minLength(5, 'Room code is required')),
		nickname: v.pipe(v.string(), v.minLength(5, 'Nickname is required')),
	})
)

const initialValues = reactive({
	// roomCode: 'abc-123',
	// nickname: 'bob',
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
						<Form
							:initialValues
							:resolver
							@submit="onFormSubmit"
						>
							<FormField v-slot="$field" name="roomCode">
								<label>Room code</label>
								<InputText type="text" placeholder="Room code" />
								<Message v-if="$field?.invalid"
									severity="error"
									size="small"
									variant="simple"
								>
									{{ $field.error?.message }}
								</Message>
							</FormField>

							<FormField v-slot="$field" name="nickname">
								<label>Nickname</label>
								<InputText type="text" placeholder="Nickname" />
								<Message v-if="$field?.invalid"
									severity="error"
									size="small"
									variant="simple"
								>
									{{ $field.error?.message }}
								</Message>
							</FormField>

							<div class="home-buttons">
								<Button type="submit" severity="secondary" label="Join" />
								<div>&nbsp;</div>
								<Button type="submit" severity="secondary" label="Create" />
							</div>
						</Form>
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

// .home-form {
// 	display: flex;
// 	flex-direction: column;
// 	margin: 1rem 0;

// 	> * {
// 		margin: 0.5rem 0;
// 	}
// }

// .home-fields {
// 	display: flex;
// 	flex-direction: column;
// 	margin: 1rem 0 1rem 0;

// 	> * {
// 		margin: 0.5rem 0;
// 	}
// }

input {
	width: 100%;
}

.p-button {
	width: 100%;
}

.home-buttons {
	width: 100%;
	display: flex;
	justify-content: space-between;
	align-items: center;
}

.or {
	font-size: 1.25rem;
	font-weight: bold;
	color: var(--p-	text-color);
}

</style>

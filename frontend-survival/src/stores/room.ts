import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useSignalR } from '@dreamonkey/vue-signalr'

export const useRoom = defineStore('room', () => {
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

	async function createGame(roomCode: string, nickname: string) {
		try {
			const res = await signalr.invoke(
				'createRoom',
				nickname,
				roomCode,
			)

			console.log(res)
		}
		catch (ex) {
			console.log(ex)
		}
	}

	async function joinGame(roomCode: string, nickname: string) {
		try {
			const res = await signalr.invoke(
				'createRoom',
				nickname,
				roomCode,
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

	return {
		createGame,
		joinGame,
	}
})

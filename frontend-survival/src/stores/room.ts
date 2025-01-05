import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useSignalR } from '@dreamonkey/vue-signalr'

import type { Room } from '@/types/Room'

export const useRoom = defineStore('room', () => {
	const signalr = useSignalR()
	const roomState = ref<Room | null>(null)

	signalr.on('Send', message => {
		console.log(`Send: ${JSON.stringify(message)}`)
	})

	signalr.on('JSON-room', message => {
		const res = JSON.parse(message)
		console.log(res)
		roomState.value = res
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

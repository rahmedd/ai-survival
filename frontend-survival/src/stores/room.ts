import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { useSignalR } from '@dreamonkey/vue-signalr'

export const useRoom = defineStore('room', () => {
	const signalr = useSignalR()

	signalr.on('Send', message => {
		console.log(`Send: ${JSON.stringify(message)}`)
	})

	const count = ref(0)
	const doubleCount = computed(() => count.value * 2)
	function increment() {
		count.value++
	}

	return { count, doubleCount, increment }
})

<script setup lang="ts">
import type { Player } from '@/models/player'
import Chip from 'primevue/chip'
import Avatar from 'primevue/avatar'
import ProgressBar from 'primevue/progressbar'
import { ref } from 'vue'
import router from '@/router'

const players = ref<Player[]>([
	{
		Id: 'njkasdcjknasdjkhnsd',
		Username: 'number:😊',
		Health: 5,
		Host: true,
	},
	{
		Id: 'nikowerfiojwerfijwefjinop',
		Username: 'raed:😂',
		Health: 5,
		Host: false,
	},
	{
		Id: 'efjiwefiewfipj',
		Username: 'a:😍',
		Health: 5,
		Host: false,
	},
	{
		Id: 'weqfniopjkwnejiompqrkfwef',
		Username: 'willy:❤️',
		Health: 5,
		Host: false,
	},
	{
		Id: 'qweijeqwjipknfqeipwn',
		Username: 'yum:😋',
		Health: 5,
		Host: false,
	},
])

const currentPlayer = ref(players.value[0])
const isHost = ref(currentPlayer.value.Host)

// TODO: REMOVE
function changeHost() {
	isHost.value = !isHost.value
}

function splitUsername(player: Player) {
	return {
		name: player.Username.split(':')[0],
		emoji: player.Username.split(':')[1] || '',
	}
}

// TODO: Add logic to wait for everyone to be ready to move to prompt
// TODO: Add logic so on the last round it goes back to home
async function allReady() {

	await router.push('prompt')
}

</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
	</header>
	<main>
		<h1>Leaderboad</h1>
		<p>test</p>
		<ul>
			<li v-for="player in players" :key="player.Id">
				<Avatar :label="splitUsername(player).emoji" size="xlarge" />
				<Chip :label="splitUsername(player).name" />
				<p>Lives</p> <!-- TODO: Make life counter here -->
			</li>
		</ul>
		<div v-if="isHost">
			<Button label="Start Game" @click="startGame"></Button>
		</div>
		<template v-else>
			<h2>Waiting for host</h2>
			<ProgressBar mode="indeterminate" style="height: 6px"></ProgressBar>
		</template>
		<Button label="Change host" @click="changeHost"></Button> <!-- TODO Remove -->
	</main>
</template>

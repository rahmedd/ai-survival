<script setup lang="ts">
import type { Player } from '@/models/player'
import Chip from 'primevue/chip'
import Avatar from 'primevue/avatar'
import ProgressBar from 'primevue/progressbar'
import ProgressSpinner from 'primevue/progressspinner'
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

// TODO: Add logic to get all players to move on to answer page at once
async function startGame() {

	await router.push('answer')
}

</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
	</header>
	<main>
		<h1>Prompt</h1>
		<div v-if="isHost">
			<Button label="Start Game" @click="startGame"></Button>
		</div>
		<template v-else>
			<h2>Waiting for host</h2>
			<ProgressBar mode="indeterminate" style="height: 6px"></ProgressBar>
		</template>
		<Button label="Change host" @click="changeHost"></Button> <!-- TODO Remove -->
		<div class="sidebar">
			<ul>
				<li v-for="player in players" :key="player.Id">
					<Avatar :label="splitUsername(player).emoji" size="xlarge" />
					<Chip :label="splitUsername(player).name" />
					<ProgressSpinner style="width: 50px; height: 50px"
						strokeWidth="8"
						fill="transparent"
						animationDuration=".5s"
						aria-label="Custom ProgressSpinner" />
				</li>
			</ul>
		</div>
	</main>
</template>

<style>
.sidebar {
	height: 100%; /* Full-height: remove this if you want "auto" height */
	width: fit-content; /* Set the width of the sidebar */
	position: fixed; /* Fixed Sidebar (stay in place on scroll) */
	z-index: 1; /* Stay on top */
	top: 0; /* Stay at the top */
	right: 0;
	background-color: #111; /* Black */
	overflow-x: hidden; /* Disable horizontal scroll */
	padding-top: 20px;
}

ul {
	list-style-type: none;
	padding: 0;
	margin: 0;
}

</style>

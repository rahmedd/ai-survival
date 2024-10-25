<script setup lang="ts">
import type { Player } from '@/models/player';
import Chip from 'primevue/chip';
import Avatar from 'primevue/avatar';
import ProgressBar from 'primevue/progressbar';
import ProgressSpinner from "primevue/progressspinner";
import Card from "primevue/card";
import { ref } from 'vue';
import router from "@/router";

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
]);
const prompt = "A very spooky prompt"
const answer = ref("sdjfsdjnfsjk")


const currentPlayer = ref(players.value[0])
const isHost = ref(currentPlayer.value.Host)

function splitUsername(player: Player) {
	return {
		name: player.Username.split(':')[0],
		emoji: player.Username.split(':')[1] || "",
	}
}

const currentUsername = ref(splitUsername(currentPlayer.value))

// TODO: Add logic to wait for everyone to be ready to move to leaderboard
function allReady() {
	router.push('leaderboard')
}
</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
	</header>
	<main>
		<h1>Judging</h1>
		<h2>{{prompt}}</h2>
		<Card style="width: 25rem; overflow: hidden">
			<template #header>{{currentUsername.emoji}}</template>
			<template #title>{{ currentUsername.name }}</template>
			<template #subtitle>{{currentUsername.name}} tried to:</template>
			<template #content>
				<p class="m-0">
					{{ answer }}
				</p>
			</template>
			<template #footer>
				<div class="flex gap-4 mt-1">
					<Button label="Previous" class="w-full" />
					<Button label="Next" class="w-full" />
				</div>
			</template>
		</Card>
		<div>
			<h2>Waiting for all Players to ready</h2>
			<ProgressBar mode="indeterminate" style="height: 6px"></ProgressBar>
		</div>
		<div class="sidebar">
			<ul>
				<li v-for="player in players" :key="player.Id">
					<Avatar :label="splitUsername(player).emoji" size="xlarge" />
					<Chip :label="splitUsername(player).name" />
					<ProgressSpinner style="width: 50px; height: 50px" strokeWidth="8" fill="transparent" animationDuration=".5s" aria-label="Custom ProgressSpinner" />
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

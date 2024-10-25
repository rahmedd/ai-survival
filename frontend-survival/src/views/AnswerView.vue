<script setup lang="ts">
import type { Player } from '@/models/player';
import Chip from 'primevue/chip';
import Avatar from 'primevue/avatar';
import ProgressBar from 'primevue/progressbar';
import ProgressSpinner from "primevue/progressspinner";
import Textarea from "primevue/textarea";
import FloatLabel from "primevue/floatlabel";
import { ref, onMounted, onUpdated } from 'vue';
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
const answer = ref("")


const currentPlayer = ref(players.value[0])
const isHost = ref(currentPlayer.value.Host)

function splitUsername(player: Player) {
	return {
		name: player.Username.split(':')[0],
		emoji: player.Username.split(':')[1] || "",
	}
}

const currentUsername = ref(splitUsername(currentPlayer.value))
const timeLeft = ref(60)

function countDown() {
	setInterval(() => {
		timeLeft.value--;
	}, 1000)
}

onMounted(() => countDown())
onUpdated(() => submitAnswers())

// TODO: Add logic to submit all answers
function submitAnswers() {
	if(timeLeft.value <= 0){
		router.push('judge')
	}
}

//TODO: Remove variable once solution is found
const placeholder = ""
</script>

<template>
	<header>
		<h1>Ai Danger Game</h1>
	</header>
	<main>
		<h1>{{prompt}}</h1>
		<FloatLabel>
			<Textarea id="over_label" v-model="answer" autoResize rows="5" cols="30" />
			<label for="over_label">{{ currentUsername.name }} will try to: </label>
		</FloatLabel>
		<div>
			<h2>Time Left: {{timeLeft}} seconds</h2>
			<ProgressBar :value="timeLeft/.6" style="height: 20px">{{placeholder}}</ProgressBar> <!-- TODO: Figure out how to make text field blank without this variable being needed -->
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

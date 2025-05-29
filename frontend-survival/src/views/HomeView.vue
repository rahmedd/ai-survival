<script setup lang="ts">
// external libs
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { faker } from '@faker-js/faker/locale/en_US'

// primevue
import type { SelectButtonChangeEvent } from 'primevue/selectbutton'

// composables
import { useRoom } from '@/stores/room'

// types

// components
import RoomForm	from '@/components/RoomForm.vue'
import type { JoinCreateRoom } from '@/types/JoinCreateRoom'


const { createGame, joinGame } = useRoom()
const router = useRouter()


const buttons = ref(['Join', 'Create'])
const selectedButton = ref(buttons.value[0])

async function onSubmit(e: JoinCreateRoom) {
	let res: boolean = false

	if (selectedButton.value === 'Join') {
		res = await joinGame(e.roomCode, e.nickname)
	}
	else if (selectedButton.value === 'Create') {
		res = await createGame(e.roomCode, e.nickname)
	}

	if (res) {
		router.push({ name: 'lobby' })
	}
	else {
		console.error('Failed to join or create game')
	}
}

function onCreateSelection(e: SelectButtonChangeEvent) {
	if (e.value === 'Join') {
		
	}
	else if (e.value === 'Create') {
		
	}
}

function generateRandomRoom(): string {
	const list = [
		faker.animal.type(),
		faker.company.buzzVerb(),
		// faker.company.buzzNoun(),
		// faker.food.fruit(),
		// faker.food.vegetable(),
		faker.number.int({ min: 0, max: 99 }),
	]

	return list.join(' ').replace(/\s+/g, '-')
}

function generateRandomUsername(): string {
	const list = [
		faker.food.fruit(),
		faker.food.vegetable(),
	]

	let ret = ''

	list.forEach(i => {
		const temp = i.split(' ').map(word => word.charAt(0).toUpperCase() + word.slice(1)).join('')
		ret += temp
	})

	return ret
}

</script>

<template>
	<div class="home-view">
		<div class="hero-horizontal">
			<div class="hero-vertical">
				<h1>SignalR Game</h1>
				<Card>
					<template #content>
						<SelectButton
							class="w-full w-full-button"
							v-model="selectedButton"
							:options="buttons"
							size="large"
							:allowEmpty="false"
							@change="onCreateSelection"
						/>
						<template v-if="selectedButton === 'Join'">
							<RoomForm
								mode="Join"
								:nickname="generateRandomUsername()"
								:roomCode="``"
								@submit="onSubmit"
							></RoomForm>
						</template>
						<template v-else-if="selectedButton === 'Create'">
							<RoomForm
								mode="Create"
								:nickname="generateRandomUsername()"
								:roomCode="generateRandomRoom()"
								@submit="onSubmit"
							></RoomForm>
						</template>
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

.p-selectbutton {
	margin-bottom: 16px;
}

.home-buttons {
	width: 100%;
	display: flex;
	justify-content: space-between;
	align-items: center;
}
</style>

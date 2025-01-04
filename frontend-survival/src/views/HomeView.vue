<script setup lang="ts">
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import { reactive } from 'vue'
import { valibotResolver } from '@primevue/forms/resolvers/valibot'
import * as v from 'valibot'
import { useEagerValidation } from '@/composables/useEagerValidation'
import type { GenericFormFieldState } from '@/types/GenericFormFieldState'
import { useRoom } from '@/stores/room'

const { getFieldFromDirtyMap, updateDirtyMap } = useEagerValidation()
const { createGame, joinGame } = useRoom()

const homeSchema = 	v.object({
	nickname: v.pipe(v.string(), v.minLength(5, 'Minimum of 5 characters')),
	roomCode: v.pipe(v.string(), v.minLength(5, 'Minimum of 5 characters')),
})

const resolver = valibotResolver(homeSchema)

type HomeForm = v.InferOutput<typeof homeSchema>
type HomeFormState = GenericFormFieldState<HomeForm>

const initialValues = reactive<HomeForm>({
	// nickname: 'bob',
	// roomCode: 'abc-123',
	nickname: '',
	roomCode: '',
})

async function callCreateGame(e: HomeFormState) {
	debugger
	if (!e.valid) return
	await createGame(e.roomCode.value, e.nickname.value)
}

async function callJoinGame(e: HomeFormState) {
	if (!e.valid) return
	await joinGame(e.roomCode.value, e.nickname.value)
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
							v-slot="$form"
							:initialValues
							:resolver
							:validateOnValueUpdate="false"
							:validateOnBlur="true"
						>
							<FormField
								v-slot="$field"
								name="nickname"
								:validateOnValueUpdate="getFieldFromDirtyMap(`nickname`)"
							>
								<label>Nickname</label>
								<InputText type="text" placeholder="Nickname" @blur="updateDirtyMap($field)"/>
								<Message v-if="$field?.invalid"
									severity="error"
									size="small"
									variant="simple"
								>
									{{ $field.error?.message }}
								</Message>
							</FormField>

							<FormField
								v-slot="$field"
								name="roomCode"
								:validateOnValueUpdate="getFieldFromDirtyMap(`roomCode`)"
							>
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

							<div class="home-buttons">
								<Button type="submit"
									severity="secondary"
									label="Join"
									@click="callJoinGame($form as any)" />
								<div>&nbsp;</div>
								<Button type="submit"
									severity="secondary"
									label="Create"
									@click="callCreateGame($form as any)" />
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

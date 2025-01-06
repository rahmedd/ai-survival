<script setup lang="ts">
// external libs
import { reactive, ref } from 'vue'

// primevue
import { valibotResolver } from '@primevue/forms/resolvers/valibot'
import type { FormSubmitEvent } from '@primevue/forms'

// composables
import { useEagerValidation } from '@/composables/useEagerValidation'

// types
import type { GenericFormSubmitEvent } from '@/types/GenericFormSubmitEvent'
import { joinCreateRoomSchema, type JoinCreateRoom } from '@/types/JoinCreateRoom'


const props = defineProps<{
	nickname: string
	roomCode: string
}>()

const emit = defineEmits<{
	submit: [value: JoinCreateRoom]
}>()

const { getFieldFromDirtyMap, updateDirtyMap } = useEagerValidation()

const resolver = valibotResolver(joinCreateRoomSchema)

const initialValues = reactive<JoinCreateRoom>({
	nickname:  props.nickname,
	roomCode: props.roomCode,
})

const buttons = ref(['Join', 'Create'])
const selectedButton = ref(buttons.value[0])

async function onSubmit(evt: FormSubmitEvent) {
	const e = evt as GenericFormSubmitEvent<JoinCreateRoom>
	if (!e.valid) return

	emit('submit', e.values)
}

</script>

<template>
	<Form
		:initialValues
		:resolver
		:validateOnValueUpdate="false"
		:validateOnBlur="true"
		@submit="onSubmit"
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
			<InputText type="text" placeholder="Room code" :disabled="!!props.roomCode"/>
			<Message
				v-if="$field?.invalid"
				severity="error"
				size="small"
				variant="simple"
			>
				{{ $field.error?.message }}
			</Message>
		</FormField>

		<div class="home-buttons">
			<Button
				type="submit"
				severity="primary"
				:label="selectedButton"
				class="w-full"
			/>
		</div>
	</Form>
</template>

<style lang="scss" scoped>
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

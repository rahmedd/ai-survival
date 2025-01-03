import { ref } from 'vue'

export function useEagerValidation() {
	const dirtyMap = ref<Record<string, boolean>>({})

	function getFieldFromDirtyMap(name: string) {
		return dirtyMap.value[name]
	}

	function updateDirtyMap(field: any) {
		if (field.dirty) {
			dirtyMap.value[field.name] = true
		}
	}

	return {
		getFieldFromDirtyMap,
		updateDirtyMap,
	}
}
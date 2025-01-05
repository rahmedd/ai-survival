import type { FormFieldState, FormSubmitEvent } from '@primevue/forms'

export type GenericFormSubmitEvent<T extends string> = FormSubmitEvent & Omit<FormSubmitEvent, 'states'> & {
	states: Record<T, FormFieldState>;
}

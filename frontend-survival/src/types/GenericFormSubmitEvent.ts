import type { FormSubmitEvent } from '@primevue/forms'

// export type GenericFormSubmitEvent<T extends string> = FormSubmitEvent & Omit<FormSubmitEvent, 'states'> & {
// 	states: Record<T, FormFieldState>;
// }

// export type GenericFormSubmitEvent<T extends string> = FormSubmitEvent & Omit<FormSubmitEvent, 'states'> & {
// 	states: Record<T, FormFieldState>;
// } & Omit<FormSubmitEvent, 'values'> & { values: Record<T, any> }

export type GenericFormSubmitEvent<T> = FormSubmitEvent & Omit<FormSubmitEvent, 'values'> & { values: T }


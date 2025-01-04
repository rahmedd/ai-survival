import type { FormFieldState } from '@primevue/forms'

export type GenericFormFieldState<T> = Record<keyof T, FormFieldState> & { valid: boolean }

import pluginVue from 'eslint-plugin-vue'
import vueTsEslintConfig from '@vue/eslint-config-typescript'
import pluginVitest from '@vitest/eslint-plugin'
import pluginPlaywright from 'eslint-plugin-playwright'
import stylistic from '@stylistic/eslint-plugin'

export default [
	{
		plugins: {
			'@stylistic': stylistic,
		},
		rules: {
			'vue/multi-word-component-names': 'off',
			'vue/singleline-html-element-content-newline': 'off',
			'vue/html-comment-indent': ['error', 'tab'],
			'vue/script-indent': ['error', 'tab', { switchCase: 1 }],
			'vue/html-indent': ['error', 'tab'],
			'vue/max-attributes-per-line': ['error', { singleline: 3, multiline: { max: 1 } }],
			'vue/attribute-hyphenation': ['error', 'never'],
			'vue/v-on-event-hyphenation': ['error', 'never'],
			'vue/camelcase': ['error', { properties: 'always' }]
		},
	},
	{
		name: 'app/files-to-lint',
		files: ['**/*.{ts,mts,tsx,vue}'],
	},

	{
		name: 'app/files-to-ignore',
		ignores: ['**/dist/**', '**/dist-ssr/**', '**/coverage/**'],
	},

	...pluginVue.configs['flat/essential'],
	...vueTsEslintConfig(),
	{
		rules: {
			'vue/html-indent': ['warn', 'tab'],
		}
	},
	{
		...pluginVitest.configs.recommended,
		files: ['src/**/__tests__/*'],
	},

	{
		...pluginPlaywright.configs['flat/recommended'],
		files: ['e2e/**/*.{test,spec}.{js,ts,jsx,tsx}'],
	},
];

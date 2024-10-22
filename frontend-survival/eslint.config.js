import pluginVue from 'eslint-plugin-vue';
import vueTsEslintConfig from '@vue/eslint-config-typescript';
import pluginVitest from '@vitest/eslint-plugin';
import pluginPlaywright from 'eslint-plugin-playwright';
import stylisticTs from '@stylistic/eslint-plugin-ts';
// import parserTs from "@typescript-eslint/parser";

export default [
	{
		plugins: {
			'@stylistic/ts': stylisticTs,
		},
		// parser: parserTs,
		rules: {
			// "@typescript-eslint/indent": ["error", "tab"],
			'@stylistic/ts/indent': ['error', 'tab'],
			'@stylistic/ts/quotes': ['error', 'single', { 'allowTemplateLiterals': true, 'avoidEscape': true }],
			'@stylistic/ts/brace-style': ['error', 'stroustrup'],
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

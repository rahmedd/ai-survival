import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LobbyView from '../views/LobbyView.vue'
import PromptView from '../views/PromptView.vue'
import AnswerView from '../views/AnswerView.vue'
import JudgeView from '../views/JudgeView.vue'
import LeaderboardView from '../views/LeaderboardView.vue'

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes: [
		{
			path: '/',
			name: 'home',
			component: HomeView
		},
		{
			path: '/lobby',
			name: 'lobby',
			component: LobbyView
		},
		{
			path: '/prompt',
			name: 'prompt',
			component: PromptView
		},
		{
			path: '/answer',
			name: 'answer',
			component: AnswerView
		},
		{
			path: '/judge',
			name: 'judge',
			component: JudgeView
		},
		{
			path: '/leaderboard',
			name: 'leaderboard',
			component: LeaderboardView
		},
		{
			path: '/about',
			name: 'about',
			// route level code-splitting
			// this generates a separate chunk (About.[hash].js) for this route
			// which is lazy-loaded when the route is visited.
			component: () => import('../views/AboutView.vue')
		}
	]
})

export default router

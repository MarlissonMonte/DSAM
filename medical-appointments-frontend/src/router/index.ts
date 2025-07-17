import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('../views/HomeView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
      meta: { requiresGuest: true }
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue'),
      meta: { requiresGuest: true }
    },
    {
      path: '/appointments',
      name: 'appointments',
      component: () => import('../views/AppointmentsView.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/schedules',
      name: 'schedules',
      component: () => import('../views/SchedulesView.vue'),
      meta: { requiresAuth: true, requiresDoctor: true }
    },
    {
      path: '/book-appointment',
      name: 'book-appointment',
      component: () => import('../views/BookAppointmentView.vue'),
      meta: { requiresAuth: true, requiresPatient: true }
    },
    {
      path: '/profile',
      name: 'profile',
      component: () => import('../views/ProfileView.vue'),
      meta: { requiresAuth: true }
    }
  ]
})

// Navigation guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  // Se a rota requer autenticação
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
    return
  }
  
  // Se a rota requer que o usuário seja médico
  if (to.meta.requiresDoctor && !authStore.isDoctor) {
    next('/')
    return
  }
  
  // Se a rota requer que o usuário seja paciente
  if (to.meta.requiresPatient && !authStore.isPatient) {
    next('/')
    return
  }
  
  // Se a rota requer que o usuário seja guest (não autenticado)
  if (to.meta.requiresGuest && authStore.isAuthenticated) {
    next('/')
    return
  }
  
  next()
})

export default router 
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { User, LoginRequest, RegisterRequest } from '@/types'
import { authService } from '@/services/api'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(null)
  const loading = ref(false)

  const isAuthenticated = computed(() => !!token.value)
  const isDoctor = computed(() => user.value?.userType === 'Doctor')
  const isPatient = computed(() => user.value?.userType === 'Patient')

  // Inicializar estado do localStorage
  const initAuth = () => {
    const storedToken = localStorage.getItem('token')
    const storedUser = localStorage.getItem('user')
    
    if (storedToken && storedUser) {
      token.value = storedToken
      user.value = JSON.parse(storedUser)
    }
  }

  const login = async (credentials: LoginRequest) => {
    loading.value = true
    try {
      const response = await authService.login(credentials)
      token.value = response.token
      user.value = response.user
      
      localStorage.setItem('token', response.token)
      localStorage.setItem('user', JSON.stringify(response.user))
      
      return response
    } catch (error) {
      throw error
    } finally {
      loading.value = false
    }
  }

  const register = async (userData: RegisterRequest) => {
    loading.value = true
    try {
      const response = await authService.register(userData)
      token.value = response.token
      user.value = response.user
      
      localStorage.setItem('token', response.token)
      localStorage.setItem('user', JSON.stringify(response.user))
      
      return response
    } catch (error) {
      throw error
    } finally {
      loading.value = false
    }
  }

  const logout = () => {
    token.value = null
    user.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  // Inicializar auth ao carregar a store
  initAuth()

  return {
    user,
    token,
    loading,
    isAuthenticated,
    isDoctor,
    isPatient,
    login,
    register,
    logout,
    initAuth
  }
}) 
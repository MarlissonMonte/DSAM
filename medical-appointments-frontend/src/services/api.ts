import axios from 'axios'
import type { 
  AuthResponse, 
  LoginRequest, 
  RegisterRequest,
  Schedule,
  Appointment,
  CreateScheduleRequest,
  UpdateScheduleRequest,
  CreateAppointmentRequest,
  UpdateAppointmentRequest,
  ScheduleSearchRequest,
  AppointmentSearchRequest,
  TimeSlot
} from '../types'
import dayjs from 'dayjs'

const API_BASE_URL = '/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Interceptor para adicionar token de autenticação
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Interceptor para tratar erros de resposta
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export const authService = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/auth/login', credentials)
    return response.data
  },

  async register(userData: RegisterRequest): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/auth/register', userData)
    return response.data
  }
}

export const scheduleService = {
  async createSchedule(scheduleData: CreateScheduleRequest): Promise<Schedule> {
    // Converter startTime e endTime para formato 'HH:mm:ss' se vierem como string
    const payload = {
      ...scheduleData,
      startTime: typeof scheduleData.startTime === 'string' ? scheduleData.startTime : dayjs(scheduleData.startTime).format('HH:mm:ss'),
      endTime: typeof scheduleData.endTime === 'string' ? scheduleData.endTime : dayjs(scheduleData.endTime).format('HH:mm:ss')
    }
    const response = await api.post<Schedule>('/schedule', payload)
    return response.data
  },

  async updateSchedule(id: number, scheduleData: UpdateScheduleRequest): Promise<Schedule> {
    // Converter startTime e endTime para formato 'HH:mm:ss' se vierem como string
    const payload = {
      ...scheduleData,
      startTime: scheduleData.startTime ? (typeof scheduleData.startTime === 'string' ? scheduleData.startTime : dayjs(scheduleData.startTime).format('HH:mm:ss')) : undefined,
      endTime: scheduleData.endTime ? (typeof scheduleData.endTime === 'string' ? scheduleData.endTime : dayjs(scheduleData.endTime).format('HH:mm:ss')) : undefined
    }
    const response = await api.put<Schedule>(`/schedule/${id}`, payload)
    return response.data
  },

  async deleteSchedule(id: number): Promise<void> {
    await api.delete(`/schedule/${id}`)
  },

  async getSchedule(id: number): Promise<Schedule> {
    const response = await api.get<Schedule>(`/schedule/${id}`)
    return response.data
  },

  async getSchedules(params?: ScheduleSearchRequest): Promise<Schedule[]> {
    const response = await api.get<Schedule[]>('/schedule', { params })
    return response.data
  },

  async getAvailableTimeSlots(doctorId: number, date: string): Promise<TimeSlot[]> {
    const response = await api.get<TimeSlot[]>(`/Schedule/available-slots/${doctorId}/${date}`)
    return response.data
  }
}

export const appointmentService = {
  async createAppointment(appointmentData: CreateAppointmentRequest): Promise<Appointment> {
    const response = await api.post<Appointment>('/appointment', appointmentData)
    return response.data
  },

  async updateAppointment(id: number, appointmentData: UpdateAppointmentRequest): Promise<Appointment> {
    const response = await api.put<Appointment>(`/appointment/${id}`, appointmentData)
    return response.data
  },

  async cancelAppointment(id: number): Promise<void> {
    await api.delete(`/appointment/${id}`)
  },

  async getAppointment(id: number): Promise<Appointment> {
    const response = await api.get<Appointment>(`/appointment/${id}`)
    return response.data
  },

  async getAppointments(params?: AppointmentSearchRequest): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>('/appointment', { params })
    return response.data
  },

  async getMyAppointments(): Promise<Appointment[]> {
    const response = await api.get<Appointment[]>('/appointment/my-appointments')
    return response.data
  }
}

export const userService = {
  async getDoctors() {
    const response = await api.get('/user/doctors')
    return response.data
  }
}

export default api 
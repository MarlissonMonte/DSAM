export interface User {
  id: number
  name: string
  email: string
  phone: string
  userType: 'Doctor' | 'Patient'
}

export interface AuthResponse {
  token: string
  user: User
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  name: string
  email: string
  password: string
  phone: string
  userType: 'Doctor' | 'Patient'
}

export interface Schedule {
  id: number
  doctorId: number
  doctorName: string
  startTime: string
  endTime: string
  durationMinutes: number
  isAvailable: boolean
  availableTimeSlots: TimeSlot[]
  startTimeString?: string
  endTimeString?: string
}

export interface TimeSlot {
  startTime: string
  endTime: string
  isAvailable: boolean
  startTimeString?: string
  endTimeString?: string
}

export interface CreateScheduleRequest {
  startTime: string
  endTime: string
  durationMinutes: number
}

export interface UpdateScheduleRequest {
  date?: string
  startTime?: string
  endTime?: string
  durationMinutes?: number
  isAvailable?: boolean
}

export interface Appointment {
  id: number
  doctorId: number
  doctorName: string
  patientId: number
  patientName: string
  scheduleId: number
  appointmentDate: string
  appointmentTime: string
  notes?: string
  status: string
  createdAt: string
}

export interface CreateAppointmentRequest {
  doctorId: number
  scheduleId: number
  appointmentDate: string
  appointmentTime?: string
  notes?: string
}

export interface UpdateAppointmentRequest {
  appointmentDate?: string
  appointmentTime?: string
  notes?: string
  status?: string
}

export interface ScheduleSearchRequest {
  doctorId?: number
  startDate?: string
  endDate?: string
  availableOnly?: boolean
}

export interface AppointmentSearchRequest {
  doctorId?: number
  patientId?: number
  startDate?: string
  endDate?: string
  status?: string
} 
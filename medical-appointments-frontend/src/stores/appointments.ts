import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Appointment, CreateAppointmentRequest, UpdateAppointmentRequest } from '@/types'
import { appointmentService } from '@/services/api'

export const useAppointmentStore = defineStore('appointments', () => {
  const appointments = ref<Appointment[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const upcomingAppointments = computed(() => {
    const now = new Date()
    return appointments.value.filter(appointment => {
      const appointmentDate = new Date(appointment.appointmentDate + 'T' + appointment.appointmentTime)
      return appointmentDate > now && appointment.status !== 'Cancelled'
    }).sort((a, b) => {
      const dateA = new Date(a.appointmentDate + 'T' + a.appointmentTime)
      const dateB = new Date(b.appointmentDate + 'T' + b.appointmentTime)
      return dateA.getTime() - dateB.getTime()
    })
  })

  const pastAppointments = computed(() => {
    const now = new Date()
    return appointments.value.filter(appointment => {
      const appointmentDate = new Date(appointment.appointmentDate + 'T' + appointment.appointmentTime)
      return appointmentDate < now || appointment.status === 'Cancelled'
    }).sort((a, b) => {
      const dateA = new Date(a.appointmentDate + 'T' + a.appointmentTime)
      const dateB = new Date(b.appointmentDate + 'T' + b.appointmentTime)
      return dateB.getTime() - dateA.getTime()
    })
  })

  const fetchMyAppointments = async () => {
    loading.value = true
    error.value = null
    try {
      const data = await appointmentService.getMyAppointments()
      appointments.value = data
    } catch (err) {
      error.value = 'Erro ao carregar agendamentos'
      console.error('Error fetching appointments:', err)
    } finally {
      loading.value = false
    }
  }

  const createAppointment = async (appointmentData: CreateAppointmentRequest) => {
    loading.value = true
    error.value = null
    try {
      const newAppointment = await appointmentService.createAppointment(appointmentData)
      appointments.value.push(newAppointment)
      return newAppointment
    } catch (err) {
      error.value = 'Erro ao criar agendamento'
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateAppointment = async (id: number, appointmentData: UpdateAppointmentRequest) => {
    loading.value = true
    error.value = null
    try {
      const updatedAppointment = await appointmentService.updateAppointment(id, appointmentData)
      const index = appointments.value.findIndex(a => a.id === id)
      if (index !== -1) {
        appointments.value[index] = updatedAppointment
      }
      return updatedAppointment
    } catch (err) {
      error.value = 'Erro ao atualizar agendamento'
      throw err
    } finally {
      loading.value = false
    }
  }

  const cancelAppointment = async (id: number) => {
    loading.value = true
    error.value = null
    try {
      await appointmentService.cancelAppointment(id)
      const index = appointments.value.findIndex(a => a.id === id)
      if (index !== -1) {
        appointments.value[index].status = 'Cancelled'
      }
    } catch (err) {
      error.value = 'Erro ao cancelar agendamento'
      throw err
    } finally {
      loading.value = false
    }
  }

  const clearError = () => {
    error.value = null
  }

  return {
    appointments,
    loading,
    error,
    upcomingAppointments,
    pastAppointments,
    fetchMyAppointments,
    createAppointment,
    updateAppointment,
    cancelAppointment,
    clearError
  }
}) 
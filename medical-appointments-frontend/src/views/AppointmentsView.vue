<template>
  <div class="appointments-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <el-icon><Calendar /></el-icon>
          <span>Meus Agendamentos</span>
        </div>
      </template>

      <el-tabs v-model="activeTab" @tab-click="handleTabClick">
        <el-tab-pane label="Próximos" name="upcoming">
          <div v-if="appointmentStore.loading" class="loading">
            <el-skeleton :rows="3" animated />
          </div>
          
          <div v-else-if="upcomingAppointments.length === 0" class="empty-state">
            <el-icon class="empty-icon"><Calendar /></el-icon>
            <p>Nenhum agendamento futuro</p>
          </div>
          
          <el-timeline v-else>
            <el-timeline-item
              v-for="appointment in upcomingAppointments"
              :key="appointment.id"
              :timestamp="formatDateTime(appointment.appointmentDate, appointment.appointmentTime)"
              placement="top"
            >
              <el-card>
                <div class="appointment-header">
                  <h4>{{ getOtherParticipantName(appointment) }}</h4>
                  <el-tag :type="getStatusType(appointment.status)">
                    {{ getStatusText(appointment.status) }}
                  </el-tag>
                </div>
                <p v-if="appointment.notes" class="appointment-notes">
                  {{ appointment.notes }}
                </p>
                <div class="appointment-actions">
                  <el-button
                    v-if="appointment.status !== 'Cancelled'"
                    type="danger"
                    size="small"
                    @click="cancelAppointment(appointment.id)"
                  >
                    Cancelar
                  </el-button>
                </div>
              </el-card>
            </el-timeline-item>
          </el-timeline>
        </el-tab-pane>

        <el-tab-pane label="Passados" name="past">
          <div v-if="appointmentStore.loading" class="loading">
            <el-skeleton :rows="3" animated />
          </div>
          
          <div v-else-if="pastAppointments.length === 0" class="empty-state">
            <el-icon class="empty-icon"><Clock /></el-icon>
            <p>Nenhum agendamento passado</p>
          </div>
          
          <el-timeline v-else>
            <el-timeline-item
              v-for="appointment in pastAppointments"
              :key="appointment.id"
              :timestamp="formatDateTime(appointment.appointmentDate, appointment.appointmentTime)"
              placement="top"
            >
              <el-card>
                <div class="appointment-header">
                  <h4>{{ getOtherParticipantName(appointment) }}</h4>
                  <el-tag :type="getStatusType(appointment.status)">
                    {{ getStatusText(appointment.status) }}
                  </el-tag>
                </div>
                <p v-if="appointment.notes" class="appointment-notes">
                  {{ appointment.notes }}
                </p>
              </el-card>
            </el-timeline-item>
          </el-timeline>
        </el-tab-pane>
      </el-tabs>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAppointmentStore } from '../stores/appointments'
import { useAuthStore } from '../stores/auth'
import type { Appointment } from '../types'
import { ElMessage, ElMessageBox } from 'element-plus'
import dayjs from 'dayjs'

const appointmentStore = useAppointmentStore()
const authStore = useAuthStore()
const activeTab = ref('upcoming')

const upcomingAppointments = computed(() => appointmentStore.upcomingAppointments)
const pastAppointments = computed(() => appointmentStore.pastAppointments)

const formatDateTime = (date: string, time: string) => {
  return dayjs(`${date}T${time}`).format('DD/MM/YYYY HH:mm')
}

const getStatusType = (status: string) => {
  switch (status) {
    case 'Scheduled': return 'primary'
    case 'Confirmed': return 'success'
    case 'Completed': return 'info'
    case 'Cancelled': return 'danger'
    default: return 'warning'
  }
}

const getStatusText = (status: string) => {
  switch (status) {
    case 'Scheduled': return 'Agendado'
    case 'Confirmed': return 'Confirmado'
    case 'Completed': return 'Concluído'
    case 'Cancelled': return 'Cancelado'
    default: return status
  }
}

const getOtherParticipantName = (appointment: Appointment) => {
  if (authStore.isDoctor) return appointment.patientName
  return appointment.doctorName
}

const handleTabClick = () => {
  // Tab changed, no additional logic needed
}

const cancelAppointment = async (appointmentId: number) => {
  try {
    await ElMessageBox.confirm(
      'Tem certeza que deseja cancelar este agendamento?',
      'Confirmar Cancelamento',
      {
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        type: 'warning'
      }
    )
    
    await appointmentStore.cancelAppointment(appointmentId)
    ElMessage.success('Agendamento cancelado com sucesso!')
  } catch (error: any) {
    if (error !== 'cancel') {
      ElMessage.error('Erro ao cancelar agendamento')
    }
  }
}

onMounted(async () => {
  await appointmentStore.fetchMyAppointments()
})
</script>

<style scoped>
.appointments-container {
  max-width: 800px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
}

.loading {
  padding: 20px;
}

.empty-state {
  text-align: center;
  padding: 40px 20px;
  color: #909399;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 10px;
}

.appointment-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.appointment-header h4 {
  margin: 0;
  color: #303133;
}

.appointment-notes {
  color: #606266;
  margin: 10px 0;
  font-style: italic;
}

.appointment-actions {
  margin-top: 10px;
}
</style> 
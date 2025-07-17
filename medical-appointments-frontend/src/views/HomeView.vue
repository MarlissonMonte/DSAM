<template>
  <div class="home-container">
    <el-row :gutter="20">
      <el-col :span="24">
        <el-card class="welcome-card">
          <div class="welcome-content">
            <el-icon class="welcome-icon"><Stethoscope /></el-icon>
            <h1>Bem-vindo ao Sistema de Agendamentos Médicos</h1>
            <p>Olá, {{ authStore.user?.name || 'Usuário' }}!</p>
            <p class="user-type">
              {{ authStore.isDoctor ? 'Médico' : 'Paciente' }}
            </p>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="stats-row">
      <el-col :span="8">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon"><Calendar /></el-icon>
            <div class="stat-info">
              <h3>{{ upcomingAppointments.length }}</h3>
              <p>Agendamentos Futuros</p>
            </div>
          </div>
        </el-card>
      </el-col>

      <el-col :span="8">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon"><Clock /></el-icon>
            <div class="stat-info">
              <h3>{{ pastAppointments.length }}</h3>
              <p>Agendamentos Passados</p>
            </div>
          </div>
        </el-card>
      </el-col>

      <el-col :span="8">
        <el-card class="stat-card">
          <div class="stat-content">
            <el-icon class="stat-icon"><User /></el-icon>
            <div class="stat-info">
              <h3>{{ authStore.user?.userType === 'Doctor' ? 'Pacientes' : 'Médicos' }}</h3>
              <p>{{ authStore.user?.userType === 'Doctor' ? 'Atendidos' : 'Disponíveis' }}</p>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <el-row :gutter="20" class="actions-row">
      <el-col :span="12">
        <el-card class="action-card">
          <template #header>
            <div class="card-header">
              <el-icon><Calendar /></el-icon>
              <span>Próximos Agendamentos</span>
            </div>
          </template>
          
          <div v-if="upcomingAppointments.length === 0" class="empty-state">
            <el-icon class="empty-icon"><Calendar /></el-icon>
            <p>Nenhum agendamento futuro</p>
          </div>
          
          <el-timeline v-else>
            <el-timeline-item
              v-for="appointment in upcomingAppointments.slice(0, 3)"
              :key="appointment.id"
              :timestamp="formatDateTime(appointment.appointmentDate, appointment.appointmentTime)"
              placement="top"
            >
              <el-card>
                <h4>{{ getOtherParticipantName(appointment) }}</h4>
                <p>{{ appointment.notes || 'Sem observações' }}</p>
                <el-tag :type="getStatusType(appointment.status)">
                  {{ getStatusText(appointment.status) }}
                </el-tag>
              </el-card>
            </el-timeline-item>
          </el-timeline>
          
          <div class="view-all">
            <router-link to="/appointments">Ver todos os agendamentos</router-link>
          </div>
        </el-card>
      </el-col>

      <el-col :span="12">
        <el-card class="action-card">
          <template #header>
            <div class="card-header">
              <el-icon><Plus /></el-icon>
              <span>Ações Rápidas</span>
            </div>
          </template>
          
          <div class="quick-actions">
            <el-button
              v-if="authStore.isPatient"
              type="primary"
              size="large"
              @click="$router.push('/book-appointment')"
              class="action-button"
            >
              <el-icon><Plus /></el-icon>
              <span style="margin-left: 5px;">Gerenciar Horários</span>
            </el-button>
            
            <el-button
              v-if="authStore.isDoctor"
              type="primary"
              size="large"
              @click="$router.push('/schedules')"
              class="action-button"
            >
              <el-icon><Clock /></el-icon>
              <span style="margin-left: 5px;">Gerenciar Horários</span>
            </el-button>
            
            <el-button
              type="info"
              size="large"
              @click="$router.push('/appointments')"
              class="action-button"
            >
              <el-icon><Calendar  /> </el-icon>
              <span style="margin-left: 5px;">Ver Agendamentos</span>
            </el-button>
            
            <el-button
              type="warning"
              size="large"
              @click="$router.push('/profile')"
              class="action-button"
            >
              <el-icon><Setting /></el-icon>
              <span style="margin-left: 5px;">Configurações</span>
            </el-button>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue'
import { useAuthStore } from '../stores/auth'
import { useAppointmentStore } from '../stores/appointments'
import type { Appointment } from '../types'
import dayjs from 'dayjs'

const authStore = useAuthStore()
const appointmentStore = useAppointmentStore()

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

onMounted(async () => {
  await appointmentStore.fetchMyAppointments()
})
</script>

<style scoped>
.home-container {
  max-width: 1200px;
  margin: 0 auto;
}

.welcome-card {
  margin-bottom: 20px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.welcome-content {
  text-align: center;
  padding: 20px;
}

.welcome-icon {
  font-size: 48px;
  margin-bottom: 20px;
}

.welcome-content h1 {
  margin: 0 0 10px 0;
  font-size: 28px;
}

.welcome-content p {
  margin: 5px 0;
  font-size: 16px;
}

.user-type {
  font-weight: bold;
  color: #ffd04b;
}

.stats-row {
  margin-bottom: 20px;
}

.stat-card {
  text-align: center;
}

.stat-content {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 15px;
}

.stat-icon {
  font-size: 32px;
  color: #409eff;
}

.stat-info h3 {
  margin: 0;
  font-size: 24px;
  color: #303133;
}

.stat-info p {
  margin: 5px 0 0 0;
  color: #606266;
}

.actions-row {
  margin-bottom: 20px;
}

.action-card {
  height: 100%;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
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

.view-all {
  text-align: center;
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #ebeef5;
}

.view-all a {
  color: #409eff;
  text-decoration: none;
}

.view-all a:hover {
  text-decoration: underline;
}

.quick-actions {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 20px;
  width: 100%;
}

.action-button {
  width: 100%;
  max-width: 400px;
  min-width: 0;
  height: 48px;
  font-size: 16px;
  font-weight: bold;
  justify-content: flex-start;
  display: flex;
  align-items: center;
  gap: 10px;
  border-radius: 8px;
}

@media (max-width: 610px) {
  .quick-actions {
    gap: 10px;
  }
  .action-button {
    width: 100vw;
    max-width: 190px;
    font-size: 15px;
    height: 65px;
    padding: 0 8px;
  }
  .stat-card {
  text-align: center;
  font-size: 12px;
}
.stat-icon {
  font-size: 19px;
  color: #409eff;
}
.stat-info h3 {
  margin: 0;
  font-size: 18px;
  color: #303133;
}

.stat-info p {
  margin: 2% 0 0 0;
  color: #242527;
  font-size: 14px;

}
@media (max-width: 500px) {
  .quick-actions {
    gap: 10px;
  }
  .stat-icon {
  font-size: 0px;
  color: #409eff;
}
.card-header {
  display: flex;
  align-items: center;
  gap: 7px;
  font-weight: bold;
  font-size: 14px;
}

.action-button {
    width: 100vw;
    max-width: 130px;
    font-size: 11px;
    height: 40px;
    padding: 0 8px;
  }
  .stat-info h3 {
  margin: 0;
  font-size: 15px;
  color: #303133;
}

.stat-info p {
  margin: 2% 0 0 0;
  color: #242527;
  font-size: 12px;

}

}
}
</style> 
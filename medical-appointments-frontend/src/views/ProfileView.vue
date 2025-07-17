<template>
  <div class="profile-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <el-icon><User /></el-icon>
          <span>Meu Perfil</span>
        </div>
      </template>

      <el-form
        ref="profileFormRef"
        :model="profileForm"
        label-width="120px"
      >
        <el-form-item label="Nome">
          <el-input
            v-model="profileForm.name"
            placeholder="Nome do usuário"
            disabled
          />
        </el-form-item>

        <el-form-item label="Email">
          <el-input
            v-model="profileForm.email"
            type="email"
            placeholder="Email do usuário"
            disabled
          />
        </el-form-item>

        <el-form-item label="Telefone">
          <el-input
            v-model="profileForm.phone"
            placeholder="Telefone do usuário"
            disabled
          />
        </el-form-item>

        <el-form-item label="Tipo de Usuário">
          <el-tag :type="authStore.isDoctor ? 'primary' : 'success'">
            {{ authStore.isDoctor ? 'Médico' : 'Paciente' }}
          </el-tag>
        </el-form-item>
      </el-form>
    </el-card>

    <!-- Estatísticas do usuário -->
    <el-card class="stats-card">
      <template #header>
        <div class="card-header">
          <el-icon><DataAnalysis /></el-icon>
          <span>Estatísticas</span>
        </div>
      </template>

      <el-row :gutter="20">
        <el-col :span="8">
          <div class="stat-item">
            <el-icon class="stat-icon"><Calendar /></el-icon>
            <div class="stat-info">
              <h3>{{ upcomingAppointments.length }}</h3>
              <p>Agendamentos Futuros</p>
            </div>
          </div>
        </el-col>

        <el-col :span="8">
          <div class="stat-item">
            <el-icon class="stat-icon"><Clock /></el-icon>
            <div class="stat-info">
              <h3>{{ pastAppointments.length }}</h3>
              <p>Agendamentos Passados</p>
            </div>
          </div>
        </el-col>

        <el-col :span="8">
          <div class="stat-item">
            <el-icon class="stat-icon"><User /></el-icon>
            <div class="stat-info">
              <h3>{{ authStore.user?.userType === 'Doctor' ? 'Pacientes' : 'Médicos' }}</h3>
              <p>{{ authStore.user?.userType === 'Doctor' ? 'Atendidos' : 'Consultados' }}</p>
            </div>
          </div>
        </el-col>
      </el-row>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useAppointmentStore } from '@/stores/appointments'

const authStore = useAuthStore()
const appointmentStore = useAppointmentStore()

const profileForm = ref({
  name: authStore.user?.name || '',
  email: authStore.user?.email || '',
  phone: authStore.user?.phone || ''
})

const upcomingAppointments = computed(() => appointmentStore.upcomingAppointments)
const pastAppointments = computed(() => appointmentStore.pastAppointments)

onMounted(async () => {
  await appointmentStore.fetchMyAppointments()
})
</script>

<style scoped>
.profile-container {
  max-width: 800px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
}

.stats-card {
  margin-top: 20px;
}

/* Ocultar estatísticas em telas menores que 510px */
@media (max-width: 510px) {
  .stats-card {
    display: none;
  }
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 15px;
  padding: 20px;
  text-align: center;
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

h3 {
  margin: 20px 0 10px 0;
  color: #303133;
}

</style> 
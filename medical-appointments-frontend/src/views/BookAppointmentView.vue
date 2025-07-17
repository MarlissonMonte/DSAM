<template>
  <div class="book-appointment-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <el-icon><Plus /></el-icon>
          <span>Agendar Consulta</span>
        </div>
      </template>

      <el-form
        ref="appointmentFormRef"
        :model="appointmentForm"
        :rules="appointmentRules"
        label-width="120px"
      >
        <el-form-item label="Médico" prop="doctorId">
          <el-select
            v-model="appointmentForm.doctorId"
            placeholder="Selecione um médico"
            style="width: 100%"
            @change="loadDoctorSchedules"
          >
            <el-option
              v-for="doctor in doctors"
              :key="doctor.id"
              :label="doctor.name"
              :value="doctor.id"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="Data" prop="appointmentDate">
          <el-date-picker
            v-model="appointmentForm.appointmentDate"
            type="date"
            placeholder="Selecione a data"
            style="width: 100%"
            :disabled-date="disabledDate"
            @change="loadAvailableTimeSlots"
          />
        </el-form-item>

        <el-form-item label="Horário" prop="appointmentTime">
          <el-select
            v-model="appointmentForm.appointmentTime"
            placeholder="Selecione o horário"
            style="width: 100%"
            :disabled="!appointmentForm.appointmentDate"
          >
            <el-option
              v-for="slot in availableTimeSlots"
              :key="slot.startTimeString || slot.startTime"
              :label="formatTimeSlot(slot)"
              :value="slot.startTimeString || slot.startTime"
              :disabled="!slot.isAvailable"
            />
          </el-select>
        </el-form-item>

        <el-form-item label="Observações" prop="notes">
          <el-input
            v-model="appointmentForm.notes"
            type="textarea"
            :rows="3"
            placeholder="Digite observações sobre a consulta (opcional)"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            type="primary"
            :loading="loading"
            @click="bookAppointment"
          >
            Agendar Consulta
          </el-button>
          <el-button @click="$router.push('/')">
            Cancelar
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <!-- Informações do médico selecionado -->
    <el-card v-if="selectedDoctor" class="doctor-info">
      <template #header>
        <div class="card-header">
          <el-icon><User /></el-icon>
          <span>Informações do Médico</span>
        </div>
      </template>
      
      <div class="doctor-details">
        <p><strong>Nome:</strong> {{ selectedDoctor.name }}</p>
        <p><strong>Email:</strong> {{ selectedDoctor.email }}</p>
        <p><strong>Telefone:</strong> {{ selectedDoctor.phone }}</p>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import { scheduleService, appointmentService, userService } from '../services/api'
import type { CreateAppointmentRequest, Schedule, TimeSlot, User } from '../types'
import dayjs from 'dayjs'

const router = useRouter()
const appointmentFormRef = ref<FormInstance>()
const loading = ref(false)
const doctors = ref<User[]>([])
const selectedDoctor = ref<User | null>(null)
const availableTimeSlots = ref<TimeSlot[]>([])

const appointmentForm = reactive<CreateAppointmentRequest>({
  doctorId: 0,
  scheduleId: 0,
  appointmentDate: '',
  appointmentTime: '',
  notes: ''
})

const appointmentRules: FormRules = {
  doctorId: [
    { required: true, message: 'Por favor, selecione um médico', trigger: 'change' }
  ],
  appointmentDate: [
    { required: true, message: 'Por favor, selecione uma data', trigger: 'change' }
  ],
  appointmentTime: [
    { required: true, message: 'Por favor, selecione um horário', trigger: 'change' }
  ]
}

const disabledDate = (time: Date) => {
  return time.getTime() < Date.now() - 8.64e7 // Desabilita datas passadas
}

const formatTimeSlot = (slot: TimeSlot) => {
  const startRaw = slot.startTimeString || slot.startTime;
  const endRaw = slot.endTimeString || slot.endTime;
  const start = dayjs(`2000-01-01T${startRaw}`).format('HH:mm');
  const end = dayjs(`2000-01-01T${endRaw}`).format('HH:mm');
  const status = slot.isAvailable ? 'Disponível' : 'Ocupado';
  return `${start} - ${end} ${slot.isAvailable ? '(Disponível)' : '(Ocupado)'}`;
}

const loadDoctors = async () => {
  try {
    doctors.value = await userService.getDoctors()
  } catch (error) {
    ElMessage.error('Erro ao carregar médicos')
  }
}

const loadDoctorSchedules = async () => {
  if (!appointmentForm.doctorId) return
  
  selectedDoctor.value = doctors.value.find(d => d.id === appointmentForm.doctorId) || null
  appointmentForm.appointmentDate = ''
  appointmentForm.appointmentTime = ''
  availableTimeSlots.value = []
}

const loadAvailableTimeSlots = async () => {
  if (!appointmentForm.doctorId || !appointmentForm.appointmentDate) return
  try {
    // A data precisa ser passada no formato YYYY-MM-DDTHH:mm:ss para a rota
    const date = dayjs(appointmentForm.appointmentDate).format('YYYY-MM-DDT00:00:00')
    availableTimeSlots.value = await scheduleService.getAvailableTimeSlots(
      appointmentForm.doctorId,
      date
    )
    appointmentForm.appointmentTime = ''
  } catch (error) {
    ElMessage.error('Erro ao carregar horários disponíveis')
  }
}

const bookAppointment = async () => {
  if (!appointmentFormRef.value) return

  try {
    await appointmentFormRef.value.validate()
    loading.value = true

    // Encontrar o schedule correspondente
    const schedule = await scheduleService.getSchedules({
      doctorId: appointmentForm.doctorId,
      startDate: appointmentForm.appointmentDate,
      endDate: appointmentForm.appointmentDate
    })

    if (schedule.length === 0) {
      throw new Error('Nenhum horário encontrado para esta data')
    }

    appointmentForm.scheduleId = schedule[0].id

    // Montar data e hora juntos (YYYY-MM-DDTHH:mm:00)
    const date = dayjs(appointmentForm.appointmentDate).format('YYYY-MM-DD')
    const time = appointmentForm.appointmentTime
    const appointmentDate = dayjs(`${date}T${time}:00`).format('YYYY-MM-DDTHH:mm:ss')

    // Enviar apenas os campos esperados pelo backend
    await appointmentService.createAppointment({
      doctorId: appointmentForm.doctorId,
      scheduleId: appointmentForm.scheduleId,
      appointmentDate,
      notes: appointmentForm.notes
    })
    ElMessage.success('Consulta agendada com sucesso!')
    router.push('/appointments')
  } catch (error: any) {
    ElMessage.error(error.response?.data?.message || 'Erro ao agendar consulta')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadDoctors()
})
</script>

<style scoped>
.book-appointment-container {
  max-width: 800px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
}

.doctor-info {
  margin-top: 20px;
}

.doctor-details p {
  margin: 10px 0;
  color: #606266;
}

.doctor-details strong {
  color: #303133;
}
</style> 
<template>
  <div class="schedules-container">
    <el-card>
      <template #header>
        <div class="card-header">
          <el-icon><Clock /></el-icon>
          <span>Gerenciar Horários</span>
          <el-button
            type="primary"
            @click="schedule ? editSchedule(schedule) : showCreateDialog = true"
          >
            <el-icon><Plus /></el-icon>
            {{ schedule ? 'Editar Horário' : 'Novo Horário' }}
          </el-button>
        </div>
      </template>

      <div v-if="loading" class="loading">
        <el-skeleton :rows="5" animated />
      </div>

      <div v-else-if="!schedule" class="empty-state">
        <el-icon class="empty-icon"><Clock /></el-icon>
        <p>Nenhum horário cadastrado</p>
        <el-button type="primary" @click="showCreateDialog = true">
          Cadastrar Horário
        </el-button>
      </div>

      <div v-else class="single-schedule">
        <div class="schedule-info">
          <p><b>Início:</b> {{ formatTime(schedule.startTimeString || schedule.startTime) }}</p>
          <p><b>Fim:</b> {{ formatTime(schedule.endTimeString || schedule.endTime) }}</p>
          <p><b>Duração:</b> {{ schedule.durationMinutes }} min</p>
        </div>
        <div class="schedule-actions">
          <el-button size="small" @click="editSchedule(schedule)">
            <el-icon><Edit /></el-icon>
            Editar
          </el-button>
        </div>
      </div>
    </el-card>

    <!-- Dialog para criar/editar horário -->
    <el-dialog
      v-model="showCreateDialog"
      :title="editingSchedule ? 'Editar Horário' : 'Novo Horário'"
      width="500px"
    >
      <el-form
        ref="scheduleFormRef"
        :model="scheduleForm"
        :rules="scheduleRules"
        label-width="100px"
      >
        <el-form-item label="Horário Início" prop="startTime">
          <el-time-picker
            v-model="scheduleForm.startTime"
            placeholder="início"
            style="width: 100%"
          />
        </el-form-item>

        <el-form-item label="Fim" prop="endTime">
          <el-time-picker
            v-model="scheduleForm.endTime"
            placeholder="Horário de fim"
            style="width: 100%"
          />
        </el-form-item>

        <el-form-item label="Duração" prop="durationMinutes">
          <el-select v-model="scheduleForm.durationMinutes" style="width: 100%">
            <el-option label="15 minutos" :value="15" />
            <el-option label="30 minutos" :value="30" />
            <el-option label="45 minutos" :value="45" />
            <el-option label="60 minutos" :value="60" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <span class="dialog-footer">
          <el-button @click="showCreateDialog = false">Cancelar</el-button>
          <el-button type="primary" @click="saveSchedule">
            {{ editingSchedule ? 'Atualizar' : 'Criar' }}
          </el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { ElMessage, ElMessageBox, type FormInstance, type FormRules } from 'element-plus'
import type { Schedule, CreateScheduleRequest, UpdateScheduleRequest } from '../types'
import dayjs from 'dayjs'
import { scheduleService } from '../services/api'
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore();
const schedule = ref<Schedule | null>(null)
const loading = ref(false)
const showCreateDialog = ref(false)
const editingSchedule = ref<Schedule | null>(null)
const scheduleFormRef = ref<FormInstance>()

const scheduleForm = reactive({
  startTime: '',
  endTime: '',
  durationMinutes: 30
})

const scheduleRules: FormRules = {
  startTime: [
    { required: true, message: 'Por favor, selecione o horário de início', trigger: 'change' }
  ],
  endTime: [
    { required: true, message: 'Por favor, selecione o horário de fim', trigger: 'change' }
  ],
  durationMinutes: [
    { required: true, message: 'Por favor, selecione a duração', trigger: 'change' }
  ]
}

const formatDate = (date: string) => {
  return dayjs(date).format('DD/MM/YYYY')
}

const formatTime = (time: string) => {
  return dayjs(`2000-01-01T${time}`).format('HH:mm')
}

const loadSchedules = async () => {
  loading.value = true
  try {
    const all = await scheduleService.getSchedules({ doctorId: authStore.user?.id })
    schedule.value = all.length > 0 ? all[0] : null
  } catch (error) {
    ElMessage.error('Erro ao carregar horário')
  } finally {
    loading.value = false
  }
}

const editSchedule = (s: Schedule) => {
  editingSchedule.value = s
  scheduleForm.startTime = s.startTimeString || s.startTime
  scheduleForm.endTime = s.endTimeString || s.endTime
  scheduleForm.durationMinutes = s.durationMinutes
  showCreateDialog.value = true
}

const saveSchedule = async () => {
  if (!scheduleFormRef.value) return

  try {
    await scheduleFormRef.value.validate()
    
    if (editingSchedule.value) {
      const updateData = {
        startTime: scheduleForm.startTime,
        endTime: scheduleForm.endTime,
        durationMinutes: scheduleForm.durationMinutes
      }
      await scheduleService.updateSchedule(editingSchedule.value.id, updateData)
      ElMessage.success('Horário atualizado com sucesso!')
    } else {
      const payload = {
        startTime: dayjs(scheduleForm.startTime).format('HH:mm:ss'),
        endTime: dayjs(scheduleForm.endTime).format('HH:mm:ss'),
        durationMinutes: scheduleForm.durationMinutes
      }
      await scheduleService.createSchedule(payload)
      ElMessage.success('Horário criado com sucesso!')
    }
    
    showCreateDialog.value = false
    editingSchedule.value = null
    resetForm()
    await loadSchedules()
  } catch (error: any) {
    ElMessage.error(error.response?.data?.message || 'Erro ao salvar horário')
  }
}

const deleteSchedule = async (scheduleId: number) => {
  try {
    await ElMessageBox.confirm(
      'Tem certeza que deseja excluir este horário?',
      'Confirmar Exclusão',
      {
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        type: 'warning'
      }
    )
    
    await scheduleService.deleteSchedule(scheduleId)
    ElMessage.success('Horário excluído com sucesso!')
    await loadSchedules()
  } catch (error: any) {
    if (error !== 'cancel') {
      ElMessage.error('Erro ao excluir horário')
    }
  }
}

const resetForm = () => {
  scheduleForm.startTime = ''
  scheduleForm.endTime = ''
  scheduleForm.durationMinutes = 30
}

onMounted(() => {
  loadSchedules()
})
</script>

<style scoped>
.schedules-container {
  max-width: 1200px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: bold;
}

.card-header .el-button {
  margin-left: auto;
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

.single-schedule {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
  border: 1px solid #ebeef5;
  border-radius: 8px;
  margin-top: 20px;
}

.schedule-info {
  flex-grow: 1;
  margin-right: 20px;
}

.schedule-info p {
  margin-bottom: 5px;
  color: #606266;
}

.schedule-info b {
  color: #303133;
}

.schedule-actions {
  display: flex;
  gap: 10px;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}
</style> 
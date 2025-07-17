<template>
  <div class="register-container">
    <el-card class="register-card">
      <template #header>
        <div class="card-header">
          <el-icon class="header-icon"><UserFilled /></el-icon>
          <h2>Cadastrar no Sistema</h2>
        </div>
      </template>

      <el-form
        ref="registerFormRef"
        :model="registerForm"
        :rules="registerRules"
        label-width="100px"
        @submit.prevent="handleRegister"
      >
        <el-form-item label="Nome" prop="name">
          <el-input
            v-model="registerForm.name"
            placeholder="Digite seu nome completo"
            :prefix-icon="User"
          />
        </el-form-item>

        <el-form-item label="Email" prop="email">
          <el-input
            v-model="registerForm.email"
            type="email"
            placeholder="Digite seu email"
            :prefix-icon="Message"
          />
        </el-form-item>

        <el-form-item label="Telefone" prop="phone">
          <el-input
            v-model="registerForm.phone"
            placeholder="Digite seu telefone"
            :prefix-icon="Phone"
          />
        </el-form-item>

        <el-form-item label="Tipo de Usuário" prop="userType">
          <el-radio-group v-model="registerForm.userType">
            <el-radio label="Patient">Paciente</el-radio>
            <el-radio label="Doctor">Médico</el-radio>
          </el-radio-group>
        </el-form-item>

        <el-form-item label="Senha" prop="password">
          <el-input
            v-model="registerForm.password"
            type="password"
            placeholder="Digite sua senha"
            :prefix-icon="Lock"
            show-password
          />
        </el-form-item>

        <el-form-item label="Confirmar Senha" prop="confirmPassword">
          <el-input
            v-model="registerForm.confirmPassword"
            type="password"
            placeholder="Confirme sua senha"
            :prefix-icon="Lock"
            show-password
          />
        </el-form-item>

        <el-form-item>
          <el-button
            type="primary"
            :loading="authStore.loading"
            @click="handleRegister"
            class="register-button"
          >
            Cadastrar
          </el-button>
        </el-form-item>
      </el-form>

      <div class="login-link">
        <p>
          Já tem uma conta?
          <router-link to="/login">Faça login aqui</router-link>
        </p>
      </div>
      <div class="back-link">
        <el-button type="info" @click="router.push('/login')" plain style="color: #303133; border: 1px solid #409eff; background: fff;">Voltar</el-button>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import type { RegisterRequest } from '@/types'

const router = useRouter()
const authStore = useAuthStore()
const registerFormRef = ref<FormInstance>()

const registerForm = reactive<RegisterRequest & { confirmPassword: string }>({
  name: '',
  email: '',
  phone: '',
  password: '',
  userType: 'Patient',
  confirmPassword: ''
})

const validateConfirmPassword = (rule: any, value: string, callback: any) => {
  if (value === '') {
    callback(new Error('Por favor, confirme sua senha'))
  } else if (value !== registerForm.password) {
    callback(new Error('As senhas não coincidem'))
  } else {
    callback()
  }
}

const registerRules: FormRules = {
  name: [
    { required: true, message: 'Por favor, digite seu nome', trigger: 'blur' },
    { min: 2, message: 'O nome deve ter pelo menos 2 caracteres', trigger: 'blur' }
  ],
  email: [
    { required: true, message: 'Por favor, digite seu email', trigger: 'blur' },
    { type: 'email', message: 'Por favor, digite um email válido', trigger: 'blur' }
  ],
  phone: [
    { required: true, message: 'Por favor, digite seu telefone', trigger: 'blur' }
  ],
  userType: [
    { required: true, message: 'Por favor, selecione o tipo de usuário', trigger: 'change' }
  ],
  password: [
    { required: true, message: 'Por favor, digite sua senha', trigger: 'blur' },
    { min: 6, message: 'A senha deve ter pelo menos 6 caracteres', trigger: 'blur' }
  ],
  confirmPassword: [
    { required: true, validator: validateConfirmPassword, trigger: 'blur' }
  ]
}

const handleRegister = async () => {
  if (!registerFormRef.value) return

  try {
    await registerFormRef.value.validate()
    
    const { confirmPassword, ...userData } = registerForm
    await authStore.register(userData)
    
    ElMessage.success('Cadastro realizado com sucesso!')
    router.push('/')
  } catch (error: any) {
    ElMessage.error(error.response?.data?.message || 'Erro ao fazer cadastro')
  }
}
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 120px);
  padding: 20px;
}

.register-card {
  width: 100%;
  max-width: 500px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}

.header-icon {
  font-size: 24px;
  color: #409eff;
}

.card-header h2 {
  margin: 0;
  color: #303133;
}

.register-button {
  width: 100%;
  height: 40px;
}

.login-link {
  text-align: center;
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #ebeef5;
}

.login-link a {
  color: #409eff;
  text-decoration: none;
}

.login-link a:hover {
  text-decoration: underline;
}

.back-link {
  text-align: center;
  margin-top: 10px;
}
</style> 
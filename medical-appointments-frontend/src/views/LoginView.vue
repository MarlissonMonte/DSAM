<template>
  <div class="login-container">
    <el-card class="login-card">
      <template #header>
        <div class="card-header">
          <el-icon class="header-icon"><User /></el-icon>
          <h2>Entrar no Sistema</h2>
        </div>
      </template>

      <el-form
        ref="loginFormRef"
        :model="loginForm"
        :rules="loginRules"
        label-width="80px"
        @submit.prevent="handleLogin"
      >
        <el-form-item label="Email" prop="email">
          <el-input
            v-model="loginForm.email"
            type="email"
            placeholder="Digite seu email"
            :prefix-icon="Message"
          />
        </el-form-item>

        <el-form-item label="Senha" prop="password">
          <el-input
            v-model="loginForm.password"
            type="password"
            placeholder="Digite sua senha"
            :prefix-icon="Lock"
            show-password
          />
        </el-form-item>

        <el-form-item>
          <el-button
            type="primary"
            :loading="authStore.loading"
            @click="handleLogin"
            class="login-button"
          >
            Entrar
          </el-button>
        </el-form-item>
      </el-form>

      <div class="register-link">
        <p>
          Não tem uma conta?
          <router-link to="/register">Cadastre-se aqui</router-link>
        </p>
      </div>
      <div class="back-link">
        <el-button type="info" @click="router.push('/')" plain style="color: #303133; border: 1px solid #409eff; background: #fff;">Voltar</el-button>
      </div>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { ElMessage, type FormInstance, type FormRules } from 'element-plus'
import type { LoginRequest } from '../types'

const router = useRouter()
const authStore = useAuthStore()
const loginFormRef = ref<FormInstance>()

const loginForm = reactive<LoginRequest>({
  email: '',
  password: ''
})

const loginRules: FormRules = {
  email: [
    { required: true, message: 'Por favor, digite seu email', trigger: 'blur' },
    { type: 'email', message: 'Por favor, digite um email válido', trigger: 'blur' }
  ],
  password: [
    { required: true, message: 'Por favor, digite sua senha', trigger: 'blur' },
    { min: 6, message: 'A senha deve ter pelo menos 6 caracteres', trigger: 'blur' }
  ]
}

const handleLogin = async () => {
  if (!loginFormRef.value) return

  try {
    await loginFormRef.value.validate()
    await authStore.login(loginForm)
    ElMessage.success('Login realizado com sucesso!')
    router.push('/')
  } catch (error: any) {
    ElMessage.error(error.response?.data?.message || 'Erro ao fazer login')
  }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 120px);
  padding: 20px;
}

.login-card {
  width: 100%;
  max-width: 400px;
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

.login-button {
  
  width: 100%;
  height: 40px;
}

.register-link {
  text-align: center;
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid #ebeef5;
}

.register-link a {
  color: #409eff;
  text-decoration: none;
}

.register-link a:hover {
  text-decoration: underline;
}

.back-link {
  text-align: center;
  margin-top: 10px;
}
</style> 
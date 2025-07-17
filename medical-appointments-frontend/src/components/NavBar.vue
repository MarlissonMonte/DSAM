<template>
  <el-menu
    :default-active="activeIndex"
    class="nav-menu"
    mode="horizontal"
    background-color="transparent"
    text-color="#fff"
    active-text-color="#ffd04b"
    @select="handleSelect"
  >
    <div class="nav-brand">
      <el-icon class="brand-icon"><Stethoscope /></el-icon>
      <span class="brand-text">Agendamentos Médicos</span>
    </div>

    <div class="nav-spacer"></div>

    <template v-if="authStore.isAuthenticated">
      <el-menu-item index="/">
        <el-icon><House /></el-icon>
        <span>Início</span>
      </el-menu-item>

      <el-menu-item index="/appointments">
        <el-icon><Calendar /></el-icon>
        <span>Meus Agendamentos</span>
      </el-menu-item>

      <el-menu-item v-if="authStore.isDoctor" index="/schedules">
        <el-icon><Clock /></el-icon>
        <span>Meus Horários</span>
      </el-menu-item>

      <el-menu-item v-if="authStore.isPatient" index="/book-appointment">
        <el-icon><Plus /></el-icon>
        <span>Agendar Consulta</span>
      </el-menu-item>

      <el-menu-item disabled class="user-name-item" style="margin-left:auto;">
        <el-icon><User /></el-icon>
        <span>{{ authStore.user?.name || 'Usuário' }}</span>
      </el-menu-item>
      <el-menu-item index="/profile">
        <el-icon><Setting /></el-icon>
        <span>Perfil</span>
      </el-menu-item>
      <el-menu-item @click="handleLogout">
        <el-icon><SwitchButton /></el-icon>
        <span>Sair</span>
      </el-menu-item>
    </template>

    <template v-else>
      <el-menu-item index="/login">
        <el-icon><User /></el-icon>
        <span>Entrar</span>
      </el-menu-item>
      <el-menu-item index="/register">
        <el-icon><UserFilled /></el-icon>
        <span>Cadastrar</span>
      </el-menu-item>
    </template>
  </el-menu>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { ElMessage } from 'element-plus'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const activeIndex = computed(() => route.path)

const handleSelect = (key: string) => {
  if (key === 'profile') return
  router.push(key)
}

const handleLogout = () => {
  authStore.logout()
  ElMessage.success('Logout realizado com sucesso!')
  router.push('/login')
}
</script>

<style scoped>
.nav-menu {
  height: 60px;
  display: flex;
  align-items: center;
  padding: 0 20px;
}

.nav-brand {
  display: flex;
  align-items: center;
  margin-right: 40px;
  color: white;
  font-size: 20px;
  font-weight: bold;
}

.brand-icon {
  font-size: 24px;
  margin-right: 10px;
}

.brand-text {
  color: white;
}

.nav-spacer {
  flex: 1;
}

.profile-menu {
  margin-left: auto;
}

/* Ajuste para o dropdown do menu de perfil */
.el-sub-menu__popper {
  background: #fff !important;
  color: #303133 !important;
  box-shadow: 0 4px 16px rgba(0,0,0,0.15);
  border-radius: 8px;
  min-width: 180px;
}

.el-sub-menu__title {
  color: #fff !important;
}

.el-menu--horizontal .el-menu-item {
  color: #b9bcc2;
}

.el-menu-item {
  color: #303133 !important;
}

.el-menu-item:hover, .el-sub-menu__title:hover {
  background: #f5f7fa !important;
  color: #409eff !important;
}

.profile-menu .el-menu-item {
  color: #303133 !important;
}

.profile-menu .el-menu-item:hover {
  background: #f5f7fa !important;
  color: #409eff !important;
}
</style> 
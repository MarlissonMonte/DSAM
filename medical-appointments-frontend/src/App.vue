<template>
  <div id="app">
    <el-container class="app-container">
      <el-header class="app-header">
        <nav-bar />
      </el-header>
      
      <el-main class="app-main">
        <router-view />
      </el-main>
      
      <el-footer class="app-footer">
        <div class="footer-content">
          <p>&copy; 2024 Sistema de Agendamentos Médicos</p>
          <p class="author-info">
            Desenvolvido por 
            <a href="https://github.com/MarlissonMonte" target="_blank" rel="noopener noreferrer">
              Marlisson Monte
            </a>
          </p>
        </div>
      </el-footer>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import NavBar from './components/NavBar.vue'
</script>

<style>
.app-container {
  min-height: 100vh;
}

.app-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 0;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.app-main {
  padding: 20px;
  background-color: #f5f7fa;
  min-height: calc(100vh - 120px);
}

.app-footer {
  background-color: #545c64;
  color: #ffffff;
  text-align: center;
  padding: 20px;
  font-size: 14px;
}

.footer-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
}

.author-info {
  font-size: 13px;
  opacity: 0.9;
}

.author-info a {
  color: #409eff;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.3s ease;
}

.author-info a:hover {
  color: #66b1ff;
  text-decoration: underline;
}

#app {
  font-family: 'Helvetica Neue', Helvetica, 'PingFang SC', 'Hiragino Sans GB', 'Microsoft YaHei', '微软雅黑', Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  background-color: #f5f7fa;
}
</style> 
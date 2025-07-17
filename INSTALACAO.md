# Guia de Instalação - Sistema de Agendamentos Médicos (DSAM)

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** - [Download](https://nodejs.org/)
- **PostgreSQL 15+** - [Download](https://www.postgresql.org/download/)
- **Git** - [Download](https://git-scm.com/)
- **Docker** (opcional) - [Download](https://www.docker.com/)

## 🚀 Instalação Passo a Passo

### 1. Clone o Repositório

```bash
git clone https://github.com/MarlissonMonte/DSAM.git
cd DSAM
```

### 2. Configurar o Banco de Dados

#### Opção A: Usando Docker (Recomendado)

```bash
# Iniciar o PostgreSQL via Docker
docker-compose up -d

# Verificar se o container está rodando
docker ps
```

#### Opção B: Instalação Manual do PostgreSQL

**Windows:**
1. Baixe o PostgreSQL do site oficial
2. Instale seguindo o assistente
3. Configure a senha do usuário postgres

**Linux (Ubuntu/Debian):**
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

**macOS:**
```bash
brew install postgresql
brew services start postgresql
```

**Criar banco de dados:**
```bash
# Conectar ao PostgreSQL
psql -U postgres

# Criar banco e usuário
CREATE DATABASE dsam;
CREATE USER dsam_user WITH PASSWORD 'dsam_password';
GRANT ALL PRIVILEGES ON DATABASE dsam TO dsam_user;
\q
```

### 3. Configurar o Backend

```bash
# Navegar para o diretório do backend
cd app.api

# Restaurar dependências
dotnet restore

# Verificar se a connection string está correta
# O arquivo appsettings.json deve conter:
# "ConnectionStrings": {
#   "DefaultConnection": "Host=localhost;Port=5433;Database=dsam;Username=dsam_user;Password=dsam_password"
# }

# Executar o backend
dotnet run
```

**Verificações:**
- Backend deve estar rodando em: `http://localhost:5195`
- Swagger disponível em: `http://localhost:5195/swagger`
- Verifique se não há erros no console

### 4. Configurar o Frontend

```bash
# Navegar para o diretório do frontend
cd medical-appointments-frontend

# Instalar dependências
npm install

# Verificar se o vite.config.js está configurado corretamente
# O proxy deve apontar para: http://localhost:5195

# Executar o frontend
npm run dev
```

**Verificações:**
- Frontend deve estar rodando em: `http://localhost:3000`
- Verifique se não há erros no console do navegador
- Teste se a página carrega corretamente

## 🧪 Testando o Sistema

### 1. Criar Contas de Teste

**Criar conta de médico:**
1. Acesse: `http://localhost:3000/register`
2. Selecione "Médico"
3. Preencha os dados:
   - Nome: Dr. João Silva
   - Email: medico@teste.com
   - Senha: 123456
   - Telefone: (11) 99999-9999

**Criar conta de paciente:**
1. Acesse: `http://localhost:3000/register`
2. Selecione "Paciente"
3. Preencha os dados:
   - Nome: Maria Santos
   - Email: paciente@teste.com
   - Senha: 123456
   - Telefone: (11) 88888-8888

### 2. Testar Funcionalidades

**Como Médico:**
1. Faça login com a conta de médico
2. Acesse `/schedules` para cadastrar horários
3. Configure horários disponíveis (ex: 08:00 às 18:00, 30 min por consulta)
4. Acesse `/appointments` para ver agendamentos

**Como Paciente:**
1. Faça login com a conta de paciente
2. Acesse `/book-appointment` para agendar consultas
3. Selecione um médico e data
4. Escolha um horário disponível
5. Confirme o agendamento
6. Acesse `/appointments` para ver seus agendamentos

## 🔧 Comandos Úteis

### Backend
```bash
# Executar em desenvolvimento
dotnet run

# Executar em modo watch (recompila automaticamente)
dotnet watch run

# Criar migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Limpar build
dotnet clean

# Restaurar dependências
dotnet restore
```

### Frontend
```bash
# Executar em desenvolvimento
npm run dev

# Build para produção
npm run build

# Preview da build
npm run preview

# Linting
npm run lint

# Formatação
npm run format
```

### Docker
```bash
# Iniciar banco de dados
docker-compose up -d

# Parar banco de dados
docker-compose down

# Ver logs do banco
docker-compose logs db

# Remover volumes (cuidado: apaga dados)
docker-compose down -v
```

## 🐛 Solução de Problemas

### Erro de Conexão com Banco

**Sintomas:** Erro ao executar `dotnet run`

**Soluções:**
1. Verifique se o PostgreSQL está rodando:
   ```bash
   # Docker
   docker ps
   
   # Linux/macOS
   sudo systemctl status postgresql
   ```

2. Teste a conexão:
   ```bash
   psql -h localhost -p 5433 -U dsam_user -d dsam
   ```

3. Verifique as credenciais no `appsettings.json`

4. Execute as migrações:
   ```bash
   dotnet ef database update
   ```

### Erro de CORS

**Sintomas:** Erro no console do navegador sobre CORS

**Soluções:**
1. Verifique se o frontend está na porta 3000
2. Confirme se o backend está na porta 5195
3. Verifique a configuração CORS no `CorsMiddleware.cs`

### Erro de Autenticação

**Sintomas:** Erro 401 ao fazer login

**Soluções:**
1. Verifique se o token JWT está sendo enviado
2. Confirme a chave JWT no `appsettings.json`
3. Verifique se o usuário existe no banco

### Erro no Frontend

**Sintomas:** Página não carrega ou erros no console

**Soluções:**
1. Verifique se todas as dependências estão instaladas:
   ```bash
   npm install
   ```

2. Limpe o cache:
   ```bash
   npm run build
   rm -rf node_modules
   npm install
   ```

3. Verifique se o proxy está configurado corretamente no `vite.config.js`

## 📊 Verificações Finais

Após a instalação, verifique se:

- [ ] Backend está rodando em `http://localhost:5195`
- [ ] Frontend está rodando em `http://localhost:3000`
- [ ] Banco de dados está acessível
- [ ] É possível criar contas de médico e paciente
- [ ] É possível fazer login
- [ ] É possível cadastrar horários (médico)
- [ ] É possível agendar consultas (paciente)
- [ ] É possível visualizar agendamentos

## 📞 Suporte

Se encontrar problemas:

1. Verifique os logs do console
2. Consulte a documentação da API em `http://localhost:5195/swagger`
3. Abra uma issue no repositório
4. Entre em contato: contatomarlisson@gmail.com

---

**Boa sorte com a instalação! 🚀** 

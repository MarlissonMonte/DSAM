# Documentação da API - Sistema de Agendamentos Médicos

## 📋 Visão Geral

A API do Sistema de Agendamentos Médicos (DSAM) é construída com **.NET 8** e fornece endpoints para autenticação, gerenciamento de agendamentos e horários médicos.

**URL Base:** `http://localhost:5195/api`

**Documentação Swagger:** `http://localhost:5195/swagger`

## 🔐 Autenticação

A API utiliza **JWT (JSON Web Tokens)** para autenticação. Todos os endpoints protegidos requerem o header:

```
Authorization: Bearer <token>
```

### Endpoints de Autenticação

#### POST /api/auth/login
**Descrição:** Autenticar usuário

**Request Body:**
```json
{
  "email": "usuario@exemplo.com",
  "password": "senha123"
}
```

**Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "Dr. João Silva",
    "email": "joao@exemplo.com",
    "phone": "(11) 99999-9999",
    "userType": "Doctor"
  }
}
```

#### POST /api/auth/register
**Descrição:** Registrar novo usuário

**Request Body:**
```json
{
  "name": "Dr. João Silva",
  "email": "joao@exemplo.com",
  "password": "senha123",
  "phone": "(11) 99999-9999",
  "userType": "Doctor"
}
```

**Response (201):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "Dr. João Silva",
    "email": "joao@exemplo.com",
    "phone": "(11) 99999-9999",
    "userType": "Doctor"
  }
}
```

## 👨‍⚕️ Gerenciamento de Horários (Médicos)

### Endpoints de Schedule

#### POST /api/schedule
**Descrição:** Criar novo horário disponível (apenas médicos)

**Headers:**
```
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "startTime": "08:00:00",
  "endTime": "18:00:00",
  "durationMinutes": 30
}
```

**Response (201):**
```json
{
  "id": 1,
  "doctorId": 1,
  "doctorName": "Dr. João Silva",
  "startTime": "08:00:00",
  "endTime": "18:00:00",
  "durationMinutes": 30,
  "isAvailable": true,
  "availableTimeSlots": [
    {
      "startTime": "08:00:00",
      "endTime": "08:30:00",
      "isAvailable": true
    }
  ]
}
```

#### PUT /api/schedule/{id}
**Descrição:** Atualizar horário existente (apenas médicos)

**Request Body:**
```json
{
  "startTime": "09:00:00",
  "endTime": "17:00:00",
  "durationMinutes": 45
}
```

#### DELETE /api/schedule/{id}
**Descrição:** Excluir horário (apenas médicos)

#### GET /api/schedule
**Descrição:** Listar horários disponíveis

**Query Parameters:**
- `doctorId` (opcional): Filtrar por médico
- `availableOnly` (opcional): Apenas horários disponíveis

#### GET /api/schedule/{id}
**Descrição:** Obter horário específico

#### GET /api/schedule/available-slots/{doctorId}/{date}
**Descrição:** Obter slots disponíveis para um médico em uma data

**URL:** `/api/schedule/available-slots/1/2024-01-15`

**Response:**
```json
[
  {
    "startTime": "08:00:00",
    "endTime": "08:30:00",
    "isAvailable": true
  },
  {
    "startTime": "08:30:00",
    "endTime": "09:00:00",
    "isAvailable": false
  }
]
```

## 📅 Gerenciamento de Agendamentos

### Endpoints de Appointment

#### POST /api/appointment
**Descrição:** Criar novo agendamento (apenas pacientes)

**Request Body:**
```json
{
  "doctorId": 1,
  "scheduleId": 1,
  "appointmentDate": "2024-01-15",
  "appointmentTime": "08:00:00",
  "notes": "Consulta de rotina"
}
```

**Response (201):**
```json
{
  "id": 1,
  "doctorId": 1,
  "doctorName": "Dr. João Silva",
  "patientId": 2,
  "patientName": "Maria Santos",
  "scheduleId": 1,
  "appointmentDate": "2024-01-15",
  "appointmentTime": "08:00:00",
  "notes": "Consulta de rotina",
  "status": "Scheduled",
  "createdAt": "2024-01-10T10:00:00Z"
}
```

#### PUT /api/appointment/{id}
**Descrição:** Atualizar agendamento

**Request Body:**
```json
{
  "appointmentDate": "2024-01-16",
  "appointmentTime": "09:00:00",
  "notes": "Consulta de rotina - reagendada",
  "status": "Confirmed"
}
```

#### DELETE /api/appointment/{id}
**Descrição:** Cancelar agendamento

#### GET /api/appointment
**Descrição:** Listar agendamentos

**Query Parameters:**
- `doctorId` (opcional): Filtrar por médico
- `patientId` (opcional): Filtrar por paciente
- `startDate` (opcional): Data inicial
- `endDate` (opcional): Data final
- `status` (opcional): Status do agendamento

#### GET /api/appointment/{id}
**Descrição:** Obter agendamento específico

#### GET /api/appointment/my-appointments
**Descrição:** Obter agendamentos do usuário logado

## 👥 Gerenciamento de Usuários

### Endpoints de User

#### GET /api/user/doctors
**Descrição:** Listar todos os médicos

**Response:**
```json
[
  {
    "id": 1,
    "name": "Dr. João Silva",
    "email": "joao@exemplo.com",
    "phone": "(11) 99999-9999",
    "userType": "Doctor"
  }
]
```

## 📊 Status de Agendamentos

Os agendamentos podem ter os seguintes status:

- **Scheduled**: Agendado
- **Confirmed**: Confirmado
- **Completed**: Concluído
- **Cancelled**: Cancelado
- **NoShow**: Não compareceu

## 🔒 Tipos de Usuário

- **Doctor**: Médico (pode criar horários e ver agendamentos)
- **Patient**: Paciente (pode agendar consultas)

## 📝 Códigos de Resposta HTTP

- **200 OK**: Sucesso
- **201 Created**: Recurso criado com sucesso
- **400 Bad Request**: Dados inválidos
- **401 Unauthorized**: Não autenticado
- **403 Forbidden**: Sem permissão
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro interno do servidor

## 🐛 Tratamento de Erros

### Exemplo de Erro
```json
{
  "message": "Email ou senha inválidos"
}
```

### Validações Comuns

**Campos Obrigatórios:**
- `name`: Nome do usuário (máx. 100 caracteres)
- `email`: Email válido (máx. 100 caracteres)
- `password`: Senha (mín. 6 caracteres)
- `phone`: Telefone (máx. 20 caracteres)
- `userType`: Tipo de usuário (Doctor/Patient)

**Regras de Negócio:**
- Email deve ser único
- Médicos podem criar apenas um horário por vez
- Pacientes não podem agendar horários já ocupados
- Agendamentos só podem ser cancelados pelo paciente ou médico responsável

## 🔧 Configuração

### Variáveis de Ambiente

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=dsam;Username=dsam_user;Password=dsam_password"
  },
  "Jwt": {
    "Key": "your-super-secret-key-with-at-least-32-characters",
    "Issuer": "MedicalAppointmentsAPI",
    "Audience": "MedicalAppointmentsClient"
  }
}
```

### CORS

Configurado para permitir:
- `http://localhost:3000`
- `http://localhost:8080`
- `http://localhost:5173`

## 📚 Exemplos de Uso

### Criar Conta de Médico
```bash
curl -X POST http://localhost:5195/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Dr. João Silva",
    "email": "joao@exemplo.com",
    "password": "senha123",
    "phone": "(11) 99999-9999",
    "userType": "Doctor"
  }'
```

### Fazer Login
```bash
curl -X POST http://localhost:5195/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao@exemplo.com",
    "password": "senha123"
  }'
```

### Criar Horário (com token)
```bash
curl -X POST http://localhost:5195/api/schedule \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "startTime": "08:00:00",
    "endTime": "18:00:00",
    "durationMinutes": 30
  }'
```

### Agendar Consulta (com token)
```bash
curl -X POST http://localhost:5195/api/appointment \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "doctorId": 1,
    "scheduleId": 1,
    "appointmentDate": "2024-01-15",
    "appointmentTime": "08:00:00",
    "notes": "Consulta de rotina"
  }'
```

## 📞 Suporte

Para dúvidas sobre a API:

1. Consulte a documentação Swagger: `http://localhost:5195/swagger`
2. Verifique os logs do servidor
3. Entre em contato: kaua@voxpopi.com

---

**API desenvolvida com ❤️ por Marlisson Monte** 
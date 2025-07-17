# Sistema de Agendamentos Médicos (DSAM)

Sistema completo de agendamentos médicos desenvolvido com
**.NET 8** (Backend),
**Vue.js 3** (Frontend) e 
**PostgreSQL** (banco de dados).

## 🚀 Funcionalidades

### Para Médicos:
- Cadastrar e atualizar horários disponíveis para consultas 
- Visualizar agendamentos realizados com informações dos pacientes
- Gerenciar conflitos de horário automaticamente
- Definir duração das consultas (padrão: 30 minutos)

### Para Pacientes:
- Consultar horários disponíveis dos médicos
- Realizar agendamentos de consultas
- Visualizar agendamentos passados e futuros
- Cancelar agendamentos

### Sistema:
- Autenticação JWT segura
- Interface responsiva, moderna e cores harmônicas
- Validação de dados em tempo real
- Notificações de alterações e cancelamentos
- Dashboard com estatísticas personalizadas

## 🛠️ Tecnologias Utilizadas

### Backend:
- **.NET 8** - Framework principal
- **Entity Framework Core 9.0** - ORM para banco de dados
- **PostgreSQL 15** - Banco de dados
- **JWT** - Autenticação
- **BCrypt.Net-Next** - Criptografia de senhas
- **Swagger** - Documentação da API
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validação de dados

### Frontend:
- **Vue.js 3.4** - Framework JavaScript
- **TypeScript 5.3** - Tipagem estática
- **Pinia 2.1** - Gerenciamento de estado
- **Vue Router 4.2** - Roteamento
- **Element Plus 2.4** - UI Components
- **Axios 1.6** - Cliente HTTP
- **Day.js 1.11** - Manipulação de datas
- **Vite 5.0** - Build tool

## 📋 Pré-requisitos

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** - [Download](https://nodejs.org/)
- **PostgreSQL 15+** - [Download](https://www.postgresql.org/download/)
- **Git** - [Download](https://git-scm.com/)
- **Docker** (opcional) - [Download](https://www.docker.com/)

## 🚀 Como Executar

### 1. Clone o repositório
```bash
git clone https://github.com/MarlissonMonte/DSAM.git
cd DSAM
```

### 2. Configurar o Banco de Dados

#### Opção A: Usando Docker (Recomendado)
```bash
docker-compose up -d
```

#### Opção B: Instalação Manual
- Windows: Baixe e instale do site oficial
- Linux: `sudo apt-get install postgresql postgresql-contrib`
- macOS: `brew install postgresql`

#### Criar banco de dados:
```sql
CREATE DATABASE dsam;
CREATE USER dsam_user WITH PASSWORD 'dsam_password';
GRANT ALL PRIVILEGES ON DATABASE dsam TO dsam_user;
```

### 3. Configurar o Backend

#### Navegar para o diretório do backend:
```bash
cd app.api
```

#### Configurar connection string:
O arquivo `appsettings.json` já está configurado para:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=dsam;Username=dsam_user;Password=dsam_password"
  }
}
```

#### Instalar dependências e executar:
```bash
dotnet restore
dotnet run
```

O backend estará disponível em: `http://localhost:5195`
Documentação da API: `http://localhost:5195/swagger`

### 4. Configurar o Frontend

#### Navegar para o diretório do frontend:
```bash
cd medical-appointments-frontend
```

#### Instalar dependências:
```bash
npm install
```

#### Executar em modo desenvolvimento:
```bash
npm run dev
```

O frontend estará disponível em: `http://localhost:3000`

## 📁 Estrutura do Projeto

```
DSAM/
├── app.api/                    # Backend .NET 8
│   ├── Controllers/           # Controladores da API
│   │   ├── AuthController.cs
│   │   ├── AppointmentController.cs
│   │   ├── ScheduleController.cs
│   │   └── UserController.cs
│   ├── Data/                  # Contexto do Entity Framework
│   │   └── ApplicationDbContext.cs
│   ├── DTOs/                  # Data Transfer Objects
│   │   ├── AppointmentDTOs.cs
│   │   ├── AuthDTOs.cs
│   │   └── ScheduleDTOs.cs
│   ├── Models/                # Modelos de dados
│   │   ├── User.cs
│   │   ├── Appointment.cs
│   │   └── Schedule.cs
│   ├── Services/              # Lógica de negócio
│   │   ├── AuthService.cs
│   │   ├── AppointmentService.cs
│   │   └── ScheduleService.cs
│   ├── Middleware/            # Middlewares personalizados
│   │   └── CorsMiddleware.cs
│   └── Migrations/            # Migrações do banco
├── medical-appointments-frontend/  # Frontend Vue.js 3
│   ├── src/
│   │   ├── components/        # Componentes Vue
│   │   │   └── NavBar.vue
│   │   ├── views/             # Páginas da aplicação
│   │   │   ├── HomeView.vue
│   │   │   ├── LoginView.vue
│   │   │   ├── RegisterView.vue
│   │   │   ├── AppointmentsView.vue
│   │   │   ├── BookAppointmentView.vue
│   │   │   ├── SchedulesView.vue
│   │   │   └── ProfileView.vue
│   │   ├── stores/            # Stores Pinia
│   │   │   ├── auth.ts
│   │   │   └── appointments.ts
│   │   ├── services/          # Serviços de API
│   │   │   └── api.ts
│   │   ├── types/             # Tipos TypeScript
│   │   │   └── index.ts
│   │   ├── router/            # Configuração de rotas
│   │   │   └── index.ts
│   │   └── assets/            # Recursos estáticos
│   │       └── main.css
│   └── public/                # Arquivos estáticos
├── docker-compose.yml         # Configuração Docker
└── README.md
```

## 🔧 Configurações Importantes

### Backend (.NET):
- **Porta**: 5195 (configurável em `launchSettings.json`)
- **Banco de dados**: PostgreSQL na porta 5433
- **Autenticação**: JWT com expiração configurável
- **CORS**: Configurado para permitir frontend na porta 3000

### Frontend (Vue.js):
- **Porta**: 3000
- **Proxy**: Configurado para redirecionar `/api` para backend
- **UI Framework**: Element Plus
- **Estado**: Pinia
- **Build Tool**: Vite

## 🧪 Testando o Sistema

### 1. Criar conta de médico:
- Acesse: `http://localhost:3000/register`
- Selecione "Médico"
- Preencha os dados e cadastre

### 2. Criar conta de paciente:
- Acesse: `http://localhost:3000/register`
- Selecione "Paciente"
- Preencha os dados e cadastre

### 3. Testar funcionalidades:
- **Como médico**: 
  - Acesse `/schedules` para cadastrar horários disponíveis
  - Visualize agendamentos em `/appointments`
- **Como paciente**: 
  - Acesse `/book-appointment` para agendar consultas
  - Visualize seus agendamentos em `/appointments`

## 🔒 Segurança

- **Senhas**: Criptografadas com BCrypt
- **Tokens**: JWT com expiração configurável
- **Validação**: Dados validados no backend e frontend
- **CORS**: Configurado para permitir apenas origens específicas
- **Autenticação**: Baseada em claims JWT

## 📱 Responsividade

O sistema é totalmente responsivo e funciona em:
- Desktop (1024px+)
- Tablet (768px - 1023px)
- Mobile (< 768px)

## 🚀 Deploy

### Backend (Produção):
```bash
cd app.api
dotnet publish -c Release
```

### Frontend (Produção):
```bash
cd medical-appointments-frontend
npm run build
```

## 🔧 Comandos Úteis

### Backend:
```bash
# Restaurar dependências
dotnet restore

# Executar em desenvolvimento
dotnet run

# Executar testes
dotnet test

# Criar migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update
```

### Frontend:
```bash
# Instalar dependências
npm install

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

## 🐛 Solução de Problemas

### Erro de conexão com banco:
1. Verifique se o PostgreSQL está rodando
2. Confirme as credenciais no `appsettings.json`
3. Execute `dotnet ef database update`

### Erro de CORS:
1. Verifique se o frontend está na porta 3000
2. Confirme a configuração CORS no `CorsMiddleware.cs`

### Erro de autenticação:
1. Verifique se o token JWT está sendo enviado
2. Confirme a chave JWT no `appsettings.json`

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 📞 Suporte

Para dúvidas ou problemas:
- Abra uma issue no repositório
- Entre em contato: contatomarlisson@gmail.com

## 👨‍💻 Autor

**Marlisson Monte**
- GitHub: [@MarlissonMonte](https://github.com/MarlissonMonte)

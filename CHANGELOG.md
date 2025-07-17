# Changelog - Sistema de Agendamentos Médicos (DSAM)

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Versionamento Semântico](https://semver.org/lang/pt-BR/).

## [1.0.0] - 2024-01-15

### Adicionado
- Sistema completo de agendamentos médicos
- Autenticação JWT para médicos e pacientes
- Interface responsiva com Vue.js 3 e Element Plus
- API REST com .NET 8 e Entity Framework Core
- Banco de dados PostgreSQL com Docker
- Gerenciamento de horários médicos
- Sistema de agendamentos com validações
- Dashboard com estatísticas personalizadas
- Perfis de usuário (médico/paciente)
- Sistema de navegação com Vue Router
- Gerenciamento de estado com Pinia
- Validação de dados em tempo real
- Interface moderna e intuitiva
- Documentação completa da API
- Configuração CORS para desenvolvimento
- Sistema de migrações do banco de dados

### Funcionalidades Principais

#### Para Médicos
- Cadastro e login de médicos
- Gerenciamento de horários disponíveis
- Visualização de agendamentos
- Configuração de duração das consultas
- Dashboard com estatísticas

#### Para Pacientes
- Cadastro e login de pacientes
- Visualização de médicos disponíveis
- Agendamento de consultas
- Cancelamento de agendamentos
- Histórico de consultas

#### Sistema
- Autenticação segura com JWT
- Validação de conflitos de horário
- Interface responsiva para mobile/desktop
- Notificações de sucesso/erro
- Sistema de rotas protegidas

### Tecnologias Utilizadas

#### Backend
- .NET 8
- Entity Framework Core 9.0
- PostgreSQL 15
- JWT Authentication
- BCrypt.Net-Next
- Swagger/OpenAPI
- AutoMapper
- FluentValidation

#### Frontend
- Vue.js 3.4
- TypeScript 5.3
- Pinia 2.1
- Vue Router 4.2
- Element Plus 2.4
- Axios 1.6
- Day.js 1.11
- Vite 5.0

#### Infraestrutura
- Docker Compose
- PostgreSQL 15
- Git

### Arquivos Principais

#### Backend
- `app.api/Controllers/` - Controladores da API
- `app.api/Models/` - Modelos de dados
- `app.api/Services/` - Lógica de negócio
- `app.api/Data/` - Contexto do Entity Framework
- `app.api/DTOs/` - Data Transfer Objects
- `app.api/Middleware/` - Middlewares personalizados

#### Frontend
- `medical-appointments-frontend/src/views/` - Páginas da aplicação
- `medical-appointments-frontend/src/components/` - Componentes Vue
- `medical-appointments-frontend/src/stores/` - Stores Pinia
- `medical-appointments-frontend/src/services/` - Serviços de API
- `medical-appointments-frontend/src/types/` - Tipos TypeScript

### Configuração

#### Banco de Dados
- PostgreSQL na porta 5433
- Database: dsam
- Usuário: dsam_user
- Senha: dsam_password

#### Backend
- Porta: 5195
- Swagger: http://localhost:5195/swagger
- CORS configurado para frontend

#### Frontend
- Porta: 3000
- Proxy configurado para backend
- Build tool: Vite

### Documentação
- README.md - Documentação principal
- INSTALACAO.md - Guia de instalação detalhado
- API_DOCUMENTATION.md - Documentação da API
- CHANGELOG.md - Histórico de mudanças

### Próximas Versões

#### [1.1.0] - Planejado
- [ ] Notificações por email
- [ ] Sistema de avaliações
- [ ] Histórico médico
- [ ] Prescrições digitais
- [ ] Upload de documentos

#### [1.2.0] - Planejado
- [ ] Chat entre médico e paciente
- [ ] Relatórios avançados
- [ ] Integração com sistemas externos
- [ ] App mobile (React Native)

#### [2.0.0] - Planejado
- [ ] Múltiplas especialidades médicas
- [ ] Sistema de pagamentos
- [ ] Telemedicina
- [ ] IA para sugestões de horários

---

## Como Contribuir

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Suporte

Para dúvidas ou problemas:
- Abra uma issue no repositório
- Entre em contato: kaua@voxpopi.com

---

**Desenvolvido com ❤️ por Marlisson Monte** 
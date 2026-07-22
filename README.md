# Order Management API

Backend de gestão de pedidos para e-commerce, desenvolvido como teste técnico para desenvolvedor .NET Senior.

---

## Decisões Arquiteturais

### Controllers vs Minimal API
Optei por **Controllers** por se encaixar melhor no contexto de Clean Architecture com CQRS. Controllers oferecem melhor organização por recurso, herança de `ControllerBase` para padronizar respostas HTTP, e são mais familiares em projetos de médio/grande porte onde convenção importa.

### Repositório específico vs `IRepository<T>` genérico
Utilizei `IOrderRepository` com métodos específicos do domínio (`GetPagedAsync`, `GetByIdAsync`) em vez de um repositório genérico. Repositórios genéricos frequentemente vazam abstrações de infraestrutura (como `IQueryable`) para a camada de aplicação, violando Clean Architecture. Com um repositório focado, o contrato reflete exatamente as necessidades do domínio.

### Factory de mapeamento vs AutoMapper
Utilizei `OrderFactory` estático para conversão de entidades para DTOs. O projeto tem poucas entidades e o mapeamento é simples — adicionar AutoMapper seria over-engineering sem benefício real.

---

## Stack

- **.NET 10** com ASP.NET Core (Controllers)
- **Clean Architecture**: Domain → Application → Infrastructure → IoC → API
- **CQRS com MediatR 12**: Commands e Queries separados
- **Entity Framework Core 10 + SQLite**: migrations aplicadas automaticamente na inicialização
- **FluentValidation**: pipeline behavior no MediatR para validação automática
- **JWT Authentication**: endpoint de login com usuário fixo
- **Serilog**: logging estruturado + pipeline behavior de request/response
- **OpenTelemetry**: rastreamento básico com export para console
- **xUnit + Moq + FluentAssertions**: testes unitários dos handlers e domínio

---

## Estrutura do Projeto

```
├── Api/
│   └── OrderManagement.Api/          # Camada de apresentação (Controllers, Program.cs)
├── Library/
│   ├── OrderManagement.Domain/       # Entidades, Enums, regras de negócio
│   ├── OrderManagement.Application/  # CQRS, DTOs, Factories, Behaviors, Interfaces
│   ├── OrderManagement.Infrastructure/ # EF Core, Repositório, Migrations
│   └── OrderManagement.IoC/          # Registro de dependências
├── Tests/
│   └── OrderManagement.Tests/        # Testes unitários (Handlers, Domain, Validators)
├── Dockerfile
└── docker-compose.yml
```

### CQRS (Application/CQRS)
```
CQRS/
├── Commands/
│   └── Orders/
│       ├── CreateOrderCommand, CreateOrderHandler, CreateOrderValidator
│       └── CancelOrderCommand, CancelOrderHandler, CancelOrderValidator
└── Queries/
    └── Orders/
        ├── GetOrderByIdQuery, GetOrderByIdHandler
        └── GetOrdersQuery, GetOrdersHandler
```

---

## Endpoints

| Método | Rota                        | Descrição                          | Auth |
|--------|-----------------------------|------------------------------------|------|
| POST   | /auth/login                 | Retorna JWT                        | Não  |
| POST   | /api/orders                 | Cria um novo pedido                | Sim  |
| GET    | /api/orders?page=1&pageSize=10 | Lista pedidos com paginação    | Sim  |
| GET    | /api/orders/{id}            | Retorna pedido por ID              | Sim  |
| PATCH  | /api/orders/{id}/cancel     | Cancela um pedido                  | Sim  |

**Credenciais de login:** `dev@martech.com` / `Senha@123`

---

## Rodando Localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Passos

```bash
# Clone o repositório
git clone <url-do-repo>
cd nttdata-gestao-de-pedidos

# Restaurar dependências
dotnet restore

# Executar a API (migrations aplicadas automaticamente)
dotnet run --project Api/OrderManagement.Api

# A API estará disponível em: http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

### Rodar os Testes

```bash
dotnet test Tests/OrderManagement.Tests/OrderManagement.Tests.csproj
```

---

## Rodando via Docker

### Pré-requisitos
- [Docker](https://www.docker.com/) e Docker Compose instalados

### Passos

```bash
# Subir o ambiente completo
docker-compose up --build

# A API estará disponível em: http://localhost:10000
# Swagger UI: http://localhost:10000/swagger
```

---

## Regras de Negócio

- Um pedido deve ter pelo menos 1 item
- `UnitPrice` e `Quantity` devem ser maiores que zero
- Apenas pedidos com status `Pending` podem ser cancelados
- `TotalAmount` é calculado no domínio: `sum(UnitPrice × Quantity)`

Repostório de teste prático para medir conhecimentos técnicos - NTT DATA

# credito-cadastro (Cadastro de Clientes)

Microsserviço de entrada do fluxo de concessão de crédito. Valida CPF/e-mail, persiste o cliente e publica o evento `ClienteCadastrado` via **outbox + RabbitMQ**.

## Stack

- .NET 10 / ASP.NET Core
- EF Core + **SQL Server**
- RabbitMQ (mensageria)
- Polly (retry no publish)
- xUnit + FluentAssertions + NSubstitute
- Docker Compose (API + SQL Server + RabbitMQ)

## Subir tudo (recomendado)

```bash
docker compose up -d --build
```

Isso sobe:

| Serviço | URL / acesso |
|---------|----------------|
| API | http://localhost:8080 |
| Swagger | http://localhost:8080/swagger |
| Health | http://localhost:8080/health |
| RabbitMQ UI | http://localhost:15672 (guest/guest) |
| SQL Server | `localhost,1433` · sa / `Your_strong_Password123` |

Logs da API:

```bash
docker compose logs -f api
```

Parar:

```bash
docker compose down
```

## Rodar só a API no host (opcional)

```bash
docker compose up -d sqlserver rabbitmq
dotnet run --project Api
```

API local: http://localhost:5214

## Endpoints

- `POST /api/v1/clientes`
- `GET /api/v1/clientes/{id}`
- `GET /health`

Na subida, as migrations rodam automaticamente (`MigrateDatabaseAsync`).

## Decisões

- **Outbox** na mesma transação do cadastro → evita double-write.
- **Database per service** (SQL Server dedicado a este microsserviço).
- Publisher atrás de `IMessageBus` → ASB poderia ser outra implementação sem mudar o domínio.
- Compose único: infra + API na mesma rede Docker.

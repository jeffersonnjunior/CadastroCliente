# credito-cadastro (Cadastro de Clientes)

Microsserviço de entrada do fluxo de concessão de crédito. Valida CPF/e-mail, persiste o cliente e publica `ClienteCadastrado` via **outbox + RabbitMQ**.

## Diagrama

Fluxo ponta a ponta da concessão de crédito (Excalidraw):  
[`docs/fluxo-concessao-credito.excalidraw`](docs/fluxo-concessao-credito.excalidraw)

Abra em [excalidraw.com](https://excalidraw.com) → **Open** → selecione o arquivo.

## Processos

| Projeto | Responsabilidade |
|---------|------------------|
| **Api** | REST `POST/GET /api/v1/clientes` + health + outbox publisher |

> Neste BC a mensageria (outbox) roda no mesmo host da Api.

## Stack

- .NET 10 / ASP.NET Core
- EF Core + SQL Server (database `CadastroCliente`)
- RabbitMQ (outbox + Polly retry no publish)
- xUnit + FluentAssertions + NSubstitute (padrão AAA)

## Subir

```bash
docker compose up -d --build
```

| Serviço | URL / acesso |
|---------|----------------|
| API Cadastro | http://localhost:8080 |
| Health | http://localhost:8080/health |
| OpenAPI | http://localhost:8080/openapi/v1.json |
| RabbitMQ UI | http://localhost:15672 (credito / credito123) |
| SQL Server | `localhost,1433` · sa / `Your_strong_Password123` |

Local (infra no Docker):

```bash
docker compose up -d sqlserver rabbitmq
dotnet run --project Api
```

## Endpoints

- `POST /api/v1/clientes`
- `GET /api/v1/clientes/{id}`
- `GET /health`

Na subida, as migrations rodam automaticamente.

## Fluxo

```
POST /clientes → SQL (cliente + outbox) → ClienteCadastrado
  → exchange credito.clientes / cliente.cadastrado
```

## Testes

```bash
dotnet test Tests/CadastroCliente.Tests.csproj
```

## Decisões

- **Outbox** na mesma transação do cadastro → evita double-write.
- **Database per service** (`CadastroCliente`).
- Publisher atrás de `IMessageBus` (ASB poderia ser outra implementação).
- Este compose sobe a **infra compartilhada** (SQL + Rabbit) usada pelos outros BCs.

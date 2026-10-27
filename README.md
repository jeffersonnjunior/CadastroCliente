# credito-cadastro (Cadastro de Clientes)

Microsserviço de entrada do fluxo de concessão de crédito. Valida CPF/e-mail, persiste o cliente e publica o evento `ClienteCadastrado` via **outbox + RabbitMQ**.

## Stack

- .NET 10 / ASP.NET Core
- EF Core + **SQL Server**
- RabbitMQ (mensageria)
- Polly (retry no publish)
- xUnit + FluentAssertions + NSubstitute

## Subir infra (canônico deste repo)

```bash
docker compose -f docker-compose.infra.yml up -d
```

- RabbitMQ UI: http://localhost:15672 (guest/guest)
- SQL Server: `localhost,1433` · user `sa` · senha `Your_strong_Password123`

## Rodar API

```bash
dotnet run --project Api
```

Na subida, `EnsureCreated` cria o database/tabelas `CadastroCliente` se ainda não existirem.

- Health: `GET /health`
- Cadastro: `POST /api/v1/clientes`
- Consulta: `GET /api/v1/clientes/{id}`

Sem Rabbit no ar, o outbox permanece pendente e o processor retenta (até 5 tentativas por mensagem). Para silenciar publish: `RabbitMq__Enabled=false`.

## Connection string

```text
ConnectionStrings__Default=Server=localhost,1433;Database=CadastroCliente;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;Encrypt=False
```

## Decisões

- **Outbox** na mesma transação do cadastro → evita double-write.
- **Database per service** (SQL Server dedicado a este microsserviço).
- Publisher atrás de `IMessageBus` → ASB poderia ser outra implementação sem mudar o domínio.

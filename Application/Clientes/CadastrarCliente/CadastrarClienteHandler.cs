using System.Text.Json;
using Application.Abstractions;
using Application.Events;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Clientes.CadastrarCliente;

public sealed class CadastrarClienteHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IClienteRepository _clientes;
    private readonly IOutboxWriter _outbox;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarClienteHandler(
        IClienteRepository clientes,
        IOutboxWriter outbox,
        IUnitOfWork unitOfWork)
    {
        _clientes = clientes;
        _outbox = outbox;
        _unitOfWork = unitOfWork;
    }

    public async Task<ClienteResponse> HandleAsync(
        CadastrarClienteCommand command,
        CancellationToken cancellationToken = default)
    {
        var cliente = Cliente.Criar(command.Nome, command.Cpf, command.Email);

        if (await _clientes.ExistsByCpfAsync(cliente.Cpf, cancellationToken))
            throw new CpfDuplicadoException(cliente.Cpf.Value);

        await _clientes.AddAsync(cliente, cancellationToken);

        var eventId = Guid.NewGuid();
        var @event = new ClienteCadastradoEvent(
            EventId: eventId,
            EventType: "ClienteCadastrado",
            SchemaVersion: 1,
            OccurredAt: DateTime.UtcNow,
            ClienteId: cliente.Id,
            Payload: new ClienteCadastradoPayload(
                cliente.Nome,
                cliente.Cpf.Value,
                cliente.Email.Value));

        var payloadJson = JsonSerializer.Serialize(@event, JsonOptions);
        await _outbox.EnqueueAsync("ClienteCadastrado", payloadJson, eventId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ClienteResponse(
            cliente.Id,
            cliente.Nome,
            cliente.Cpf.Value,
            cliente.Email.Value,
            cliente.CriadoEmUtc);
    }
}

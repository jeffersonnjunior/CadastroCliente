using Application.Abstractions;
using Application.Clientes.CadastrarCliente;

namespace Application.Clientes.ObterCliente;

public sealed class ObterClienteHandler
{
    private readonly IClienteRepository _clientes;

    public ObterClienteHandler(IClienteRepository clientes)
    {
        _clientes = clientes;
    }

    public async Task<ClienteResponse?> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _clientes.GetByIdAsync(id, cancellationToken);
        if (cliente is null)
            return null;

        return new ClienteResponse(
            cliente.Id,
            cliente.Nome,
            cliente.Cpf.Value,
            cliente.Email.Value,
            cliente.CriadoEmUtc);
    }
}

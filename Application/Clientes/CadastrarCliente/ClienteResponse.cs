namespace Application.Clientes.CadastrarCliente;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Cpf,
    string Email,
    DateTime CriadoEmUtc);

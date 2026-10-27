using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Cliente
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public Cpf Cpf { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = null!;
    public DateTime CriadoEmUtc { get; private set; }

    private Cliente()
    {
    }

    public static Cliente Criar(string? nome, string? cpf, string? email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");

        var nomeNormalizado = nome.Trim();
        if (nomeNormalizado.Length < 2)
            throw new DomainException("Nome deve ter ao menos 2 caracteres.");

        return new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = nomeNormalizado,
            Cpf = Cpf.Create(cpf),
            Email = EmailAddress.Create(email),
            CriadoEmUtc = DateTime.UtcNow
        };
    }
}

using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class ClienteRepository : IClienteRepository
{
    private readonly CadastroDbContext _db;

    public ClienteRepository(CadastroDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _db.Clientes.AddAsync(cliente, cancellationToken);
    }

    public Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _db.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByCpfAsync(Cpf cpf, CancellationToken cancellationToken = default)
    {
        // Comparar o VO inteiro (ValueConverter) — .Value não traduz no EF Core.
        return _db.Clientes.AnyAsync(x => x.Cpf == cpf, cancellationToken);
    }
}

using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCpfAsync(Cpf cpf, CancellationToken cancellationToken = default);
}

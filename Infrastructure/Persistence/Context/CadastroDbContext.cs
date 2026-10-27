using Domain.Entities;
using Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public sealed class CadastroDbContext : DbContext
{
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CadastroDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

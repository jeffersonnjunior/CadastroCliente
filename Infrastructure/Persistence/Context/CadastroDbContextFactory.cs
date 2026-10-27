using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence.Context;

/// <summary>
/// Usado pelo EF CLI para gerar migrations (dotnet ef migrations add ...).
/// </summary>
public sealed class CadastroDbContextFactory : IDesignTimeDbContextFactory<CadastroDbContext>
{
    public CadastroDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? "Server=localhost,1433;Database=CadastroCliente;User Id=sa;Password=Your_strong_Password123;TrustServerCertificate=True;Encrypt=False";

        var options = new DbContextOptionsBuilder<CadastroDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new CadastroDbContext(options);
    }
}

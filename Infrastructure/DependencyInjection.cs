using Application.Abstractions;
using Infrastructure.Messaging;
using Infrastructure.Messaging.RabbitMq;
using Infrastructure.Outbox;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "Connection string 'Default' não configurada. Defina ConnectionStrings:Default para o SQL Server.");

        services.AddDbContext<CadastroDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(CadastroDbContext).Assembly.FullName)));

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
        services.AddHostedService<OutboxProcessorHostedService>();

        return services;
    }
}

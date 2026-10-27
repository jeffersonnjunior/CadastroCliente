using Application.Clientes.CadastrarCliente;
using Application.Clientes.ObterCliente;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CadastrarClienteHandler>();
        services.AddScoped<ObterClienteHandler>();
        return services;
    }
}

using Application.Clientes.CadastrarCliente;
using Application.Clientes.ObterCliente;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class AddApplicationDependencyInjection
{
    public static void DependencyInjectionApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = configuration;

        services.AddScoped<CadastrarClienteHandler>();
        services.AddScoped<ObterClienteHandler>();
    }
}

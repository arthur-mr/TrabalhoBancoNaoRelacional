using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Servicos.Modulos;

namespace RestApiFurb.Dominio.Mediator.Modulos;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarInfraEstrutura(this IServiceCollection services, string connectionString)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CriarUsuarioComando).Assembly));
        services.AdicionarServicos(connectionString);

        return services;
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Servicos.Modulos;

namespace RestApiFurb.Dominio.Mediator.Modulos;

public static class InstalarDependencias
{
    public static void AdicionarInfraEstrutura(this IServiceCollection services, IConfiguration configuracoes)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CriarUsuarioComando).Assembly));
        services.AdicionarServicos(configuracoes);
    }
}
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Servicos.Servicos;
using RestApiFurb.Infra.Modulos;

namespace RestApiFurb.Dominio.Servicos.Modulos;

public static class InstalarDependencias
{
    public static IServiceCollection AdicionarServicos(this IServiceCollection services, string connectionString)
    {
        services.AdicionarBancoDeDados(connectionString);

        services.AddScoped<IUsuarioServico, UsuarioServico>();
        services.AddScoped<IComandaServico, ComandaServico>();
        services.AddScoped<IProdutoServico, ProdutoServico>();
        services.AddScoped<INotificadorServico, NotificadorServico>();
        services.AddScoped<IPublicadorMensagemServico, PublicadorMensagemServico>();

        return services;
    }
}
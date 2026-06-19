using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Servicos.Servicos;
using RestApiFurb.Infra.Modulos;

namespace RestApiFurb.Dominio.Servicos.Modulos;

public static class InstalarDependencias
{
    public static void AdicionarServicos(this IServiceCollection services, IConfiguration configuracoes)
    {
        services.AdicionarBancoDeDados(configuracoes);

        services.AddScoped<IUsuarioServico, UsuarioServico>();
        services.AddScoped<IComandaServico, ComandaServico>();
        services.AddScoped<IProdutoServico, ProdutoServico>();
        services.AddScoped<IOutboxServico, OutboxServico>();
        services.AddScoped<INotificadorServico, NotificadorServico>();
        services.AddScoped<IPublicadorMensagemServico, PublicadorMensagemServico>();

        var urlElastic = configuracoes["Elasticsearch:Url"];
        var defaultIndex = configuracoes["Elasticsearch:Index"];

        var settings = new ElasticsearchClientSettings(new Uri(urlElastic)).DefaultIndex(defaultIndex);
        var elasticClient = new ElasticsearchClient(settings);
        services.AddSingleton(elasticClient);
    }
}
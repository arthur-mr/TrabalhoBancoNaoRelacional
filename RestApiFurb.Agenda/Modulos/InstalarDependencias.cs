using RestApiFurb.Agenda;
using RestApiFurb.Agenda.Agendas;
using RestApiFurb.Dominio.Mediator.Modulos;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class InstalarDependencias
{
    public static void AdicionarDependenciasServico(this IServiceCollection servicos, IConfiguration configuracao)
    {
        servicos.AddHostedService<Processo>();
        servicos.AdicionarAgendaDeTarefas();
        servicos.AddScoped<ProcessarProdutosElasticsearchAgenda>();
        servicos.AdicionarInfraEstrutura(configuracao);
    }
}
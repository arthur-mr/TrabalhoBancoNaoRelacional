using Myrp.Framework.Agendador.Interfaces;
using Myrp.Framework.Hosting.Aplicacao.Bases;
using RestApiFurb.Agenda.Agendas;

namespace RestApiFurb.Agenda;

public class Processo : ProcessoBase<Processo>
{
    private readonly IAgenda agenda;
    private readonly IServiceScopeFactory fabricaEscopo;

    public Processo(
        ILogger<Processo> logger,
        IHostApplicationLifetime tempoDeVida,
        IAgenda agenda,
        IServiceScopeFactory fabricaEscopo,
        ILoggerFactory loggerFactory)
        : base(logger, tempoDeVida)
    {
        this.agenda = agenda;
        this.fabricaEscopo = fabricaEscopo;
    }

    protected override Task ExecutarAsync(CancellationToken cancellationToken)
    {
        agenda.AgendarTarefa<ProcessarProdutosElasticsearchAgenda>(x => x.Now().AndEvery(3).Seconds());

        agenda.IniciarAgenda();

        return Task.CompletedTask;
    }

    protected override void Iniciado()
    {
    }

    protected override void Parando()
    {
        agenda.Parar();
    }
}

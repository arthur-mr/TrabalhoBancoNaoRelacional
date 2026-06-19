using MediatR;
using Myrp.Framework.Agendador.Tarefas;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Agenda.Agendas;

internal sealed class ProcessarProdutosElasticsearchAgenda : TarefaAgendadaComSetupBase<Guid>
{
    private readonly IMediator mediator;

    protected override ushort QuantidadeProcessamentoEmParalelo => 1;

    public ProcessarProdutosElasticsearchAgenda(
        ILogger<TarefaAgendadaComSetupBase<Guid>> logger,
        IServiceScopeFactory fabricaEscopo,
        IMediator mediator)
        : base(logger, fabricaEscopo)
    {
        this.mediator = mediator;
    }

    protected override Task<IList<Guid>> ObterDadosParaProcessar(CancellationToken cancellationToken)
    {
        var consulta = new ObterProdutoOutboxConsulta();
        return mediator.Send(consulta);
    }

    protected override Task ExecutarOperacaoAsync(Guid dado, CancellationToken cancellationToken)
    {
        var comando = new SalvarProdutoOutboxComando(dado);
        return mediator.Send(comando);
    }
}
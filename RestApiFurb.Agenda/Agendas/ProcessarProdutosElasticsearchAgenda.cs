using MediatR;
using Myrp.Framework.Agendador.Tarefas;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Agenda.Agendas;

internal sealed class ProcessarProdutosElasticsearchAgenda : TarefaAgendadaBase
{
    private readonly IMediator mediator;

    public ProcessarProdutosElasticsearchAgenda(
        ILogger<TarefaAgendadaBase> logger,
        IMediator mediator)
        : base(logger)
    {
        this.mediator = mediator;
    }

    protected override async Task ExecutarOperacaoAsync(CancellationToken cancellationToken)
    {
        var consulta = new ObterProdutoOutboxConsulta();
        var dados = await mediator.Send(consulta);

        var comando = new SalvarProdutoOutboxComando(dados);
        await mediator.Send(comando);
    }
}
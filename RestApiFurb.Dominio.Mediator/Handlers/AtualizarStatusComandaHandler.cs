using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class AtualizarStatusComandaHandler : IRequestHandler<AtualizarStatusComandaComando>
{
    private readonly IComandaServico servico;

    public AtualizarStatusComandaHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(AtualizarStatusComandaComando request, CancellationToken cancellationToken)
    {
        return servico.AtualizarStatusComandaAsync(request.ComandaId, request.Status, cancellationToken);
    }
}
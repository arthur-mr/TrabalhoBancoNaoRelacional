using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal sealed class DeletarComandaHandler : IRequestHandler<DeletarComandaComando>
{
    private readonly IComandaServico servico;

    public DeletarComandaHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(DeletarComandaComando request, CancellationToken cancellationToken)
    {
        return servico.DeletarComandaAsync(request.Id, cancellationToken);
    }
}
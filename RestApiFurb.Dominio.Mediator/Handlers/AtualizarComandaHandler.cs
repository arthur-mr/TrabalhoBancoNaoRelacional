using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class AtualizarComandaHandler : IRequestHandler<AtualizarComandaComando>
{
    private readonly IComandaServico servico;

    public AtualizarComandaHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(AtualizarComandaComando request, CancellationToken cancellationToken)
    {
        return servico.AtualizarComandaAsync(request.Id, request.Contrato, cancellationToken);
    }
}
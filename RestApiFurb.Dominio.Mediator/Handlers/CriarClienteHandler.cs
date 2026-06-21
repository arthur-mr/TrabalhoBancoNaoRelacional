using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class CriarClienteHandler : IRequestHandler<CriarClienteComando>
{
    private readonly IClienteServico servico;

    public CriarClienteHandler(IClienteServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(CriarClienteComando request, CancellationToken cancellationToken)
    {
        return servico.CriarClienteAsync(request.Contrato, cancellationToken);
    }
}

using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class SalvarProdutoOutboxHandler : IRequestHandler<SalvarProdutoOutboxComando>
{
    private readonly IOutboxServico servico;

    public SalvarProdutoOutboxHandler(IOutboxServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(SalvarProdutoOutboxComando request, CancellationToken cancellationToken)
    {
        return servico.ProcessarMensagensEmMassaAsync(request.Ids, cancellationToken);
    }
}
using MediatR;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Servicos.Servicos;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class CriarProdutoHandler : IRequestHandler<CriarProdutoComando>
{
    private readonly IProdutoServico servico;

    public CriarProdutoHandler(IProdutoServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(CriarProdutoComando request, CancellationToken cancellationToken)
    {
        return servico.CriarProdutoAsync(request.Contrato, cancellationToken);
    }
}

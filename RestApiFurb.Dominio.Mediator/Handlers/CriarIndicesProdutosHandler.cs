using MediatR;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Servicos.Servicos;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class CriarIndicesProdutosHandler : IRequestHandler<CriarIndicesProdutoComando>
{
    private readonly IProdutoServico servico;

    public CriarIndicesProdutosHandler(IProdutoServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(CriarIndicesProdutoComando request, CancellationToken cancellationToken)
    {
        return servico.CriarIndiceProdutosAsync(cancellationToken);
    }
}
using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Servicos.Servicos;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class ObterProdutoPorIdHandler : IRequestHandler<ObterProdutoPorIdConsulta, ListarProdutoContrato>
{
    private readonly IProdutoServico servico;

    public ObterProdutoPorIdHandler(IProdutoServico servico)
    {
        this.servico = servico;
    }

    public Task<ListarProdutoContrato> Handle(ObterProdutoPorIdConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterPorIdAsync(request.ProdutoId, cancellationToken);
    }
}
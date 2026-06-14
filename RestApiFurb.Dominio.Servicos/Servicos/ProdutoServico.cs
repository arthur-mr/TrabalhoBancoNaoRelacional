using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class ProdutoServico : IProdutoServico
{
    private readonly IRepositorioBase<Produto> repositorioBase;

    public ProdutoServico(IRepositorioBase<Produto> repositorioBase)
    {
        this.repositorioBase = repositorioBase;
    }

    public async Task CriarProdutoAsync(CriarProdutoContrato contrato, CancellationToken cancellationToken)
    {
        var entidade = new Produto(
            nome: contrato.Nome,
            preco: contrato.Preco,
            codigo: contrato.Codigo,
            codigoBarras: contrato.CodigoBarras,
            categoria: contrato.Categoria,
            quantidadeEstoque: contrato.QuantidadeEstoque);

        await repositorioBase.SalvarAsync(entidade, cancellationToken);
    }
}

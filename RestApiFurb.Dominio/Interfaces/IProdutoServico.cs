using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

public interface IProdutoServico
{
    Task CriarProdutoAsync(CriarProdutoContrato contrato, CancellationToken cancellationToken);

    Task<ListarProdutoContrato> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task CriarIndiceProdutosAsync(CancellationToken cancellationToken);

    Task<IEnumerable<ProdutoAutocompleteContrato>> BuscarAutocompleteAsync(string termoBusca, CancellationToken cancellationToken);
}
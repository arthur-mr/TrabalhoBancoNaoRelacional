using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

public interface IProdutoServico
{
    Task CriarProdutoAsync(CriarProdutoContrato contrato, CancellationToken cancellationToken);
}
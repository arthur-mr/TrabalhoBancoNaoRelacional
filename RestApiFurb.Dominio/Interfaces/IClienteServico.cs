using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Interfaces;

public interface IClienteServico
{
    Task CriarClienteAsync(CriarClienteContrato contrato, CancellationToken cancellationToken);

    Task<IList<ListarClienteContrato>> ObterClientesAsync(FiltroClienteContrato filtroContrato, CancellationToken cancellationToken);
}
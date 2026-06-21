using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Interfaces;

public interface IComandaServico
{
    Task<ComandaCriadaContrato> CriarComandaAsync(CriarComandaContrato contrato, CancellationToken cancellationToken);

    Task<IList<ComandaCriadaContrato>> ObterTodasComandasAsync(CancellationToken cancellationToken);

    Task<ComandaCriadaContrato> ObterComandaPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task AtualizarComandaAsync(Guid id, AtualizarComandaContrato contrato, CancellationToken cancellationToken);

    Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken);

    Task AtualizarStatusComandaAsync(Guid comandaId, StatusComanda status, CancellationToken cancellationToken);
}
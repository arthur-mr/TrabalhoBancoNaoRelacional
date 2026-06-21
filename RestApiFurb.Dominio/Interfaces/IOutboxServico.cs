namespace RestApiFurb.Dominio.Interfaces;

public interface IOutboxServico
{
    Task<IList<Guid>> ObterMensagensPendentesAsync(CancellationToken cancellationToken);
    Task ProcessarMensagensEmMassaAsync(IList<Guid> ids, CancellationToken cancellationToken);
}
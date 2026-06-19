namespace RestApiFurb.Dominio.Interfaces;

public interface IOutboxServico
{
    Task<IList<Guid>> ObterMensagensPendentesAsync(CancellationToken cancellationToken);
    Task ProcessarMensagemAsync(Guid id, CancellationToken cancellationToken);
}
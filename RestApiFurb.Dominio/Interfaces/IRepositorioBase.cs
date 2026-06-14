using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Interfaces;

public interface IRepositorioBase<T> where T : ModeloBase
{
    IQueryable<T> MontarConsulta();

    Task<T> ObterPorIdAsync(Guid id);

    Task SalvarAsync(T modelo, CancellationToken cancellationToken);

    Task SalvarAsync(IList<T> modelos, CancellationToken cancellationToken);

    Task AtualizarAsync(T modelo, CancellationToken cancellationToken);

    Task DeletarAsync(Guid id, CancellationToken cancellationToken);
}

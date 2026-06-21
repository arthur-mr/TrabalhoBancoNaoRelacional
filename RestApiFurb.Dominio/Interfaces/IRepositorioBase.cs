using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Interfaces;

public interface IRepositorioBase
{
    IQueryable<T> MontarConsulta<T>() where T : ModeloBase;

    Task<T> ObterPorIdAsync<T>(Guid id) where T : ModeloBase;

    Task AdicionarAsync<T>(T modelo, CancellationToken cancellationToken) where T : ModeloBase;

    Task AdicionarAsync<T>(IList<T> modelos, CancellationToken cancellationToken) where T : ModeloBase;

    Task AtualizarAsync<T>(T modelo, CancellationToken cancellationToken) where T : ModeloBase;

    Task AtualizarAsync<T>(IList<T> modelos, CancellationToken cancellationToken) where T : ModeloBase;

    Task DeletarAsync<T>(Guid id, CancellationToken cancellationToken) where T : ModeloBase;

    Task ExecutarOperacoesEmTransacaoAsync(Func<IRepositorioBase, Task> acao, CancellationToken cancellationToken);
}
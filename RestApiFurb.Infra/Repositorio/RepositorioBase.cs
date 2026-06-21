using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos.Base;
using RestApiFurb.Infra.Contextos;

namespace RestApiFurb.Infra.Repositorio;

internal sealed class RepositorioBase : IRepositorioBase
{
    private readonly Contexto contexto;

    public RepositorioBase(Contexto contexto)
    {
        this.contexto = contexto;
    }

    public async Task<T> ObterPorIdAsync<T>(Guid id) where T : ModeloBase
    {
        return await contexto.Set<T>().FindAsync(new object[] { id });
    }

    public IQueryable<T> MontarConsulta<T>() where T : ModeloBase
    {
        return contexto.Set<T>().AsQueryable();
    }

    public async Task AdicionarAsync<T>(T modelo, CancellationToken cancellationToken) where T : ModeloBase
    {
        contexto.Set<T>().Add(modelo);
        if (contexto.Database.CurrentTransaction == null)
            await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task AdicionarAsync<T>(IList<T> modelos, CancellationToken cancellationToken) where T : ModeloBase
    {
        if (!modelos.Any()) return;

        contexto.Set<T>().AddRange(modelos);
        if (contexto.Database.CurrentTransaction == null)
            await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync<T>(T modelo, CancellationToken cancellationToken) where T : ModeloBase
    {
        contexto.Set<T>().Update(modelo);
        if (contexto.Database.CurrentTransaction == null)
            await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync<T>(IList<T> modelos, CancellationToken cancellationToken) where T : ModeloBase
    {
        contexto.Set<T>().UpdateRange(modelos);
        if (contexto.Database.CurrentTransaction == null)
            await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletarAsync<T>(Guid id, CancellationToken cancellationToken) where T : ModeloBase
    {
        var dbSet = contexto.Set<T>();
        var entidade = await dbSet.FindAsync(new object[] { id }, cancellationToken);

        if (entidade == null)
            throw new KeyNotFoundException($"Entidade do tipo {typeof(T).Name} com id {id} não encontrada.");

        if (entidade is ModeloComExclusao modelComExclusao)
        {
            modelComExclusao.Desativar();
            dbSet.Update(entidade);
        }
        else
        {
            dbSet.Remove(entidade);
        }

        if (contexto.Database.CurrentTransaction == null)
            await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecutarOperacoesEmTransacaoAsync(Func<IRepositorioBase, Task> acao, CancellationToken cancellationToken)
    {
        await contexto.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await acao(this);
            await contexto.SaveChangesAsync(cancellationToken);
            await contexto.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await contexto.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
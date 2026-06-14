using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Comanda : ModeloComExclusao
{
    public Guid UsuarioId { get; private set; }
    public IList<ProdutoComanda> ProdutosComanda { get; private set; }
    public virtual Usuario Usuario { get; private set; }

    public Comanda()
    { }

    public Comanda(Guid id, Guid usuarioId, IList<ProdutoComanda> produtoComandas)
    {
        Id = id;
        UsuarioId = usuarioId;
        ProdutosComanda = produtoComandas;
    }

    public void AtualizarUsuarioId(Guid UsuarioId)
    {
        this.UsuarioId = UsuarioId;
    }
}
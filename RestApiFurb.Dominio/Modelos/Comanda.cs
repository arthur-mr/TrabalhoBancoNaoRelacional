using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Comanda : ModeloComExclusao
{
    public Guid UsuarioId { get; private set; }
    public string Identificacao { get; private set; }
    public IList<ProdutoComanda> ProdutosComanda { get; private set; }
    public virtual Usuario Usuario { get; private set; }

    public Comanda()
    { }

    public Comanda(Guid id, Guid usuarioId, string identificacao, IList<ProdutoComanda> produtoComandas)
    {
        Id = id;
        UsuarioId = usuarioId;
        Identificacao = identificacao;
        ProdutosComanda = produtoComandas;
    }

    public void AtualizarUsuarioId(Guid UsuarioId)
    {
        this.UsuarioId = UsuarioId;
    }
}
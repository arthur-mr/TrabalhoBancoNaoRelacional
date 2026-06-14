using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class ProdutoComanda : ModeloBase
{
    public Guid ComandaId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public virtual Produto Produto { get; private set; }
    public virtual Comanda Comanda { get; private set; }

    public ProdutoComanda()
    { }

    public ProdutoComanda(Guid comandaId, Guid produtoId)
    {
        ComandaId = comandaId;
        ProdutoId = produtoId;
    }
}
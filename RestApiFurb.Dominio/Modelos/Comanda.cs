using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Comanda : ModeloComExclusao
{
    public Guid ClienteId { get; private set; }
    public string Identificacao { get; private set; }
    public IList<ProdutoComanda> ProdutosComanda { get; private set; }
    public virtual Cliente Cliente { get; private set; }

    public Comanda()
    { }

    public Comanda(Guid id, Guid ClienteId, string identificacao, IList<ProdutoComanda> produtoComandas)
    {
        Id = id;
        ClienteId = ClienteId;
        Identificacao = identificacao;
        ProdutosComanda = produtoComandas;
    }

    public void AtualizarClienteId(Guid ClienteId)
    {
        this.ClienteId = ClienteId;
    }
}
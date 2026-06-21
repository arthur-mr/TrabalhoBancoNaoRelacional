using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Comanda : ModeloComExclusao
{
    public Guid? ClienteId { get; private set; }
    public string Identificacao { get; private set; }
    public StatusComanda Status { get; private set; }
    public IList<ProdutoComanda> ProdutosComanda { get; private set; }
    public virtual Cliente Cliente { get; private set; }

    public Comanda()
    { }

    public Comanda(Guid id, Guid? clienteId, string identificacao, IList<ProdutoComanda> produtoComandas)
    {
        Id = id;
        ClienteId = clienteId;
        Identificacao = identificacao;
        ProdutosComanda = produtoComandas;
        Status = StatusComanda.Aberta;
    }

    public void AtualizarClienteId(Guid? ClienteId)
    {
        this.ClienteId = ClienteId;
    }

    public void AtualizarStatus(StatusComanda status)
    {
        Status = status;
    }
}
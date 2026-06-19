using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Produto : ModeloComExclusao
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Codigo { get; private set; }
    public string CodigoBarras { get; private set; }
    public string Categoria { get; private set; }
    public int QuantidadeEstoque { get; private set; }

    public Produto()
    { }

    public Produto(
        string nome,
        decimal preco,
        string codigo,
        string codigoBarras,
        string categoria,
        int quantidadeEstoque)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Preco = preco;
        Codigo = codigo;
        CodigoBarras = codigoBarras;
        Categoria = categoria;
        QuantidadeEstoque = quantidadeEstoque;
    }
}
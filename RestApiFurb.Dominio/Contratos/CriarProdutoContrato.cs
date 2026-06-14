namespace RestApiFurb.Dominio.Contratos;

public sealed record CriarProdutoContrato(
    string Nome,
    decimal Preco,
    string Codigo,
    string CodigoBarras,
    string Categoria,
    int QuantidadeEstoque);
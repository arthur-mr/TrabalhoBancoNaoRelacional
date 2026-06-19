namespace RestApiFurb.Dominio.Contratos;

public sealed record ListarProdutoContrato(
    Guid Id,
    string Nome,
    decimal Preco,
    string Codigo,
    string CodigoBarras,
    string Categoria,
    int QuantidadeEstoque);
namespace RestApiFurb.Api.ViewModels;

public sealed record ListarProdutoViewModel(
    Guid Id,
    string Nome,
    decimal Preco,
    string Codigo,
    string CodigoBarras,
    string Categoria,
    int QuantidadeEstoque);

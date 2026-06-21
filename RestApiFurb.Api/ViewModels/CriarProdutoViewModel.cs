namespace RestApiFurb.Api.ViewModels;

public sealed record CriarProdutoViewModel(
    string Nome,
    decimal Preco,
    string Codigo,
    string CodigoBarras,
    string Categoria,
    int QuantidadeEstoque);
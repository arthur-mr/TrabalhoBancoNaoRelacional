namespace RestApiFurb.Api.ViewModels;

public sealed record ListarProdutoViewModel(
    Guid Id,
    string Nome,
    decimal Preco);

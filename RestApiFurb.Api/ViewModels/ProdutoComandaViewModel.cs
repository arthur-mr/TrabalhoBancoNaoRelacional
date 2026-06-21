namespace RestApiFurb.Api.ViewModels;

public sealed record ProdutoComandaViewModel(
    Guid Id,
    string Nome,
    decimal Preco,
    int Quantidade);
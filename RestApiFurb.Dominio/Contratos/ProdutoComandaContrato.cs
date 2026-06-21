namespace RestApiFurb.Dominio.Contratos;

public sealed record ProdutoComandaContrato(
    Guid Id,
    string Nome,
    decimal Preco,
    int Quantidade);
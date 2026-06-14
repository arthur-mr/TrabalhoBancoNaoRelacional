namespace RestApiFurb.Dominio.Contratos;

public sealed record ListarProdutoContrato(
    Guid Id,
    string Nome,
    decimal Preco);

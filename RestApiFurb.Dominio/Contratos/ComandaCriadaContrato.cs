namespace RestApiFurb.Dominio.Contratos;

public sealed record ComandaCriadaContrato(
    Guid Id,
    Guid ClienteId,
    string Identificacao,
    string NomeCliente,
    string TelefoneCliente,
    IList<ListarProdutoContrato> Produtos);
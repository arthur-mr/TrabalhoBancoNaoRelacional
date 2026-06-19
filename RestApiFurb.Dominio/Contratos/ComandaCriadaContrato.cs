namespace RestApiFurb.Dominio.Contratos;

public sealed record ComandaCriadaContrato(
    Guid Id,
    Guid UsuarioId,
    IList<ListarProdutoContrato> Produtos);
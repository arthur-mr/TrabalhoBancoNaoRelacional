namespace RestApiFurb.Dominio.Contratos;

public sealed record ComandaCriadaContrato(
    Guid Id,
    Guid UsuarioId,
    string NomeUsuario,
    string TelefoneUsuario,
    IList<ListarProdutoContrato> Produtos);
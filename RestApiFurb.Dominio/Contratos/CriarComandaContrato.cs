namespace RestApiFurb.Dominio.Contratos;

public sealed record CriarComandaContrato(Guid UsuarioId, IList<Guid> ProdutosIds);
namespace RestApiFurb.Dominio.Contratos;

public sealed record AtualizarComandaContrato(
    Guid? UsuarioId,
    IList<Guid> ProdutosParaRemover,
    IList<Guid> ProdutosParaAdicionar);
namespace RestApiFurb.Dominio.Contratos;

public sealed record AtualizarComandaContrato(
    Guid? ClienteId,
    IList<CriarComandaItemContrato> ProdutosParaRemover,
    IList<CriarComandaItemContrato> ProdutosParaAdicionar);
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Contratos;

public sealed record AtualizarComandaContrato(
    Guid? ClienteId,
    StatusComanda Status,
    IList<CriarComandaItemContrato> ProdutosParaRemover,
    IList<CriarComandaItemContrato> ProdutosParaAdicionar);
namespace RestApiFurb.Dominio.Contratos;

public sealed record CriarComandaContrato(string Identificacao, IList<CriarComandaItemContrato> Itens);
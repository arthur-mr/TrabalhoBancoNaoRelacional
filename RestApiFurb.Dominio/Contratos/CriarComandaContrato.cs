namespace RestApiFurb.Dominio.Contratos;

public sealed record CriarComandaContrato(string Identificacao, Guid? ClienteId, IList<CriarComandaItemContrato> Itens);
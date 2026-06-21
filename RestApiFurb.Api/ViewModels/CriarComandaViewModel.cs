namespace RestApiFurb.Api.ViewModels;

public sealed record CriarComandaViewModel(string Identificacao, Guid? ClienteId, IList<CriarComandaItemViewModel> Itens);
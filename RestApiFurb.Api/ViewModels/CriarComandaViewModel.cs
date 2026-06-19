namespace RestApiFurb.Api.ViewModels;

public sealed record CriarComandaViewModel(string Identificacao, IList<CriarComandaItemViewModel> Itens);
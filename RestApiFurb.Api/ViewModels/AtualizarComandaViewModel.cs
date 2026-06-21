namespace RestApiFurb.Api.ViewModels;

public sealed record AtualizarComandaViewModel(
    Guid? ClienteId,
    IList<CriarComandaItemViewModel> ProdutosParaRemover,
    IList<CriarComandaItemViewModel> ProdutosParaAdicionar);
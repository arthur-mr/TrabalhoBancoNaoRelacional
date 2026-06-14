namespace RestApiFurb.Api.ViewModels;

public sealed record AtualizarComandaViewModel(
    Guid? UsuarioId,
    IList<Guid> ProdutosParaRemover,
    IList<Guid> ProdutosParaAdicionar);
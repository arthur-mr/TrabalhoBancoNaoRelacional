using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Api.ViewModels;

public sealed record AtualizarComandaViewModel(
    Guid? ClienteId,
    StatusComanda Status,
    IList<CriarComandaItemViewModel> ProdutosParaRemover,
    IList<CriarComandaItemViewModel> ProdutosParaAdicionar);
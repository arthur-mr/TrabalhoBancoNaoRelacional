using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.ViewModels;

public sealed record CriarComandaViewModel(Guid UsuarioId, IList<Guid> ProdutosIds);
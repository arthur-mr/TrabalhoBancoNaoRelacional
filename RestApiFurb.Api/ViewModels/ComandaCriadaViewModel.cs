using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.ViewModels;

public sealed record ComandaCriadaViewModel(
    Guid Id,
    Guid UsuarioId,
    string NomeUsuario,
    string TelefoneUsuario,
    IList<ListarProdutoViewModel> Produtos);
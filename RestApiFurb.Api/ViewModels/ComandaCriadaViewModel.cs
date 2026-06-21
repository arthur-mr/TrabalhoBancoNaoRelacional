using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.ViewModels;

public sealed record ComandaCriadaViewModel(
    Guid Id,
    Guid ClienteId,
    string NomeCliente,
    string TelefoneCliente,
    IList<ListarProdutoViewModel> Produtos);
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Api.ViewModels;

public sealed record ComandaCriadaViewModel(
    Guid Id,
    Guid? ClienteId,
    string Identificacao,
    decimal ValorTotal,
    StatusComanda Status,
    string NomeCliente,
    string TelefoneCliente,
    IList<ProdutoComandaViewModel> Produtos);
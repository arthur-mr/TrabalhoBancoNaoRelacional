namespace RestApiFurb.Api.ViewModels;

public sealed record ListarClienteViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone);
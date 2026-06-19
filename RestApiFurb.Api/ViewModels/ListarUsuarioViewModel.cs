namespace RestApiFurb.Api.ViewModels;

public sealed record ListarUsuarioViewModel(
    Guid Id,
    string Nome,
    string Email,
    string Telefone);
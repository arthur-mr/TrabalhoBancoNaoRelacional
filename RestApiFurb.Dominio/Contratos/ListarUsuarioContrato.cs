namespace RestApiFurb.Dominio.Contratos;

public sealed record ListarUsuarioContrato(
    Guid Id,
    string Nome,
    string Email,
    string Telefone);
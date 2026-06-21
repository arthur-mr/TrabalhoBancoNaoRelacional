namespace RestApiFurb.Dominio.Contratos;

public sealed record ListarClienteContrato(
    Guid Id,
    string Nome,
    string Email,
    string Telefone);
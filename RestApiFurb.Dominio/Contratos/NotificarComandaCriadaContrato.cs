namespace RestApiFurb.Dominio.Contratos;

public sealed record NotificarComandaCriadaContrato(
    string NomeUsuario,
    string EmailUsuario,
    ComandaCriadaContrato Comanda);
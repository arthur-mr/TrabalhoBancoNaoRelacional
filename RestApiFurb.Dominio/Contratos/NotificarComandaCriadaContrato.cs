namespace RestApiFurb.Dominio.Contratos;

public sealed record NotificarComandaCriadaContrato(
    string NomeCliente,
    string EmailCliente,
    ComandaCriadaContrato Comanda);
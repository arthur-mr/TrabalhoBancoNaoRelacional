namespace RestApiFurb.Dominio.Contratos;

public sealed record EmailContrato(
    string Destinatario,
    string Titulo,
    string Corpo);
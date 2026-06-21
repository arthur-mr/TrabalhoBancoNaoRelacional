using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Contratos;

public sealed record ComandaCriadaContrato(
    Guid Id,
    Guid? ClienteId,
    string Identificacao,
    decimal ValorTotal,
    StatusComanda Status,
    string NomeCliente,
    string TelefoneCliente,
    IList<ProdutoComandaContrato> Produtos);
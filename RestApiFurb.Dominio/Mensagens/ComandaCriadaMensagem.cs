using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Utils;

namespace RestApiFurb.Dominio.Mensagens;

[Fila("ComandaCriada")]
public sealed record ComandaCriadaMensagem(string NomeCliente, string EmailCliente, ComandaCriadaContrato Comanda);
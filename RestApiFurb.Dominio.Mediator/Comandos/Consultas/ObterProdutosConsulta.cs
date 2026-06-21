using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterClientesConsulta(FiltroClienteContrato Contrato) : IRequest<IList<ListarClienteContrato>>;
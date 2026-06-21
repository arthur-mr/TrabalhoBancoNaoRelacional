using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterClientesConsulta(int Offset) : IRequest<IList<ListarClienteContrato>>;
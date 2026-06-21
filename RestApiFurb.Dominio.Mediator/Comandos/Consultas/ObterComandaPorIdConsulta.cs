using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterComandaPorIdConsulta(Guid Id) : IRequest<ComandaCriadaContrato>;
using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterUsuariosConsulta(int Offset) : IRequest<IList<ListarUsuarioContrato>>;
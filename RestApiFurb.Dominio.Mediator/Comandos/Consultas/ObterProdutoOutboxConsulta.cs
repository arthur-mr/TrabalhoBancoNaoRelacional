using MediatR;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterProdutoOutboxConsulta() : IRequest<IList<Guid>>;
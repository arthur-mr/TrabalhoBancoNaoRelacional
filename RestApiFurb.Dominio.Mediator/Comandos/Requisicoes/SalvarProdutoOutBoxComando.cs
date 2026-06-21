using MediatR;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record SalvarProdutoOutboxComando(IList<Guid> Ids) : IRequest;
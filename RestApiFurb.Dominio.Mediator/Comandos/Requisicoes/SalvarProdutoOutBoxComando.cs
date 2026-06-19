using MediatR;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record SalvarProdutoOutboxComando(Guid Id) : IRequest;
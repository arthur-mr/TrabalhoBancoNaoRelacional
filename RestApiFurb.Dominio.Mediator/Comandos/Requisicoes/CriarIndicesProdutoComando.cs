using MediatR;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record CriarIndicesProdutoComando() : IRequest;
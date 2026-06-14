using MediatR;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record DeletarComandaComando(Guid Id) : IRequest;
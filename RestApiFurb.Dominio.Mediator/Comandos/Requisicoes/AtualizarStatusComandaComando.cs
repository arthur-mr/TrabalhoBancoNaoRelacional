using MediatR;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record AtualizarStatusComandaComando(Guid ComandaId, StatusComanda Status) : IRequest;
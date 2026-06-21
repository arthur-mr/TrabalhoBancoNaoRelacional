using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record AtualizarComandaComando(Guid Id, AtualizarComandaContrato Contrato) : IRequest;
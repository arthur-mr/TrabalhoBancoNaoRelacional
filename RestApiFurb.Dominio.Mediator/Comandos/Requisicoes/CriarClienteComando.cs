using MediatR;
using RestApiFurb.Dominio.Contratos;
namespace RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

public sealed record CriarClienteComando(CriarClienteContrato Contrato) : IRequest;
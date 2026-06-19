using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterProdutoPorIdConsulta(Guid ProdutoId) : IRequest<ListarProdutoContrato>;
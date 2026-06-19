using MediatR;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Mediator.Comandos.Consultas;

public sealed record ObterAutoCompleteConsulta(string TermoBusca) : IRequest<IEnumerable<ProdutoAutocompleteContrato>>;
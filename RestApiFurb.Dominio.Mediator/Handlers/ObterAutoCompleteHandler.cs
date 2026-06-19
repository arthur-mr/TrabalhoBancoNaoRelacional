using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Servicos.Servicos;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class ObterAutoCompleteHandler : IRequestHandler<ObterAutoCompleteConsulta, IEnumerable<ProdutoAutocompleteContrato>>
{
    private readonly IProdutoServico servico;

    public ObterAutoCompleteHandler(IProdutoServico servico)
    {
        this.servico = servico;
    }

    public Task<IEnumerable<ProdutoAutocompleteContrato>> Handle(ObterAutoCompleteConsulta request, CancellationToken cancellationToken)
    {
        return servico.BuscarAutocompleteAsync(request.TermoBusca, cancellationToken);
    }
}
using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class ObterClientesHandler : IRequestHandler<ObterClientesConsulta, IList<ListarClienteContrato>>
{
    private readonly IClienteServico servico;

    public ObterClientesHandler(IClienteServico servico)
    {
        this.servico = servico;
    }

    public Task<IList<ListarClienteContrato>> Handle(ObterClientesConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterClientesAsync(request.Offset, cancellationToken);
    }
}


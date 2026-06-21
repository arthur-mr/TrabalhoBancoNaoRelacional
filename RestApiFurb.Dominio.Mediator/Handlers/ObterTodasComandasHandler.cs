using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal sealed class ObterTodasComandasHandler : IRequestHandler<ObterTodasComandasConsulta, IList<ComandaCriadaContrato>>
{
    private readonly IComandaServico servico;

    public ObterTodasComandasHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task<IList<ComandaCriadaContrato>> Handle(ObterTodasComandasConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterTodasComandasAsync(cancellationToken);
    }
}

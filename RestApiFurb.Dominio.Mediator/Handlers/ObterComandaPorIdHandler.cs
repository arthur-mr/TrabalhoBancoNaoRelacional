using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal sealed class ObterComandaPorIdHandler : IRequestHandler<ObterComandaPorIdConsulta, ComandaCriadaContrato>
{
    private readonly IComandaServico servico;

    public ObterComandaPorIdHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task<ComandaCriadaContrato> Handle(ObterComandaPorIdConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterComandaPorIdAsync(request.Id, cancellationToken);
    }
}

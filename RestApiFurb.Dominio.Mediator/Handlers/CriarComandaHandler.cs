using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class CriarComandaHandler : IRequestHandler<CriarComandaComando, ComandaCriadaContrato>
{
    private readonly IComandaServico servico;

    public CriarComandaHandler(IComandaServico servico)
    {
        this.servico = servico;
    }

    public Task<ComandaCriadaContrato> Handle(CriarComandaComando request, CancellationToken cancellationToken)
    {
        return servico.CriarComandaAsync(request.Contrato, cancellationToken);
    }
}
using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class NotificarComandaCriadaHandler : IRequestHandler<NotificarComandaCriadaComando>
{
    private readonly INotificadorServico servico;

    public NotificarComandaCriadaHandler(INotificadorServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(NotificarComandaCriadaComando request, CancellationToken cancellationToken)
    {
        return servico.NotificarComandaCriadaAsync(request.Contrato, cancellationToken);
    }
}
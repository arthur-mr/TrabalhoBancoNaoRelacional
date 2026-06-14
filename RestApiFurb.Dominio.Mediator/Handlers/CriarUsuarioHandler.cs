using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class CriarUsuarioHandler : IRequestHandler<CriarUsuarioComando>
{
    private readonly IUsuarioServico servico;

    public CriarUsuarioHandler(IUsuarioServico servico)
    {
        this.servico = servico;
    }

    public Task Handle(CriarUsuarioComando request, CancellationToken cancellationToken)
    {
        return servico.CriarUsuarioAsync(request.Contrato, cancellationToken);
    }
}

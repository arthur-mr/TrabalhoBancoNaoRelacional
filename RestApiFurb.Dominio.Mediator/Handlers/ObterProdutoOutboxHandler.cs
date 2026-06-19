using MediatR;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class ObterProdutoOutboxHandler : IRequestHandler<ObterProdutoOutboxConsulta, IList<Guid>>
{
    private readonly IOutboxServico servico;

    public ObterProdutoOutboxHandler(IOutboxServico servico)
    {
        this.servico = servico;
    }

    public Task<IList<Guid>> Handle(ObterProdutoOutboxConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterMensagensPendentesAsync(cancellationToken);
    }
}
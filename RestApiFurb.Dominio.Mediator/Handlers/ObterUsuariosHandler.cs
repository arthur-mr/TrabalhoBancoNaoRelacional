using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;

namespace RestApiFurb.Dominio.Mediator.Handlers;

internal class ObterUsuariosHandler : IRequestHandler<ObterUsuariosConsulta, IList<ListarUsuarioContrato>>
{
    private readonly IUsuarioServico servico;

    public ObterUsuariosHandler(IUsuarioServico servico)
    {
        this.servico = servico;
    }

    public Task<IList<ListarUsuarioContrato>> Handle(ObterUsuariosConsulta request, CancellationToken cancellationToken)
    {
        return servico.ObterUsuariosAsync(request.Offset, cancellationToken);
    }
}


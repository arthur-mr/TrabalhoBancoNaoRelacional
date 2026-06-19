using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Interfaces;

public interface IUsuarioServico
{
    Task CriarUsuarioAsync(CriarUsuarioContrato contrato, CancellationToken cancellationToken);

    Task<IList<ListarUsuarioContrato>> ObterUsuariosAsync(int offset, CancellationToken cancellationToken);
}
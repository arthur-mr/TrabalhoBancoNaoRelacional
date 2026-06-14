using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class UsuarioServico : IUsuarioServico
{
    private readonly IRepositorioBase<Usuario> repositorio;

    public UsuarioServico(IRepositorioBase<Usuario> repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task CriarUsuarioAsync(CriarUsuarioContrato contrato, CancellationToken cancellationToken)
    {
        var usuario = new Usuario(nome: contrato.Nome, email: contrato.Email, telefone: contrato.Telefone);

        await repositorio.SalvarAsync(usuario, cancellationToken);
    }
}
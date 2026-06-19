using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class UsuarioServico : IUsuarioServico
{
    private readonly IRepositorioBase repositorio;

    public UsuarioServico(IRepositorioBase repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task CriarUsuarioAsync(CriarUsuarioContrato contrato, CancellationToken cancellationToken)
    {
        var usuario = new Usuario(nome: contrato.Nome, email: contrato.Email, telefone: contrato.Telefone);

        await repositorio.AdicionarAsync(usuario, cancellationToken);
    }

    public async Task<IList<ListarUsuarioContrato>> ObterUsuariosAsync(int offset, CancellationToken cancellationToken)
    {
        return await repositorio.MontarConsulta<Usuario>()
            .Skip(offset)
            .Take(10)
            .Select(x => new ListarUsuarioContrato(x.Id, x.Nome, x.Email, x.Telefone))
            .ToListAsync(cancellationToken);
    }
}
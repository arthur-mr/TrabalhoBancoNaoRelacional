using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;
using System.Linq.Expressions;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class ClienteServico : IClienteServico
{
    private readonly IRepositorioBase repositorio;

    public ClienteServico(IRepositorioBase repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task CriarClienteAsync(CriarClienteContrato contrato, CancellationToken cancellationToken)
    {
        var Cliente = new Cliente(nome: contrato.Nome, email: contrato.Email, telefone: contrato.Telefone);

        await repositorio.AdicionarAsync(Cliente, cancellationToken);
    }

    public async Task<IList<ListarClienteContrato>> ObterClientesAsync(FiltroClienteContrato filtroContrato, CancellationToken cancellationToken)
    {
        var filtroExpression = CriarFiltroExpression(filtroContrato);

        return await repositorio.MontarConsulta<Cliente>()
            .Where(filtroExpression)
            .Skip(filtroContrato.Offset)
            .Take(10)
            .Select(x => new ListarClienteContrato(x.Id, x.Nome, x.Email, x.Telefone))
            .ToListAsync(cancellationToken);
    }
    private Expression<Func<Cliente, bool>> CriarFiltroExpression(FiltroClienteContrato filtroContrato)
    {
        return x => string.IsNullOrWhiteSpace(filtroContrato.Nome)
                    || x.Nome.ToLower().Contains(filtroContrato.Nome.ToLower());
    }
}
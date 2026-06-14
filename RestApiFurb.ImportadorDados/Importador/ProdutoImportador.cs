using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.ImportadorDados.Fakers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiFurb.ImportadorDados.Importador;

internal sealed class ProdutoImportador
{
    private readonly IRepositorioBase<Produto> repositorio;

    public ProdutoImportador(IRepositorioBase<Produto> repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task AdicionarProdutosEmMassaAsync(CancellationToken cancellationToken)
    {
        int tamanhopagina = 10000;
        int quantidadeAdicionada = 0;

        while (quantidadeAdicionada < 1000000)
        {
            var produtos = ProdutoFaker.GerarMassaDeDados(tamanhopagina);
            await repositorio.SalvarAsync(produtos, cancellationToken);
            quantidadeAdicionada += tamanhopagina;
        }
    }
}

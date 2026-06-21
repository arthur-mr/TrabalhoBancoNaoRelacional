using Bogus.Extensions.UnitedKingdom;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.ImportadorDados.Fakers;
using System.Text.Json;

namespace RestApiFurb.ImportadorDados.Importador;

internal sealed class ProdutoImportador
{
    private readonly IRepositorioBase repositorio;

    public ProdutoImportador(IRepositorioBase repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task AdicionarProdutosEmMassaAsync(CancellationToken cancellationToken)
    {
        int tamanhopagina = 10000;
        int quantidadeAdicionada = 0;

        while (quantidadeAdicionada < 10000000)
        {
            var produtos = ProdutoFaker.GerarMassaDeDados(tamanhopagina);
            await repositorio.AdicionarAsync(produtos, cancellationToken);
            
            var outbox = new List<OutboxMessage>();
            foreach (var produto in produtos)
            {
                var payloadObj = new
                {
                    produto.Id,
                    produto.Nome
                };

                var payloadJson = JsonSerializer.Serialize(payloadObj);

                var outboxMessage = new OutboxMessage(
                    tipoEvento: "ProdutoCriado",
                    payload: payloadJson);

                outbox.Add(outboxMessage);

            }

            await repositorio.AdicionarAsync(outbox, cancellationToken);
            Console.WriteLine($"Produtos adicionados com sucesso: Quantidade {produtos.Count}");

            quantidadeAdicionada += tamanhopagina;
        }
    }
}

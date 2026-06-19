using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Analysis;
using Elastic.Esql.Extensions;
using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;
using System.Text.Json;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class ProdutoServico : IProdutoServico
{
    private readonly IRepositorioBase repositorioBase;
    private readonly ElasticsearchClient elasticClient;

    public ProdutoServico(
        IRepositorioBase repositorioBase,
        ElasticsearchClient elasticClient)
    {
        this.repositorioBase = repositorioBase;
        this.elasticClient = elasticClient;
    }

    public async Task CriarProdutoAsync(CriarProdutoContrato contrato, CancellationToken cancellationToken)
    {
        var entidade = new Produto(
            nome: contrato.Nome,
            preco: contrato.Preco,
            codigo: contrato.Codigo,
            codigoBarras: contrato.CodigoBarras,
            categoria: contrato.Categoria,
            quantidadeEstoque: contrato.QuantidadeEstoque);

        var payloadObj = new
        {
            entidade.Id,
            entidade.Nome
        };

        var payloadJson = JsonSerializer.Serialize(payloadObj);

        var outboxMessage = new OutboxMessage(
            tipoEvento: "ProdutoCriado",
            payload: payloadJson);

        await repositorioBase.ExecutarOperacoesEmTransacaoAsync(async (repo) =>
        {
            await repo.AdicionarAsync(entidade, cancellationToken);
            await repo.AdicionarAsync(outboxMessage, cancellationToken);

        }, cancellationToken);
    }

    public async Task<ListarProdutoContrato> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await repositorioBase.MontarConsulta<Produto>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (produto == null)
            return null;

        var contrato = new ListarProdutoContrato(
            Id: produto.Id,
            Nome: produto.Nome,
            Preco: produto.Preco,
            Codigo: produto.Codigo,
            CodigoBarras: produto.CodigoBarras,
            Categoria: produto.Categoria,
            QuantidadeEstoque: produto.QuantidadeEstoque);

        return contrato;
    }

    public async Task<IEnumerable<ProdutoAutocompleteContrato>> BuscarAutocompleteAsync(string termoBusca, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(termoBusca))
            return Enumerable.Empty<ProdutoAutocompleteContrato>();

        var response = await elasticClient.SearchAsync<ProdutoAutocompleteContrato>(s => s
                    .Indices("produtos")
                    .Size(10)
                    .Source(so => so.Filter(f => f.Includes(new[] { "id", "nome" })))
                    .Query(q => q
                        .MatchPhrasePrefix(m => m
                            .Field(p => p.Nome)
                            .Query(termoBusca)
                        )
                    ), cancellationToken);

        if (!response.IsValidResponse)
            throw new Exception($"Falha ao consultar o Elasticsearch. Motivo: {response.ElasticsearchServerError?.Error?.Reason}");

        return response.Documents;
    }

    public async Task CriarIndiceProdutosAsync(CancellationToken cancellationToken)
    {
        var indexName = "produtos";

        var existsResponse = await elasticClient.Indices.ExistsAsync(indexName);

        if (existsResponse.Exists)
            return;

        var createResponse = await elasticClient.Indices.CreateAsync(indexName, c => c
            .Settings(s => s
                .Analysis(a => a
                    .Tokenizers(t => t
                        .EdgeNGram("autocomplete_tokenizer", e => e
                            .MinGram(2)
                            .MaxGram(10)
                            .TokenChars(new[] { TokenChar.Letter, TokenChar.Digit })
                        )
                    )
                    .Analyzers(an => an
                        .Custom("autocomplete_analyzer", ca => ca
                            .Tokenizer("autocomplete_tokenizer")
                            .Filter(new[] { "lowercase" })
                        )
                        .Custom("autocomplete_search_analyzer", ca => ca
                            .Tokenizer("standard")
                            .Filter(new[] { "lowercase" })
                        )
                    )
                )
            )
            .Mappings(m => m
                .Properties<Produto>(p => p
                    .Text(t => t.Nome, text => text
                        .Analyzer("autocomplete_analyzer")
                        .SearchAnalyzer("autocomplete_search_analyzer")
                    )
                )
            )
        );

        if (!createResponse.IsValidResponse)
            throw new Exception("Erro ao criar o índice no Elasticsearch.");
    }
}
using Elastic.Clients.Elasticsearch;
using global::RestApiFurb.Dominio.Contratos;
using global::RestApiFurb.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Interfaces;
using System.Text.Json;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class OutboxServico : IOutboxServico
{
    private readonly IRepositorioBase repositorio;
    private readonly ElasticsearchClient elasticClient;

    public OutboxServico(IRepositorioBase repositorio, ElasticsearchClient elasticClient)
    {
        this.repositorio = repositorio;
        this.elasticClient = elasticClient;
    }

    public async Task<IList<Guid>> ObterMensagensPendentesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await repositorio.MontarConsulta<OutboxMessage>()
                .Where(x => !x.Processado)
                .OrderBy(x => x.DataCriacao)
                .Take(100000)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return new List<Guid>();
        }
    }

    public async Task ProcessarMensagensEmMassaAsync(IList<Guid> ids, CancellationToken cancellationToken)
    {
        if (ids == null || !ids.Any()) return;

        List<OutboxMessage> mensagens = new List<OutboxMessage>();

        foreach (var id in ids)
        {
            var msg = await repositorio.ObterPorIdAsync<OutboxMessage>(id);
            if (msg != null && !msg.Processado)
                mensagens.Add(msg);
        }

        if (!mensagens.Any()) return;

        var produtosParaIndexar = new List<ProdutoAutocompleteContrato>();
        var dicionarioMensagens = new Dictionary<string, OutboxMessage>();

        foreach (var mensagem in mensagens)
        {
            if (mensagem.TipoEvento == "ProdutoCriado")
            {
                try
                {
                    var produtoElastic = JsonSerializer.Deserialize<ProdutoAutocompleteContrato>(mensagem.Payload);
                    if (produtoElastic != null)
                    {
                        produtosParaIndexar.Add(produtoElastic);
                        dicionarioMensagens[produtoElastic.Id.ToString()] = mensagem;
                    }
                }
                catch (Exception ex)
                {
                    mensagem.RegistrarErro($"Erro deserialização: {ex.Message}");
                }
            }
        }

        if (produtosParaIndexar.Any())
        {
            const int tamanhoChunk = 10000;

            for (int offset = 0; offset < produtosParaIndexar.Count; offset += tamanhoChunk)
            {
                var chunk = produtosParaIndexar.Skip(offset).Take(tamanhoChunk).ToList();

                try
                {
                    var bulkResponse = await elasticClient.BulkAsync(b => b
                        .Index("produtos")
                        .IndexMany(chunk, (descriptor, produto) => descriptor.Id(produto.Id.ToString()))
                    , cancellationToken);

                    if (!bulkResponse.IsValidResponse)
                    {
                        throw new Exception($"Falha Elastic: {bulkResponse.DebugInformation}");
                    }

                    foreach (var item in bulkResponse.Items)
                    {
                        if (dicionarioMensagens.TryGetValue(item.Id, out var outboxMsg))
                        {
                            if (item.IsValid)
                                outboxMsg.MarcarComoProcessado();
                            else
                                outboxMsg.RegistrarErro($"Erro item Bulk: {item.Error?.Reason}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    foreach (var produto in chunk)
                    {
                        if (dicionarioMensagens.TryGetValue(produto.Id.ToString(), out var outboxMsg))
                        {
                            outboxMsg.RegistrarErro($"Falha no Envio Elastic: {ex.Message}");
                        }
                    }
                }
            }
        }

        await repositorio.ExecutarOperacoesEmTransacaoAsync(async (repo) =>
        {
            foreach (var mensagem in mensagens)
            {
                await repo.AtualizarAsync(mensagem, cancellationToken);
            }
        }, cancellationToken);

        Console.WriteLine($"[Worker] Lote de {mensagens.Count} mensagens processado com sucesso!");
    }
}
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
        return await repositorio.MontarConsulta<OutboxMessage>()
            .Where(x => !x.Processado)
            .OrderBy(x => x.DataCriacao)
            .Take(50)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task ProcessarMensagemAsync(Guid id, CancellationToken cancellationToken)
    {
        await repositorio.ExecutarOperacoesEmTransacaoAsync(async (repo) =>
        {
            var mensagem = await repo.ObterPorIdAsync<OutboxMessage>(id);
            if (mensagem == null || mensagem.Processado)
                return;

            try
            {
                if (mensagem.TipoEvento == "ProdutoCriado")
                {
                    var produtoElastic = JsonSerializer.Deserialize<ProdutoAutocompleteContrato>(mensagem.Payload);

                    if (produtoElastic != null)
                    {
                        var response = await elasticClient.IndexAsync(produtoElastic, i => i
                            .Index("produtos")
                            .Id(produtoElastic.Id.ToString())
                        , cancellationToken);

                        if (!response.IsValidResponse)
                            throw new Exception($"Falha Elastic: {response.ElasticsearchServerError?.Error?.Reason}");
                    }
                }

                mensagem.MarcarComoProcessado();
                await repo.AtualizarAsync(mensagem, cancellationToken);
            }
            catch (Exception ex)
            {
                mensagem.RegistrarErro(ex.Message);
                await repo.AtualizarAsync(mensagem, cancellationToken);
            }

        }, cancellationToken);
    }
}
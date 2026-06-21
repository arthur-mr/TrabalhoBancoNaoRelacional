using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class ComandaServico : IComandaServico
{
    private readonly IRepositorioBase repositorio;
    private readonly IPublicadorMensagemServico mensageria;

    public ComandaServico(
        IRepositorioBase repositorio,
        IPublicadorMensagemServico mensageria)
    {
        this.repositorio = repositorio;
        this.mensageria = mensageria;
    }

    public async Task<ComandaCriadaContrato> CriarComandaAsync(CriarComandaContrato contrato, CancellationToken cancellationToken)
    {
        var comandaId = Guid.NewGuid();
        var produtosComanda = new List<ProdutoComanda>();
        var produtosRetorno = new List<ProdutoComandaContrato>();

        Cliente cliente = null;

        if (contrato.ClienteId.HasValue)
        {
            cliente = await repositorio.ObterPorIdAsync<Cliente>(contrato.ClienteId.Value);

            if (cliente is null)
                throw new Exception($"Cliente com id {contrato.ClienteId.Value} não encontrado");
        }

        foreach (var item in contrato.Itens)
        {
            var produto = await repositorio.ObterPorIdAsync<Produto>(item.ProdutoId)
                ?? throw new ArgumentException($"Produto com ID {item.ProdutoId} não encontrado.");

            var produtoComanda = new ProdutoComanda(
                comandaId: comandaId,
                produtoId: produto.Id,
                quantidade: item.Quantidade);

            var produtoRetorno = new ProdutoComandaContrato(
                Id: produto.Id,
                Nome: produto.Nome,
                Preco: produto.Preco,
                Quantidade: item.Quantidade);

            produtosComanda.Add(produtoComanda);
            produtosRetorno.Add(produtoRetorno);
        }

        var entidade = new Comanda(
            id: comandaId,
            clienteId: contrato.ClienteId,
            identificacao: contrato.Identificacao,
            produtoComandas: produtosComanda);

        await repositorio.AdicionarAsync(entidade, cancellationToken);

        var valorTotal = entidade.ProdutosComanda.Sum(x => x.Produto.Preco * x.Quantidade);

        var retorno = new ComandaCriadaContrato(
            Id: entidade.Id,
            ClienteId: entidade.ClienteId,
            Identificacao: entidade.Identificacao,
            ValorTotal: valorTotal,
            Status: entidade.Status,
            NomeCliente: cliente?.Nome,
            TelefoneCliente: cliente?.Telefone,
            Produtos: produtosRetorno);

        return retorno;
    }

    public async Task<IList<ComandaCriadaContrato>> ObterTodasComandasAsync(CancellationToken cancellationToken)
    {
        var comandas = await repositorio
            .MontarConsulta<Comanda>()
            .Where(x => x.Ativo)
            .Include(c => c.ProdutosComanda)
            .ThenInclude(pc => pc.Produto)
            .Include(c => c.Cliente)
            .ToListAsync(cancellationToken);

        var contratos = comandas.Select(c =>
        {
            var valorTotal = c.ProdutosComanda.Sum(x => x.Produto.Preco * x.Quantidade);

            var produtos = c.ProdutosComanda
                .Select(pc => new ProdutoComandaContrato(
                    Id: pc.Produto.Id,
                    Nome: pc.Produto.Nome,
                    Preco: pc.Produto.Preco,
                    Quantidade: pc.Quantidade))
                .ToList();

            return new ComandaCriadaContrato(
                Id: c.Id,
                ClienteId: c.Cliente?.Id,
                Identificacao: c.Identificacao,
                ValorTotal: valorTotal,
                Status: c.Status,
                NomeCliente: c.Cliente?.Nome,
                TelefoneCliente: c.Cliente?.Telefone,
                Produtos: produtos);
        })
        .ToList();

        return contratos;
    }

    public async Task<ComandaCriadaContrato> ObterComandaPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var comanda = await repositorio
            .MontarConsulta<Comanda>()
            .Where(x => x.Ativo)
            .Include(c => c.ProdutosComanda)
            .ThenInclude(pc => pc.Produto)
            .Include(c => c.Cliente)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (comanda is null)
            return null;

        var produtos = comanda.ProdutosComanda
            .Select(pc => new ProdutoComandaContrato(
                Id: pc.Produto.Id,
                Nome: pc.Produto.Nome,
                Preco: pc.Produto.Preco,
                Quantidade: pc.Quantidade))
            .ToList();

        var valorTotal = comanda.ProdutosComanda.Sum(x => x.Produto.Preco * x.Quantidade);

        return new ComandaCriadaContrato(
            Id: comanda.Id,
            ClienteId: comanda.Cliente?.Id,
            Identificacao: comanda.Identificacao,
            ValorTotal: valorTotal,
            Status: comanda.Status,
            NomeCliente: comanda.Cliente?.Nome,
            TelefoneCliente: comanda.Cliente?.Telefone,
            Produtos: produtos);
    }

    public async Task AtualizarComandaAsync(Guid id, AtualizarComandaContrato contrato, CancellationToken cancellationToken)
    {
        var comanda = await repositorio
            .MontarConsulta<Comanda>()
            .Include(x => x.ProdutosComanda)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (comanda is null)
            throw new KeyNotFoundException($"Comanda {id} não encontrada.");

        if (contrato.ClienteId.HasValue)
        {
            var existe = await repositorio
                .MontarConsulta<Cliente>()
                .AnyAsync(u => u.Id == contrato.ClienteId.Value, cancellationToken);

            if (!existe)
                throw new KeyNotFoundException($"Usuário {contrato.ClienteId.Value} não encontrado.");
        }

        comanda.AtualizarClienteId(contrato.ClienteId);
        comanda.AtualizarStatus(contrato.Status);

        foreach (var produtoRemover in contrato.ProdutosParaRemover)
        {
            var produtoParaRemover = comanda.ProdutosComanda.FirstOrDefault(pc => pc.ProdutoId == produtoRemover.ProdutoId);

            if (produtoParaRemover is null)
                throw new KeyNotFoundException($"Produto {produtoRemover.ProdutoId} não está na comanda ou foi removido mais que a quantidade presente.");

            comanda.ProdutosComanda.Remove(produtoParaRemover);

            var produtoComanda = await repositorio.MontarConsulta<ProdutoComanda>().FirstOrDefaultAsync(pc => pc.Id == produtoParaRemover.Id, cancellationToken);

            await repositorio.DeletarAsync<ProdutoComanda>(produtoComanda.Id, cancellationToken);
        }

        foreach (var produtoAdicionar in contrato.ProdutosParaAdicionar)
        {
            var existe = await repositorio
                .MontarConsulta<Produto>()
                .AnyAsync(p => p.Id == produtoAdicionar.ProdutoId, cancellationToken);

            if (!existe)
                throw new KeyNotFoundException($"Produto {produtoAdicionar.ProdutoId} não existe.");

            comanda.ProdutosComanda.Add(new ProdutoComanda(comandaId: comanda.Id, produtoId: produtoAdicionar.ProdutoId, quantidade: produtoAdicionar.Quantidade));
        }

        await repositorio.AtualizarAsync(comanda, cancellationToken);
    }

    public async Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken)
    {
        await repositorio.DeletarAsync<Comanda>(id, cancellationToken);
    }

    public async Task AtualizarStatusComandaAsync(Guid comandaId, StatusComanda status, CancellationToken cancellationToken)
    {
        var entidade = await repositorio.ObterPorIdAsync<Comanda>(comandaId);

        if (entidade is null)
            throw new Exception($"Não foi possível localizar a comanda com id {comandaId}");

        entidade.AtualizarStatus(status);
        await repositorio.AtualizarAsync(entidade, cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mensagens;
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
        var produtosRetorno = new List<ListarProdutoContrato>();

        foreach (var item in contrato.Itens)
        {
            var produto = await repositorio.ObterPorIdAsync<Produto>(item.ProdutoId)
                ?? throw new ArgumentException($"Produto com ID {item.ProdutoId} não encontrado.");

            var produtoComanda = new ProdutoComanda(
                comandaId: comandaId,
                produtoId: produto.Id,
                quantidade: item.Quantidade);

            var produtoRetorno = new ListarProdutoContrato(
                Id: produto.Id,
                Nome: produto.Nome,
                Preco: produto.Preco,
                Codigo: produto.Codigo,
                CodigoBarras: produto.CodigoBarras,
                Categoria: produto.Categoria,
                QuantidadeEstoque: produto.QuantidadeEstoque);

            produtosComanda.Add(produtoComanda);
            produtosRetorno.Add(produtoRetorno);
        }

        var entidade = new Comanda(
            id: comandaId,
            ClienteId: Guid.NewGuid(),
            identificacao: contrato.Identificacao,
            produtoComandas: produtosComanda);

        await repositorio.AdicionarAsync(entidade, cancellationToken);

        var retorno = new ComandaCriadaContrato(
            Id: entidade.Id,
            ClienteId: entidade.ClienteId,
            Identificacao: entidade.Identificacao,
            NomeCliente: entidade.Cliente.Nome,
            TelefoneCliente: entidade.Cliente.Telefone,
            Produtos: produtosRetorno);

        var mensagem = new ComandaCriadaMensagem(NomeCliente: entidade.Cliente.Nome, EmailCliente: entidade.Cliente.Email, retorno);

        mensageria.Publicar(mensagem);

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
            var produtos = c.ProdutosComanda
                .Select(pc => new ListarProdutoContrato(
                    Id: pc.Produto.Id,
                    Nome: pc.Produto.Nome,
                    Preco: pc.Produto.Preco,
                    Codigo: pc.Produto.Codigo,
                    CodigoBarras: pc.Produto.CodigoBarras,
                    Categoria: pc.Produto.Categoria,
                    QuantidadeEstoque: pc.Produto.QuantidadeEstoque))
                .ToList();

            return new ComandaCriadaContrato(
                Id: c.Id,
                ClienteId: c.Cliente.Id,
                Identificacao: c.Identificacao,
                NomeCliente: c.Cliente.Nome,
                TelefoneCliente: c.Cliente.Telefone,
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
            .Select(pc => new ListarProdutoContrato(
                Id: pc.Produto.Id,
                Nome: pc.Produto.Nome,
                Preco: pc.Produto.Preco,
                Codigo: pc.Produto.Codigo,
                CodigoBarras: pc.Produto.CodigoBarras,
                Categoria: pc.Produto.Categoria,
                QuantidadeEstoque: pc.Produto.QuantidadeEstoque))
            .ToList();

        return new ComandaCriadaContrato(
            Id: comanda.Id,
            ClienteId: comanda.Cliente.Id,
            Identificacao: comanda.Identificacao,
            NomeCliente: comanda.Cliente.Nome,
            TelefoneCliente: comanda.Cliente.Telefone,
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

            comanda.AtualizarClienteId(contrato.ClienteId.Value);
        }

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
}
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
            usuarioId: Guid.NewGuid(),
            identificacao: contrato.Identificacao,
            produtoComandas: produtosComanda);

        await repositorio.AdicionarAsync(entidade, cancellationToken);

        var retorno = new ComandaCriadaContrato(
            Id: entidade.Id,
            UsuarioId: Guid.NewGuid(),
            Identificacao: entidade.Identificacao,
            Produtos: produtosRetorno);

        var mensagem = new ComandaCriadaMensagem(
            Identificacao: entidade.Identificacao,
            Comanda: retorno);

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
            .Include(c => c.Usuario)
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
                UsuarioId: c.Usuario.Id,
                NomeUsuario: c.Usuario.Nome,
                TelefoneUsuario: c.Usuario.Telefone,
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
            .Include(c => c.Usuario)
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
            UsuarioId: comanda.Usuario.Id,
            NomeUsuario: comanda.Usuario.Nome,
            TelefoneUsuario: comanda.Usuario.Telefone,
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

        if (contrato.UsuarioId.HasValue)
        {
            var existe = await repositorio
                .MontarConsulta<Usuario>()
                .AnyAsync(u => u.Id == contrato.UsuarioId.Value, cancellationToken);

            if (!existe)
                throw new KeyNotFoundException($"Usuário {contrato.UsuarioId.Value} não encontrado.");

            comanda.AtualizarUsuarioId(contrato.UsuarioId.Value);
        }

        foreach (var produtoIdRemover in contrato.ProdutosParaRemover)
        {
            var produtoParaRemover = comanda.ProdutosComanda.FirstOrDefault(pc => pc.ProdutoId == produtoIdRemover);

            if (produtoParaRemover is null)
                throw new KeyNotFoundException($"Produto {produtoIdRemover} não está na comanda ou foi removido mais que a quantidade presente.");

            comanda.ProdutosComanda.Remove(produtoParaRemover);

            var produtoComanda = await repositorio.MontarConsulta<ProdutoComanda>().FirstOrDefaultAsync(pc => pc.Id == produtoParaRemover.Id, cancellationToken);

            await repositorio.DeletarAsync<ProdutoComanda>(produtoComanda.Id, cancellationToken);
        }

        foreach (var produtoIdAdicionar in contrato.ProdutosParaAdicionar)
        {
            var existe = await repositorio
                .MontarConsulta<Produto>()
                .AnyAsync(p => p.Id == produtoIdAdicionar, cancellationToken);

            if (!existe)
                throw new KeyNotFoundException($"Produto {produtoIdAdicionar} não existe.");

            comanda.ProdutosComanda.Add(new ProdutoComanda(comandaId: comanda.Id, produtoId: produtoIdAdicionar));
        }

        await repositorio.AtualizarAsync(comanda, cancellationToken);
    }

    public async Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken)
    {
        await repositorio.DeletarAsync<Comanda>(id, cancellationToken);
    }
}
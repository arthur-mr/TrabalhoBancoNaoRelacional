using Microsoft.EntityFrameworkCore;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Mensagens;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class ComandaServico : IComandaServico
{
    private readonly IRepositorioBase<Comanda> repositorio;
    private readonly IRepositorioBase<Produto> repositorioProduto;
    private readonly IRepositorioBase<Usuario> repositorioUsuario;
    private readonly IRepositorioBase<ProdutoComanda> repositorioProdutoComanda;
    private readonly IPublicadorMensagemServico mensageria;

    public ComandaServico(
        IRepositorioBase<Comanda> repositorio,
        IRepositorioBase<Produto> repositorioProduto,
        IRepositorioBase<Usuario> repositorioUsuario,
        IRepositorioBase<ProdutoComanda> repositorioProdutoComanda,
        IPublicadorMensagemServico mensageria)
    {
        this.repositorio = repositorio;
        this.repositorioProduto = repositorioProduto;
        this.repositorioUsuario = repositorioUsuario;
        this.repositorioProdutoComanda = repositorioProdutoComanda;
        this.mensageria = mensageria;
    }

    public async Task<ComandaCriadaContrato> CriarComandaAsync(CriarComandaContrato contrato, CancellationToken cancellationToken)
    {
        var comandaId = Guid.NewGuid();
        var usuario = await repositorioUsuario.ObterPorIdAsync(contrato.UsuarioId);
        var produtosComanda = new List<ProdutoComanda>();
        var produtosRetorno = new List<ListarProdutoContrato>();

        if (usuario is null)
        {
            throw new ArgumentException($"Usuário com ID {contrato.UsuarioId} não encontrado.");
        }

        foreach (var produtoId in contrato.ProdutosIds)
        {
            var produto = await repositorioProduto.ObterPorIdAsync(produtoId)
                ?? throw new ArgumentException($"Produto com ID {produtoId} não encontrado.");

            var produtoComanda = new ProdutoComanda(comandaId: comandaId, produtoId: produto.Id);
            var produtoRetorno = new ListarProdutoContrato(produto.Id, produto.Nome, produto.Preco);

            produtosComanda.Add(produtoComanda);
            produtosRetorno.Add(produtoRetorno);
        }

        var entidade = new Comanda(
            id: comandaId,
            usuarioId: contrato.UsuarioId,
            produtoComandas: produtosComanda);

        await repositorio.SalvarAsync(entidade, cancellationToken);

        var retorno = new ComandaCriadaContrato(
            Id: entidade.Id,
            UsuarioId: entidade.UsuarioId,
            NomeUsuario: usuario.Nome,
            TelefoneUsuario: usuario.Telefone,
            Produtos: produtosRetorno);

        var mensagem = new ComandaCriadaMensagem(
            NomeUsuario: usuario.Nome,
            EmailUsuario: usuario.Email,
            Comanda: retorno);

        mensageria.Publicar(mensagem);

        return retorno;
    }

    public async Task<IList<ComandaCriadaContrato>> ObterTodasComandasAsync(CancellationToken cancellationToken)
    {
        var comandas = await repositorio
            .MontarConsulta()
            .Where(x => x.Ativo)
            .Include(c => c.ProdutosComanda)
            .ThenInclude(pc => pc.Produto)
            .Include(c => c.Usuario)
            .ToListAsync(cancellationToken);

        var contratos = comandas.Select(c =>
        {
            var produtos = c.ProdutosComanda
                .Select(pc => new ListarProdutoContrato(pc.Produto.Id, pc.Produto.Nome, pc.Produto.Preco))
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
            .MontarConsulta()
            .Where(x => x.Ativo)
            .Include(c => c.ProdutosComanda)
            .ThenInclude(pc => pc.Produto)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (comanda is null)
            return null;

        var produtos = comanda.ProdutosComanda
            .Select(pc => new ListarProdutoContrato(pc.Produto.Id, pc.Produto.Nome, pc.Produto.Preco))
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
            .MontarConsulta()
            .Include(x => x.ProdutosComanda)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (comanda is null)
            throw new KeyNotFoundException($"Comanda {id} não encontrada.");

        if (contrato.UsuarioId.HasValue)
        {
            var existe = await repositorioUsuario
                .MontarConsulta()
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

            var produtoComanda = await repositorioProdutoComanda.MontarConsulta().FirstOrDefaultAsync(pc => pc.Id == produtoParaRemover.Id, cancellationToken);

            await repositorioProdutoComanda.DeletarAsync(produtoComanda.Id, cancellationToken);
        }

        foreach (var produtoIdAdicionar in contrato.ProdutosParaAdicionar)
        {
            var existe = await repositorioProduto
                .MontarConsulta()
                .AnyAsync(p => p.Id == produtoIdAdicionar, cancellationToken);

            if (!existe)
                throw new KeyNotFoundException($"Produto {produtoIdAdicionar} não existe.");

            comanda.ProdutosComanda.Add(new ProdutoComanda(comandaId: comanda.Id, produtoId: produtoIdAdicionar));
        }

        await repositorio.AtualizarAsync(comanda, cancellationToken);
    }

    public async Task DeletarComandaAsync(Guid id, CancellationToken cancellationToken)
    {
        await repositorio.DeletarAsync(id, cancellationToken);
    }
}
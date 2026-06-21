using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.Conversores;

internal sealed class ComandaConversor : IComandaConversor
{
    public AtualizarComandaContrato ConvertarParaContrato(AtualizarComandaViewModel viewModel)
    {
        if (viewModel is null)
            return null;

        var itensAdicionar = viewModel.ProdutosParaAdicionar.Select(x => new CriarComandaItemContrato(x.ProdutoId, x.Quantidade)).ToList();
        var itensRemover = viewModel.ProdutosParaRemover.Select(x => new CriarComandaItemContrato(x.ProdutoId, x.Quantidade)).ToList();

        return new AtualizarComandaContrato(
            ClienteId: viewModel.ClienteId,
            Status: viewModel.Status,
            ProdutosParaRemover: itensRemover,
            ProdutosParaAdicionar: itensAdicionar);
    }

    public CriarComandaContrato ConverterParaContrato(CriarComandaViewModel viewModel)
    {
        if (viewModel is null)
            return null;

        var itens = viewModel.Itens?.Select(x => new CriarComandaItemContrato(x.ProdutoId, x.Quantidade)).ToList();

        return new CriarComandaContrato(Identificacao: viewModel.Identificacao, ClienteId: viewModel.ClienteId, Itens: itens);
    }

    public ComandaCriadaViewModel ConverterParaViewModel(ComandaCriadaContrato contrato)
    {
        return new ComandaCriadaViewModel(
         Id: contrato.Id,
         ClienteId: contrato.ClienteId,
         Identificacao: contrato.Identificacao,
         ValorTotal: contrato.ValorTotal,
         Status: contrato.Status,
         NomeCliente: contrato.NomeCliente,
         TelefoneCliente: contrato.TelefoneCliente,
         Produtos: contrato.Produtos.Select(ConverterParaViewModel).ToList());
    }

    private ProdutoComandaViewModel ConverterParaViewModel(ProdutoComandaContrato contrato)
    {
        return new ProdutoComandaViewModel(
            Id: contrato.Id,
            Nome: contrato.Nome,
            Preco: contrato.Preco,
            Quantidade: contrato.Quantidade);
    }
}

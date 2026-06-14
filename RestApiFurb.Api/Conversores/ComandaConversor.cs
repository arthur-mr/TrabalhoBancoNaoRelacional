using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Api.Conversores;

internal sealed class ComandaConversor : IComandaConversor
{
    public AtualizarComandaContrato ConvertarParaContrato(AtualizarComandaViewModel viewModel)
    {
        return new AtualizarComandaContrato(
            UsuarioId: viewModel.UsuarioId,
            ProdutosParaRemover: viewModel.ProdutosParaRemover,
            ProdutosParaAdicionar: viewModel.ProdutosParaAdicionar);
    }

    public CriarComandaContrato ConverterParaContrato(CriarComandaViewModel viewModel)
    {
        return new CriarComandaContrato(UsuarioId: viewModel.UsuarioId, ProdutosIds: viewModel.ProdutosIds);
    }

    public ComandaCriadaViewModel ConverterParaViewModel(ComandaCriadaContrato contrato)
    {
        return new ComandaCriadaViewModel(
         Id: contrato.Id,
         UsuarioId: contrato.UsuarioId,
         NomeUsuario: contrato.NomeUsuario,
         TelefoneUsuario: contrato.TelefoneUsuario,
         Produtos: contrato.Produtos.Select(ConverterParaViewModel).ToList());
    }

    private ListarProdutoViewModel ConverterParaViewModel(ListarProdutoContrato contrato)
    {
        return new ListarProdutoViewModel(
            Id: contrato.Id,
            Nome: contrato.Nome,
            Preco: contrato.Preco);
    }
}

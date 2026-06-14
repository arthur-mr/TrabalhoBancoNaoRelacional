using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProdutoController : ControllerBase
{
    private readonly IMediator mediator;

    public ProdutoController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoViewModel viewModel)
    {
        var (ehValido, mensagemRetorno) = ValidarViewModel(viewModel);

        if (!ehValido)
            return BadRequest(mensagemRetorno);

        var contrato = new CriarProdutoContrato(
            Nome: viewModel.Nome,
            Preco: viewModel.Preco,
            Codigo: viewModel.Codigo,
            CodigoBarras: viewModel.CodigoBarras,
            Categoria: viewModel.Categoria,
            QuantidadeEstoque: viewModel.QuantidadeEstoque);

        var comando = new CriarProdutoComando(contrato);
        await mediator.Send(comando);

        return Ok("Produto criado com sucesso");
    }

    private (bool, string) ValidarViewModel(CriarProdutoViewModel viewModel)
    {
        if (viewModel is null)
            return (false, "Produto não pode ser nulo.");

        if (string.IsNullOrWhiteSpace(viewModel.Nome))
            return (false, "Nome do produto é obrigatório.");

        if (viewModel.Preco <= 0)
            return (false, "Preço do produto deve ser maior que zero.");

        return (true, string.Empty);
    }
}

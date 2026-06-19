using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]/[Action]")]
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            return BadRequest("ID inválido.");

        var consulta = new ObterProdutoPorIdConsulta(id);
        var contrato = await mediator.Send(consulta, cancellationToken);

        if (contrato == null)
            return NotFound("Produto não encontrado.");

        var viewModel = new ListarProdutoViewModel(
            Id: contrato.Id,
            Nome: contrato.Nome,
            Preco: contrato.Preco,
            Codigo: contrato.Codigo,
            CodigoBarras: contrato.CodigoBarras,
            Categoria: contrato.Categoria,
            QuantidadeEstoque: contrato.QuantidadeEstoque);

        return Ok(contrato);
    }

    [HttpPost]
    public async Task<IActionResult> CriarIndicesProduto()
    {
        var comando = new CriarIndicesProdutoComando();
        await mediator.Send(comando);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Autocomplete([FromQuery] string termo, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(termo) || termo.Length < 2)
            return Ok(new List<Produto>());

        var consulta = new ObterAutoCompleteConsulta(termo);
        var resultados = await mediator.Send(consulta, cancellationToken);
        var viewModels = resultados.Select(x => new ProdutoAutoCompleteViewModel(x.Id, x.Nome)).ToList();

        return Ok(viewModels);
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

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiFurb.Api.Conversores;
using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;

namespace RestApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComandaController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IComandaConversor conversor;

    public ComandaController(IMediator mediator, IComandaConversor conversor)
    {
        this.mediator = mediator;
        this.conversor = conversor;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodasComandas()
    {
        try
        {
            var comando = new ObterTodasComandasConsulta();
            var retorno = await mediator.Send(comando);
            var viewModelRetorno = retorno.Select(conversor.ConverterParaViewModel).ToList();
            return Ok(viewModelRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao obter comandas: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterComandaPorId([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("ID da comanda não pode ser vazio.");

        try
        {
            var comando = new ObterComandaPorIdConsulta(id);
            var retorno = await mediator.Send(comando);

            if (retorno == null)
                return NotFound("Comanda não encontrada.");

            var viewModelRetorno = conversor.ConverterParaViewModel(retorno);
            return Ok(viewModelRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao obter comanda: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CriarComanda([FromBody] CriarComandaViewModel viewModel)
    {
        if (viewModel == null)
            return BadRequest("Comanda não pode ser nula.");

        var contrato = conversor.ConverterParaContrato(viewModel);
        var comando = new CriarComandaComando(contrato);

        try
        {
            var retorno = await mediator.Send(comando);
            var viewModelRetorno = conversor.ConverterParaViewModel(retorno);
            return Ok(viewModelRetorno);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao criar comanda: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> AtualizarComanda([FromRoute] Guid id, [FromBody] AtualizarComandaViewModel viewModel)
    {
        if (viewModel == null)
            return BadRequest("Comanda não pode ser nula.");

        var contrato = conversor.ConvertarParaContrato(viewModel);
        var comando = new AtualizarComandaComando(id, contrato);

        try
        {
            await mediator.Send(comando);
            return Ok("Comanda atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao atualizar comanda: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletarComanda([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest("ID da comanda não pode ser vazio.");

        var comando = new DeletarComandaComando(id);

        try
        {
            await mediator.Send(comando);
            return Ok("Comanda deletada com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao deletar comanda: {ex.Message}");
        }
    }
}

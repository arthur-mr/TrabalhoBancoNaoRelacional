using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiFurb.Api.ViewModels;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Consultas;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using System.Text.RegularExpressions;

namespace RestApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IMediator mediator;

    public UsuarioController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioViewModel viewModel)
    {
        var (ehValido, mensagemValidacao) = ValidarViewModel(viewModel);

        if (!ehValido)
            return BadRequest(mensagemValidacao);

        var contrato = new CriarUsuarioContrato(Nome: viewModel.Nome, Email: viewModel.Email, Telefone: viewModel.Telefone);
        var comando = new CriarUsuarioComando(contrato);
        await mediator.Send(comando);

        return Ok("Usuário cadastrado com sucesso!");
    }

    [HttpGet]
    public async Task<IActionResult> ObterUsuarios([FromQuery] int offset)
    {
        var consulta = new ObterUsuariosConsulta(offset);
        var contratos = await mediator.Send(consulta);

        if (contratos is null)
            return null;

        var viewModels = contratos
            .Select(x => new ListarUsuarioViewModel(
                Id: x.Id,
                Nome: x.Nome,
                Email: x.Email,
                Telefone: x.Telefone))
            .ToList();

        return Ok(viewModels);
    }

    private (bool, string) ValidarViewModel(CriarUsuarioViewModel viewModel)
    {
        if (viewModel is null)
            return (false, "Usuário não pode ser nulo.");

        if (string.IsNullOrWhiteSpace(viewModel.Nome))
            return (false, "Nome do usuário é obrigatório.");

        var emailPattern = new Regex("^[A-Za-z0-9._%+-]+@gmail\\.com$");
        if (string.IsNullOrWhiteSpace(viewModel.Email) || !emailPattern.IsMatch(viewModel.Email))
            return (false, "E-mail inválido. Deve ser um endereço @gmail.com.");

        var telefonePattern = new Regex("^\\d{11}$");
        if (string.IsNullOrWhiteSpace(viewModel.Telefone) || !telefonePattern.IsMatch(viewModel.Telefone))
            return (false, "Telefone inválido. Deve conter exatamente 11 dígitos sem pontuações.");

        return (true, string.Empty);
    }
}


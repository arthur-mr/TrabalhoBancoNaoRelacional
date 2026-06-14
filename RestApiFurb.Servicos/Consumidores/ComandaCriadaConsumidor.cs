using MediatR;
using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Mediator.Comandos.Requisicoes;
using RestApiFurb.Dominio.Mensagens;

namespace RestApiFurb.Servicos.Consumidores;

internal sealed class ComandaCriadaConsumidor : ConsumidorBase<ComandaCriadaMensagem>
{
    private readonly IMediator mediator;

    public ComandaCriadaConsumidor(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public override async Task ConsumirMensagem(ComandaCriadaMensagem mensagem)
    {
        Console.WriteLine($"Consumindo mensagem: {mensagem}");
        var contrato = new NotificarComandaCriadaContrato(
            NomeUsuario: mensagem.NomeUsuario,
            EmailUsuario: mensagem.EmailUsuario,
            Comanda: mensagem.Comanda);

        var comando = new NotificarComandaCriadaComando(contrato);

        await mediator.Send(comando);
        Console.WriteLine($"Consumida mensagem: {mensagem}");
    }
}

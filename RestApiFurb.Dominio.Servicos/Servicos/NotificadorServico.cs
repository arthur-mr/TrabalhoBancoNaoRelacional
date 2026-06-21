using RestApiFurb.Dominio.Contratos;
using RestApiFurb.Dominio.Interfaces;
using RestApiFurb.Dominio.Modelos;
using System.Net;
using System.Net.Mail;

namespace RestApiFurb.Dominio.Servicos.Servicos;

internal sealed class NotificadorServico : INotificadorServico
{
    private readonly IRepositorioBase repositorioBase;

    public NotificadorServico(IRepositorioBase repositorioBase)
    {
        this.repositorioBase = repositorioBase;
    }

    public async Task NotificarComandaCriadaAsync(NotificarComandaCriadaContrato contrato, CancellationToken cancellationToken)
    {
        var de = "notificadorcsharp@gmail.com";
        var senha = "fyou ezph wfds nsso";
        var para = contrato.EmailCliente;

        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(de, senha),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var email = new MailMessage(de, para)
        {
            Subject = "Seu pedido já está sendo preparado!",
            Body = $"Olá {contrato.NomeCliente}! Seu pedido já está sendo preparado!",
            IsBodyHtml = false
        };

        smtp.Send(email);
    }
}

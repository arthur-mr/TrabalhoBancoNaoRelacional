using RestApiFurb.Dominio.Contratos;

namespace RestApiFurb.Dominio.Interfaces;

public interface INotificadorServico
{
    Task NotificarComandaCriadaAsync(NotificarComandaCriadaContrato contrato, CancellationToken cancellationToken);
}
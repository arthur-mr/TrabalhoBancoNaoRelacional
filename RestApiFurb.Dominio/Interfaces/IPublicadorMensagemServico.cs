namespace RestApiFurb.Dominio.Interfaces;

public interface IPublicadorMensagemServico
{
    void Publicar<TMensagem>(TMensagem mensagem);
}

using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class OutboxMessage : ModeloBase
{
    public string TipoEvento { get; private set; }
    public string Payload { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Processado { get; private set; }
    public DateTime? DataProcessamento { get; private set; }
    public string Erro { get; private set; }

    protected OutboxMessage() 
    { }

    public OutboxMessage(string tipoEvento, string payload)
    {
        Id = Guid.NewGuid();
        TipoEvento = tipoEvento;
        Payload = payload;
        DataCriacao = DateTime.UtcNow;
        Processado = false;
    }

    public void MarcarComoProcessado()
    {
        Processado = true;
        DataProcessamento = DateTime.UtcNow;
    }

    public void RegistrarErro(string mensagemErro)
    {
        Erro = mensagemErro;
    }
}
namespace RestApiFurb.Dominio.Modelos.Base;

public class ModeloComExclusao : ModeloBase
{
    public bool Ativo { get; private set; }
    public DateTime DataAtivacao { get; private set; }
    public DateTime? DataDesativacao { get; private set; }

    public ModeloComExclusao()
    { }

    public void Desativar()
    {
        DataDesativacao = DateTime.UtcNow;
        Ativo = false;
    }
}

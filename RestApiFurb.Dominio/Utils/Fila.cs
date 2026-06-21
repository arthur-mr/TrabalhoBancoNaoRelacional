namespace RestApiFurb.Dominio.Utils;

[AttributeUsage(AttributeTargets.Class)]
public class Fila : Attribute
{
    public string Nome { get; }

    public Fila(string nome)
    {
        Nome = nome;
    }
}
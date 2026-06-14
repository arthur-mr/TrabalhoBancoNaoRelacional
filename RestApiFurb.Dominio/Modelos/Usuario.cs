using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Usuario : ModeloComExclusao
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }

    public Usuario(string nome, string email, string telefone)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }
}

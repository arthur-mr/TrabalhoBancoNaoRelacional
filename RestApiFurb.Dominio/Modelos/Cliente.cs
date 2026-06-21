using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Dominio.Modelos;

public class Cliente : ModeloComExclusao
{
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }

    public Cliente(string nome, string email, string telefone)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }
}

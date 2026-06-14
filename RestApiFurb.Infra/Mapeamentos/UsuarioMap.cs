using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    private const string NOME_TABELA = "USUARIO";

    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Telefone).HasColumnName("TELEFONE").IsRequired().HasMaxLength(12);
    }
}
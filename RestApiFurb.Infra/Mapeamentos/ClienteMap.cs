using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ClienteMap : IEntityTypeConfiguration<Cliente>
{
    private const string NOME_TABELA = "CLIENTE";

    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Telefone).HasColumnName("TELEFONE").IsRequired().HasMaxLength(12);
    }
}
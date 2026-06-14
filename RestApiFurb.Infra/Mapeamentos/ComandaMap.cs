using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ComandaMap : IEntityTypeConfiguration<Comanda>
{
    private const string NOME_TABELA = "COMANDA";

    public void Configure(EntityTypeBuilder<Comanda> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.UsuarioId).HasColumnName("USUARIO_FK").IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasMany(c => c.ProdutosComanda)
            .WithOne(pc => pc.Comanda)
            .HasForeignKey(pc => pc.ComandaId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
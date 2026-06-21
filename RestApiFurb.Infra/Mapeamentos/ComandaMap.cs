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
        builder.Property(x => x.ClienteId).HasColumnName("CLIENTE_FK").IsRequired(false);
        builder.Property(x => x.Identificacao).HasColumnName("IDENTIFICACAO").IsRequired();
        builder.Property(x => x.Status).HasColumnName("STATUS").IsRequired();

        builder.HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.ClienteId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasMany(c => c.ProdutosComanda)
            .WithOne(pc => pc.Comanda)
            .HasForeignKey(pc => pc.ComandaId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
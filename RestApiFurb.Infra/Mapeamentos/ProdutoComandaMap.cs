using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ProdutoComandaMap : IEntityTypeConfiguration<ProdutoComanda>
{
    private const string NOME_TABELA = "PRODUTO_COMANDA";

    public void Configure(EntityTypeBuilder<ProdutoComanda> builder)
    {
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.ComandaId).HasColumnName("COMANDA_FK").IsRequired();
        builder.Property(x => x.ProdutoId).HasColumnName("PRODUTO_FK").IsRequired();
        builder.Property(x => x.Quantidade).HasColumnName("QUANTIDADE").IsRequired();

        builder.HasOne(x => x.Produto)
            .WithMany()
            .HasForeignKey(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}
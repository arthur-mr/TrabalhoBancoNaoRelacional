using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;
using RestApiFurb.Infra.Mapeamentos.Base;

namespace RestApiFurb.Infra.Mapeamentos;

internal class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    private const string NOME_TABELA = "PRODUTO";

    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        MapeamentoComum.MapearComum(builder);
        builder.ToTable(NOME_TABELA);
        builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
        builder.Property(x => x.Preco).HasColumnName("PRECO").HasPrecision(10,2).IsRequired();
        builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
        builder.Property(x => x.CodigoBarras).HasColumnName("CODIGO_BARRAS").IsRequired();
        builder.Property(x => x.Categoria).HasColumnName("CATEGORIA").IsRequired();
        builder.Property(x => x.QuantidadeEstoque).HasColumnName("QUANTIDADE_ESTOQUE").IsRequired();
    }
}
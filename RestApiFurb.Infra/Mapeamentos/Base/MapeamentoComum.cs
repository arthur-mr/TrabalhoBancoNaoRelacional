using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos.Base;

namespace RestApiFurb.Infra.Mapeamentos.Base;

public static class MapeamentoComum
{
    public static void MapearComum<T>(EntityTypeBuilder<T> builder) where T : ModeloComExclusao
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
        builder.Property(x => x.Ativo).HasColumnName("ATIVO").IsRequired().HasDefaultValueSql("1").ValueGeneratedOnAdd();
        builder.Property(x => x.DataAtivacao)
            .HasColumnName("DATA_ATIVACAO")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder.Property(x => x.DataDesativacao).HasColumnName("DATA_DESATIVACAO").IsRequired(false);
    }
}
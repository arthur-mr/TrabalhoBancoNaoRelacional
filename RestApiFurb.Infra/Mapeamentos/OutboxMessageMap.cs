using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestApiFurb.Dominio.Modelos;

namespace RestApiFurb.Infra.Mapeamentos;

internal class OutboxMessageMap : IEntityTypeConfiguration<OutboxMessage>
{
    private const string NOME_TABELA = "OUTBOX_MESSAGE";

    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(NOME_TABELA);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
        builder.Property(x => x.TipoEvento).HasColumnName("TIPO_EVENTO").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Payload).HasColumnName("PAYLOAD").IsRequired();
        builder.Property(x => x.DataCriacao).HasColumnName("DATA_CRIACAO").IsRequired();
        builder.Property(x => x.Processado).HasColumnName("PROCESSADO").IsRequired();
        builder.Property(x => x.DataProcessamento).HasColumnName("DATA_PROCESSAMENTO").IsRequired(false);
        builder.Property(x => x.Erro).HasColumnName("ERRO").IsRequired(false);
    }
}
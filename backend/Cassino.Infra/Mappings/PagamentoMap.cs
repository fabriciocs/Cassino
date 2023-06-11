using Cassino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cassino.Infra.Mappings;

public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder
            .Property(a => a.Valor)
            .IsRequired();
        
        builder
            .Property(c => c.Conteudo)
            .IsRequired()
            .HasMaxLength(6000);

        builder
            .Property(c => c.Aprovado)
            .IsRequired();

        builder
            .Property(c => c.DataPagamento)
            .IsRequired();
            
        builder
            .HasOne(c => c.Usuario)
            .WithMany(c => c.Pagamentos)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
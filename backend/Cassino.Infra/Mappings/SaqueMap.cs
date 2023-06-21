using Cassino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cassino.Infra.Mappings;

public class SaqueMap : IEntityTypeConfiguration<Saque>
{
    public void Configure(EntityTypeBuilder<Saque> builder)
    {
        builder
            .Property(a => a.Valor)
            .IsRequired();
        
        builder
            .Property(a => a.DataSaque)
            .HasDefaultValue(DateTime.Now)
            .IsRequired();
        
        builder
            .Property(a => a.Aprovado)
            .HasDefaultValue(true)
            .IsRequired();
        
        builder
            .HasOne(c => c.Usuario)
            .WithMany(c => c.Saques)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }

}
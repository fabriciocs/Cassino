using Cassino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Infra.Mappings
{
    public class ApostaMap : IEntityTypeConfiguration<Aposta>
    {
        public void Configure(EntityTypeBuilder<Aposta> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Apostas)
                .HasForeignKey(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(a => a.Valor)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}

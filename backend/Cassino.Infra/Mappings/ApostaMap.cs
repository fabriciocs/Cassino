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
                .Property(a => a.IdUsuario);

            builder
                .Property(a => a.Valor)
                .IsRequired()
                .HasPrecision(18, 2);
        }
    }
}

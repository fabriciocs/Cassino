using Cassino.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cassino.Domain.Validation
{
    public class ApostaValidator : AbstractValidator<Aposta>
    {
        public ApostaValidator()
        {
            RuleFor(a => a.IdUsuario)
                .NotEmpty();

            RuleFor(a => a.Valor) 
                .NotEmpty();

            RuleFor(a => a.Data)
                .NotEmpty();

            RuleFor(a => a.Jogo)
                .NotEmpty();
        }
    }
}

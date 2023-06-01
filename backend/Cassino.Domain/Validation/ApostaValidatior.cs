using Cassino.Domain.Entities;
using FluentValidation;

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

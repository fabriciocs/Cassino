using Cassino.Domain.Entities;
using FluentValidation;

namespace Cassino.Domain.Validation
{
    public class ApostaValidator : AbstractValidator<Aposta> // não tá sendo usado
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

            RuleFor(a => a.EhApostaInicial)
                .NotEmpty();
        }
    }
}

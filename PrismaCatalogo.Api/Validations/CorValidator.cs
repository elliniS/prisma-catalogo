using FluentValidation;
using PrismaCatalogo.Api.Extensions;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Validations
{
    public class CorValidator : AbstractValidator<Cor>
    {
        public CorValidator(IEnumerable<Cor> cor)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para a cor!")
                .IsUnique(cor)
                .WithMessage("Outra cor já esta usando este nome!!");
        }
    }
}

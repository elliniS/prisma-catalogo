using FluentValidation;
using Newtonsoft.Json.Linq;
using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
                .WithMessage("Cor já existe!");
        }
    }
}

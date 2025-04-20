using FluentValidation;
using Newtonsoft.Json.Linq;
using PrismaCatalogo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class CorValidator : AbstractValidator<CorViewModel>
    {
        public CorValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para a cor!");
        }
    }
}

using FluentValidation;
using Newtonsoft.Json.Linq;
using PrismaCatalogo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class TamanhoValidator : AbstractValidator<TamanhoViewModel>
    {
        public TamanhoValidator(IEnumerable<TamanhoViewModel> tamanhos)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o tamanho!")
                .IsUnique(tamanhos)
                .WithMessage("Outro tamanho já esta usando este nome!!");
        }
    }
}

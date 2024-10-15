using FluentValidation;
using Newtonsoft.Json.Linq;
using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class TamanhoValidator : AbstractValidator<Tamanho>
    {
        public TamanhoValidator(IEnumerable<Tamanho> tamanhos)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o tamanho!")
                .IsUnique(tamanhos)
                .WithMessage("Outro tamanho já esta usando este nome!!");
        }
    }
}

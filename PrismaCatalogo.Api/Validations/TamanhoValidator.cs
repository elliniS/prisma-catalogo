using FluentValidation;
using PrismaCatalogo.Api.Context;
using PrismaCatalogo.Api.DTOs;
using PrismaCatalogo.Api.Extensions;
using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Api.Validations
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

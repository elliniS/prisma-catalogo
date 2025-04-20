using FluentValidation;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class ProdutoFilhoValidator : AbstractValidator<ProdutoFilhoViewModel>
    {
        public ProdutoFilhoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o produto!");
        }
    }
}

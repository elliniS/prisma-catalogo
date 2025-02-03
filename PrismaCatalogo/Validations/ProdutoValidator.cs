using FluentValidation;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class ProdutoValidator : AbstractValidator<ProdutoViewModel>
    {
        public ProdutoValidator(IEnumerable<ProdutoViewModel> produto)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o produto!")
                .IsUnique(produto)
                .WithMessage("Outro produto já esta usando este nome!");
        }
    }
}

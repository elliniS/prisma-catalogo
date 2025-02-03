using FluentValidation;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PrismaCatalogo.Validations
{
    public class ProdutoFilhoValidator : AbstractValidator<ProdutoFilhoViewModel>
    {
        public ProdutoFilhoValidator(IEnumerable<ProdutoFilhoViewModel> produtoFilho)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para o produto!")
                .IsUnique(produtoFilho)
                .WithMessage("Outro produto já esta usando este nome!");
        }
    }
}

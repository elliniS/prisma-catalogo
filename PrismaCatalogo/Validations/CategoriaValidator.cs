using FluentValidation;
using PrismaCatalogo.Web.Models;

namespace PrismaCatalogo.Validations
{
    public class CategoriaValidator : AbstractValidator<CategoriaViewModel>
    {
        public CategoriaValidator(IEnumerable<CategoriaViewModel> categorias) 
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("Informe algum nome para a categoria!")
                .IsUnique(categorias)
                .WithMessage("Outra categoria já esta usando este nome!");
        }

        public CategoriaValidator()
        {
            RuleFor(c => c.CategoriasFilhas)
                .Empty()
                .WithMessage("Categoria não pode ser deletada, pois possui categorias filhas!");
        }
    }
}

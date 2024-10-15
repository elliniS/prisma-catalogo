using FluentValidation;
using PrismaCatalogo.Models;

namespace PrismaCatalogo.Validations
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator(IEnumerable<Categoria> categorias) 
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

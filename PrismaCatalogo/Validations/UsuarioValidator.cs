using FluentValidation;
using PrismaCatalogo.Web.Models;

namespace PrismaCatalogo.Validations
{
    public class UsuarioValidator : AbstractValidator<UsuarioViewModel>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe um nome para o usuario!");

            RuleFor(x => x.NomeUsuario)
                .NotEmpty()
                .WithMessage("Informe um nome para o usuario!");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Informe uma senha para o usuario!");
        }
    }
}

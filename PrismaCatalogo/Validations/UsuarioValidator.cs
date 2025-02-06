using FluentValidation;
using PrismaCatalogo.Web.Models;

namespace PrismaCatalogo.Validations
{
    public class UsuarioValidator : AbstractValidator<UsuarioViewModel>
    {
        public UsuarioValidator(IEnumerable<UsuarioViewModel> usuarios)
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe um nome para o usuario!");

            RuleFor(x => x.NomeUsuario)
                .NotEmpty()
                .WithMessage("Informe um nome para o usuario!")
                .IsUnique(usuarios)
                .WithMessage("Outro usuario já esta usando este nome!!");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Informe uma senha para o usuario!");
        }
    }
}

using FluentValidation;
using PrismaCatalogo.Api.Extensions;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Validations
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator(IEnumerable<Usuario> usuarios)
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

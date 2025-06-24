using PrismaCatalogo.Api.Enuns;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.UsuarioDTO
{
    public class UsuarioRequestDTO
    {
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public EnumUsuarioTipo UsuarioTipo { get; set; }
    }
}

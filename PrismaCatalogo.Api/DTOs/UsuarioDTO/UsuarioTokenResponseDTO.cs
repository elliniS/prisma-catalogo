using PrismaCatalogo.Api.Enuns;
using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.UsuarioDTO
{
    public class UsuarioTokenResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public EnumUsuarioTipo UsuarioTipo { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

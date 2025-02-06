using PrismaCatalogo.Api.Enuns;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.UsuarioDTO
{
    public class UsuarioLoginRequestDTO
    {
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
    }
}

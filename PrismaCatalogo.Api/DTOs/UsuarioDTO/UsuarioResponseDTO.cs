using PrismaCatalogo.Api.Enuns;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.UsuarioDTO
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public EnumUsuarioTipo UsuarioTipo { get; set; }
    }
}

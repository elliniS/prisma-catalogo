using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CodigoReenviaSenhaDTO
{
    public class CodigoReenviaSenhaRequestDTO
    {
        public int UsuarioId { get; set; }
        public string? Codigo { get; set; }
        public string? Senha {  get; set; } 
    }
}

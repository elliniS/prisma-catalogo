using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CorDTO
{
    public class CorRequestDTO
    {
        public string? Nome { get; set; }
        public string? CodigoHexadecimal { get; set; }
        public string? FotoBytes { get; set; }
    }
}

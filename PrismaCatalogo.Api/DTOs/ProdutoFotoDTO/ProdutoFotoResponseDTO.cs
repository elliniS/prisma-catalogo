using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CorDTO
{
    public class ProdutoFotoResponseDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string? FotoBytes { get; set; }
        public string Caminho { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CorDTO
{
    public class ProdutoFilhoFotoResponseDTO
    {
        public int Id { get; set; }
        public int ProdutoFilhoId { get; set; }
        public string? FotoBytes { get; set; }
        public string? Caminho { get; set; }
    }
}

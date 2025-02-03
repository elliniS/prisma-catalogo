using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CorDTO
{
    public class ProdutoFotoRequestDTO
    {
        public int? Id { get; set; }
        public int ProdutoId { get; set; } 
        public string? FotoBytes { get; set; }
        public bool FgExcluir { get; set; }
    }
}

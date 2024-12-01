using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CategoriaDTO
{
    public class CategoriaResponseDTO
    {
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public string Nome { get; set; }
        public IEnumerable<CategoriaResponseDTO>? CategoriasFilhas { get; set; }
    }
}

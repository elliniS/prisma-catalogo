using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CategoriaDTO
{
    public class CategoriaRequestDTO
    {
        public int? IdPai { get; set; }
        public string Nome { get; set; }
    }
}

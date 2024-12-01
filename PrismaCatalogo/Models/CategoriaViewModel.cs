using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        
        public string? Nome { get; set; }
        [Display(Name = "Categoria pai")]
        public int? IdPai { get; set; }
        [Display(Name = "Categorias filhas")]
        public ICollection<CategoriaViewModel>? CategoriasFilhas { get; set; }
    }
}

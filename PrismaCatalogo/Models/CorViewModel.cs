using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class CorViewModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        [Display(Name = "Codigo Hexadecimal")]
        public string? CodigoHexadecimal { get; set; }
        [Display(Name = "Foto")]
        public string? FotoBytes { get; set; }
    }
}

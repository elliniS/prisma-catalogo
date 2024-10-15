using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace PrismaCatalogo.Models
{
    public class Cor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Codigo Hexadecimal")]
        public string? CodigoHexadecimal { get; set; }
        [Display(Name = "Foto")]
        public string? FotoBytes { get; set; }
    }
}

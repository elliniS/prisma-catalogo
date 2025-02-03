using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace PrismaCatalogo.Api.Models
{
    public class Cor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Codigo Hexadecimal")]
        public string? CodigoHexadecimal { get; set; }
        [Display(Name = "Foto")]
        public string? FotoBytes { get; set; }


        public ICollection<ProdutoFilho>? ProdutosFilhos { get; set; }
    }
}

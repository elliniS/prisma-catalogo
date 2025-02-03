using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class ProdutoFotoViewModel
    {
        public int Id { get; set; }

        public string? FotoBytes { get; set; }
        public bool FgExcluir { get; set; }

        //[Display(Name = "Fotos")]
        //public IEnumerable<ProdutoFilhoFotoViewModel>? produtoFilhoFotos { get; set; }
    }
}

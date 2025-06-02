using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class FotoViewModel
    {
        public int Id { get; set; }

        public bool FgPrincipal { get; set; }
        public string? FotoBytes { get; set; }
        public string? Caminho { get; set; }
        public bool FgExcluir { get; set; }

        //[Display(Name = "Fotos")]
        //public IEnumerable<ProdutoFilhoFotoViewModel>? produtoFilhoFotos { get; set; }
    }
}

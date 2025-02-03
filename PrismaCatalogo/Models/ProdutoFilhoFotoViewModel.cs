using Newtonsoft.Json;

namespace PrismaCatalogo.Web.Models
{
    public class ProdutoFilhoFotoViewModel
    {
        public int Id { get; set; }

        public string? FotoBytes { get; set; }
        public bool FgExcluir { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }   
        public string? Observacao { get; set; }  
        public bool Ativo { get; set; }
        public ProdutoFotoViewModel? FotoCapa { get; set; }

        [Display(Name = "Produtos filhos")]
        public IEnumerable<ProdutoFilhoViewModel>? ProdutosFilhos { get; set; }

        public List<ProdutoFotoViewModel>? Fotos { get; set; }
    }
}

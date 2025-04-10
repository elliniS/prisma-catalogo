using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Observacao { get; set; }
        public bool Ativo { get; set; }
        
        public ICollection<ProdutoFoto>? Fotos { get; set; }

        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public ICollection<ProdutoFilho>? ProdutosFilhos { get; set; }
    }
}

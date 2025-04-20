using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismaCatalogo.Api.Models
{
    public class ProdutoFilho
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public double? Preco { get; set; }
        public int QuantEstoque { get; set; }
        public bool Ativo { get; set; }
        //public bool Visivel { get; set; }

        //[Display(Name = "Fotos")]
        //public IEnumerable<string>? FotoBytes { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public int? CorId { get; set; }
        public Cor? Cor { get; set; }

        public int? TamanhoId { get; set; } 
        public Tamanho? Tamanho { get; set; }

        public ICollection<ProdutoFilhoFoto>? Fotos { get; set; }


        [NotMapped]
        public ProdutoFoto? Fotocapa { get; set; }
    }
}

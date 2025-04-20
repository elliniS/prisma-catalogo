using PrismaCatalogo.Api.DTOs.CategoriaDTO;
using PrismaCatalogo.Api.DTOs.CorDTO;

namespace PrismaCatalogo.Api.DTOs.ProdutoDTO
{
    public class ProdutoRequestDTO
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; } 
        public string? Descricao { get; set; }
        public string? Observacao { get; set; }
        //public ProdutoFotoRequestDTO? FotoCapa { get; set; }

        public int? CategoriaId { get; set; }
        //public CategoriaRequestDTO Categoria { get; set; }

        public IEnumerable<ProdutoFotoRequestDTO>? Fotos { get; set; }
    }
}

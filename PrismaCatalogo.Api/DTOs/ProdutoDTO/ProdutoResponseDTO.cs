using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.DTOs.ProdutoFilhoDTO;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.DTOs.ProdutoDTO
{
    public class ProdutoResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Observacao { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<ProdutoFilhoResponseDTO> ProdutosFilhos { get; set; }
        public IEnumerable<ProdutoFotoResponseDTO> Fotos { get; set; }
    }
}

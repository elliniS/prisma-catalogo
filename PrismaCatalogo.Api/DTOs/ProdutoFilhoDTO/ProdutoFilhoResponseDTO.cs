using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.DTOs.CorDTO;
using PrismaCatalogo.Api.DTOs.TamanhoDTO;

namespace PrismaCatalogo.Api.DTOs.ProdutoFilhoDTO
{
    public class ProdutoFilhoResponseDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public double? Preco { get; set; }
        public int QuantEstoque { get; set; }
        public bool Ativo {  get; set; }

        //public int? CorId { get; set; }
        //public int? TamanhoId { get; set; }
        public CorResponseDTO Cor { get; set; }
        public TamanhoResponseDTO Tamanho { get; set; }

        public IEnumerable<ProdutoFilhoFotoResponseDTO> Fotos { get; set; }
    }
}

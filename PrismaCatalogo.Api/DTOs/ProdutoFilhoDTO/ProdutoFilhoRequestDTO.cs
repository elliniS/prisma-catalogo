using PrismaCatalogo.Api.DTOs.CorDTO;

namespace PrismaCatalogo.Api.DTOs.ProdutoFilhoDTO
{
    public class ProdutoFilhoRequestDTO
    {
        public string Nome { get; set; }
        public int QuantEstoque { get; set; }
        public double? Preco { get; set; }
        public bool Ativo {  get; set; }

        public int ProdutoId { get; set; }
        public int? CorId { get; set; }
        public int? TamanhoId { get; set; }

        public IEnumerable<ProdutoFilhoFotoRequestDTO> Fotos { get; set; }
    }
}

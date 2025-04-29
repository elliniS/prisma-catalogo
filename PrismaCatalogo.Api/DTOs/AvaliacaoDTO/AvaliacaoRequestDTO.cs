using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.AvaliacaoDTO
{
    public class AvaliacaoRequestDTO
    {
        public int Nota { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DtInclusao { get; set; }
        public int UsuarioId { get; set; }
        public int ProdutoId { get; set; }
    }
}

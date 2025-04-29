using PrismaCatalogo.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.AvaliacaoDTO
{
    public class AvaliacaoResponseDTO
    {
        public int Id { get; set; }
        public int Nota { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DtInclusao { get; set; }

        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        //public Usuario Usuario { get; set; }

        public int ProdutoId { get; set; }
        //public Produto Produto { get; set; }
    }
}

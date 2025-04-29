using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class AvaliacaoViewModel
    {
        public int Id { get; set; }
        [Display(Name ="")]
        public int Nota { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DtInclusao { get; set; }

        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        //public Usuario Usuario { get; set; }

        public int ProdutoId { get; set; }
    }
}

namespace PrismaCatalogo.Api.Models
{
    public class CodigoReenviaSenha
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Codigo { get; set; }
        public bool usado { get; set; }
        public DateTime DtCriado { get; set; }
    }
}

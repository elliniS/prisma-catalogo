namespace PrismaCatalogo.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public string RefreshTokenValue { get; set; }
    }
}

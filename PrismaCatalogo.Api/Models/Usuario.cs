using PrismaCatalogo.Api.Enuns;

namespace PrismaCatalogo.Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public EnumUsuarioTipo UsuarioTipo { get; set; }
    }
}

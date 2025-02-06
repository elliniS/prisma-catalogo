using Newtonsoft.Json;
using PrismaCatalogo.Enuns;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Nome de Usuario")]
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public bool FgLembra { get; set; }
        public EnumUsuarioTipo UsuarioTipo { get; set; }
    }
}

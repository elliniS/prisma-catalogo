using Newtonsoft.Json;
using PrismaCatalogo.Enuns;
using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Web.Models
{
    public class UsuarioLoginViewModel
    {
        [Display(Name = "Nome de Usuario")]
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        [Display(Name = "Lembre-se de mim")]
        public bool FgLembra { get; set; }
        public string? ReturnUrl { get; set; }
        //public EnumUsuarioTipo UsuarioTipo { get; set; }
    }
}

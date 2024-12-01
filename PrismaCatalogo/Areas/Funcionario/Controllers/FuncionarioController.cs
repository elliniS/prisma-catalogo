using Microsoft.AspNetCore.Mvc;


namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

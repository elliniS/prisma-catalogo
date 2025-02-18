using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{

    [Area("Funcionario")]
    public class FuncionarioController : Controller
    {
        [Authorize(Roles = "Funcionario")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

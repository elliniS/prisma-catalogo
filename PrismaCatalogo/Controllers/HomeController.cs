using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Diagnostics;

namespace PrismaCatalogo.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProdutoService _produtoService;

        public HomeController(ILogger<HomeController> logger, IProdutoService produtoService)
        {
            _logger = logger;
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index()
        { 
            var produtos = (await _produtoService.GetAll()).Where(p => p.Ativo = true);

            foreach (var produto in produtos)
            {
                produto.FotoCapa = produto.Fotos.OrderBy(f => f.Id).FirstOrDefault();

                //if (produto.Fotos != null && produto.Fotos.Count() > 0)
                //    produto.Fotos.OrderBy(f => f.Id).FirstOrDefault().FgPrincipal = true;

            }

            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

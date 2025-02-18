using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Diagnostics;

namespace PrismaCatalogo.Web.Controllers
{
    //[Authorize]
    public class ProdutoController : Controller
    {
        private IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService) 
        {
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index(int produtoId)
        { 
            var produto = await _produtoService.FindById(produtoId);

            if (produto.ProdutosFilhos != null) {
                var tamanhos= produto.ProdutosFilhos.Select(p => p.Tamanho).Distinct().ToList();

                //ViewBag.ProdutoTamanhos = new SelectList(tamanhos, "Id", "Nome");
                var comp = new CorComparer();

                ViewBag.ProdutoCores = produto.ProdutosFilhos.Select(p => p.Cor).Distinct(comp).ToList();

            }

            var produtoDetalhe = new ProdutoDetalheViewModel(produto);

            //if (produto.Fotos != null && produto.Fotos.Count() > 0)
            //    produto.Fotos.OrderBy(f => f.Id).FirstOrDefault().FgPrincipal = true;

            
            return View(produtoDetalhe);

        }

        [HttpPost]
        public async Task<IActionResult> SelecionaFilho(int produtoId, int? corId, int? tamanhoId)
        {
            var produto = await _produtoService.FindById(produtoId);

            if (produto.ProdutosFilhos != null)
            {
                var comp = new CorComparer();
                ViewBag.ProdutoCores = produto.ProdutosFilhos.Select(p => p.Cor).Distinct(comp).ToList();
            }

            ProdutoDetalheViewModel produtoDetalhe = tamanhoId ==null ? new ProdutoDetalheViewModel(produto, corId) : new ProdutoDetalheViewModel(produto, corId, tamanhoId);



            var selectListsTamanho = new SelectList(produtoDetalhe.Tamanhos, "Id", "Nome");

            if(tamanhoId != null)
            {
                var item = selectListsTamanho.Where(p => p.Value == tamanhoId.ToString()).FirstOrDefault();
                
                if (item != null)
                {
                    item.Selected = true;
                } 
            }

            ViewBag.ProdutoTamanhos = selectListsTamanho;



            return View("index", produtoDetalhe);
        }

        //[HttpPost]
        //public async Task<IActionResult> SelecionaFilho(int produtoId, int? corId, int? tamahoId)
        //{
        //    var produto = await _produtoService.FindById(produtoId);

        //    if (produto.ProdutosFilhos != null)
        //    {
        //        var tamanhos = produto.ProdutosFilhos.Select(p => p.Tamanho).Distinct().ToList();

        //        ViewBag.ProdutoTamanhos = new SelectList(tamanhos, "Id", "Nome");
        //        ViewBag.ProdutoCores = produto.ProdutosFilhos.Select(p => p.Cor).Distinct().ToList();
        //    }

        //    var produtoDetalhe = new ProdutoDetalheViewModel(produto, corId, tamahoId);

        //    return View();
        //}
    }
}

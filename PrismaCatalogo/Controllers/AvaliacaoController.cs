using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    
    public class AvaliacaoController : Controller
    {
        private IAvaliacaoService _avaliacaoService;

        public AvaliacaoController(IAvaliacaoService avaliacao)
        {
            _avaliacaoService = avaliacao;
        }

        [HttpGet]
        public async Task<IActionResult> ObterAvaliacoes(int produtoId)
        {
            var avaliacaos = await _avaliacaoService.FindByProduto(produtoId);

            if(avaliacaos == null)
            {
                avaliacaos = new List<AvaliacaoViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar avaliacao!";
            }

            //ViewData["mensagemError"] = TempData["mensagemError"];

            string idusuario = User.FindFirst("Id")?.Value.Trim();

            if (idusuario != null)
            {
                avaliacaos = avaliacaos.Where(a => a.UsuarioId != Convert.ToInt32(idusuario)).ToList();
            }
            return PartialView("_Avaliacoes", avaliacaos);
            
        }

        [HttpGet]
        public async Task<IActionResult> ObterAvaliacaoUsuario(int produtoId, int usuarioId)
        {
            try
            {
                var avaliacao = await _avaliacaoService.FindByProdutoUsuario(produtoId, usuarioId);

                if (avaliacao == null)
                {
                    avaliacao = new AvaliacaoViewModel() { 
                        ProdutoId = produtoId,
                        UsuarioId = usuarioId
                    };
                }

                return PartialView("_AvaliacaoUsuario", avaliacao);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //// GET: Funcionario/Avaliacao/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    try
        //    {
        //        var avaliacao = await _avaliacaoService.FindById(Convert.ToInt32(id));

        //        if (avaliacao == null)
        //        {
        //            throw new Exception();
        //        }

        //        return View(avaliacao);
        //    }
        //    catch
        //    {
        //        ViewData["mensagemError"] = "Erro ao buscar avaliacao!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// GET: Avaliacao/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([Bind("Id,Nota,Mensagem,UsuarioId,ProdutoId")] AvaliacaoViewModel avaliacao)
        {
            try
            {
                if(avaliacao.Id == 0) 
                { 
                    var result = await _avaliacaoService.Create(avaliacao);
                }
                else
                {
                    var result = await _avaliacaoService.Update(avaliacao.Id, avaliacao);
                }
            }
            catch(Exception e)
            {
                //ViewData["mensagemError"] = e.Message;
                return StatusCode(500, new { mensagem = "Erro ao salvar avaliação", erro = e.Message });
            }


            return Json(new { sucesso = true, mensagem = "Avaliação salva com sucesso!" });
        }

        //// GET: Avaliacao/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    try {
        //        var avaliacao = await _avaliacaoService.FindById(Convert.ToInt32(id));

        //        if (avaliacao == null)
        //        {
        //            throw new Exception();                    
        //        }
        //        return View(avaliacao);
        //    }
        //     catch
        //    {
        //        ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        //// POST: Avaliacao/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] AvaliacaoViewModel avaliacaoViewModel)
        //{
        //    //AvaliacaoValidator validations = new AvaliacaoValidator();
        //    //var resul = validations.Validate(avaliacaoViewModel);

        //    //if (resul.IsValid)
        //   // {
        //        try
        //        {
        //            var re = await _avaliacaoService.Update(id, avaliacaoViewModel);
        //           // return RedirectToAction(nameof(Index));
        //        }
        //        catch(Exception e)
        //        {
        //            ViewData["mensagemError"] = e.Message;
        //        }
        //   // }

        //    //ModelState.Clear();
        //   // resul.AddToModelState(ModelState);

        //    return View(avaliacaoViewModel);
        //}

        // GET: Avaliacao/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    try
        //    {
        //        var avaliacao = await _avaliacaoService.FindById(Convert.ToInt32(id));

        //        if (avaliacao == null)
        //        {
        //            throw new Exception();
        //        }

        //        return View(avaliacao);
        //    }
        //    catch
        //    {
        //        ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        // POST: Avaliacao/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var re = await _avaliacaoService.Delete(id);
            }
            catch(Exception e)
            {
                return StatusCode(500, new { mensagem = "Erro ao salvar avaliação", erro = e.Message });
            }

            return Json(new { sucesso = true, mensagem = "Avaliação deletada com sucesso!" });
        }

    }
}

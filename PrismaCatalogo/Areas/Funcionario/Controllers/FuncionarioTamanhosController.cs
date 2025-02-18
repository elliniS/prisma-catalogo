using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    [Authorize(Roles = "Funcionario")]
    [Area("Funcionario")]
    public class FuncionarioTamanhosController : Controller
    {
        private ITamanhoService _tamanhoService;

        public FuncionarioTamanhosController(ITamanhoService tamanho)
        {
            _tamanhoService = tamanho;
        }

        // GET: Funcionario/FuncionarioTamanhos
        public async Task<IActionResult> Index()
        {
            var tamanhos = await _tamanhoService.GetAll();

            if(tamanhos == null)
            {
                tamanhos = new List<TamanhoViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar tamanho!";
            }

            return View(tamanhos);
        }

        // GET: Funcionario/FuncionarioTamanhos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var tamanho = await _tamanhoService.FindById(Convert.ToInt32(id));

                if (tamanho == null)
                {
                    throw new Exception();
                }

                return View(tamanho);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao buscar tamanho!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Funcionario/FuncionarioTamanhos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionario/FuncionarioTamanhos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TamanhoViewModel tamanho)
        {
            TamanhoValidator validations = new TamanhoValidator(await _tamanhoService.GetAll());
            var resul = validations.Validate(tamanho);

            if (resul.IsValid)
            {
                try
                {
                    var result = await _tamanhoService.Create(tamanho);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao cadastrar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(tamanho);
        }

        // GET: Funcionario/FuncionarioTamanhos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                var tamanho = await _tamanhoService.FindById(Convert.ToInt32(id));

                if (tamanho == null)
                {
                    throw new Exception();                    
                }
                return View(tamanho);
            }
             catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioTamanhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TamanhoViewModel tamanhoViewModel)
        {
            var tamanhos = (await _tamanhoService.GetAll()).Where(t => t.Id != id);
            TamanhoValidator validations = new TamanhoValidator(tamanhos);
            var resul = validations.Validate(tamanhoViewModel);

            if (resul.IsValid)
            {
                try
                {
                    var re = await _tamanhoService.Update(id, tamanhoViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao atualizar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(tamanhoViewModel);
        }

        // GET: Funcionario/FuncionarioTamanhos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var tamanho = await _tamanhoService.FindById(Convert.ToInt32(id));

                if (tamanho == null)
                {
                    throw new Exception();
                }

                return View(tamanho);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioTamanhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tamanho = await _tamanhoService.FindById(id);
            if (tamanho != null)
            {
                try
                {
                    var re = await _tamanhoService.Delete(id);
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao deletar!";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TamanhoExists(int id)
        {
            return _tamanhoService.FindById(id).Result != null;
        }
    }
}

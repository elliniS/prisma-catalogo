using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    [Authorize(Roles = "Funcionario")]
    [Area("Funcionario")]
    public class FuncionarioCategoriasController : Controller
    {
        private ICategoriaService _categoriasService;

        public FuncionarioCategoriasController(ICategoriaService categoria)
        {
            _categoriasService = categoria;
        }

        // GET: Funcionario/Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriasService.GetAll();

            if (categorias == null)
            {
                categorias = new List<CategoriaViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar categoria!";
            }

            ViewData["mensagemError"] = TempData["mensagemError"];

            return View(categorias);
        }

        // GET: Funcionario/Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var categoria = await _categoriasService.FindById(Convert.ToInt32(id));

                if (categoria == null)
                {
                    throw new Exception();
                }

                return View(categoria);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao encontar categoria!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Funcionario/Categorias/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll() , "Id", "Id");
            return View();
        }

        // POST: Funcionario/Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPai,Nome")] CategoriaViewModel categoriaViewModel)
        {
            CategoriaValidator validationRules = new CategoriaValidator();

            var result = validationRules.Validate(categoriaViewModel);

            if (result.IsValid)
            {
                try{
                    var val = await _categoriasService.Create(categoriaViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e) {
                    ViewData["mensagemError"] = e.Message;
                }
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll(), "Id", "Id", categoriaViewModel.IdPai);
            return View(categoriaViewModel);
        }

        // GET: Funcionario/Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var categoria = await _categoriasService.FindById(Convert.ToInt32(id));

                if (categoria == null)
                {
                    throw new Exception();
                }

                //ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll(), "Id", "Id", categoria.IdPai);
                return View(categoria);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao encontar categoria!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPai,Nome")] CategoriaViewModel categoriaViewModel)
        {
            CategoriaValidator validationRules = new CategoriaValidator();
            var result = validationRules.Validate(categoriaViewModel);

            if (result.IsValid)
            {
                try
                {
                    var re = await _categoriasService.Update(id, categoriaViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e) 
                {
                    ViewData["mensagemError"] = e.Message;
                }
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            //ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll(), "Id", "Id", categoriaViewModel.IdPai);
            return View(categoriaViewModel);
        }

        // GET: Funcionario/Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var categoria = await _categoriasService.FindById(Convert.ToInt32(id));
                if (categoria == null)
                {
                    throw new Exception();
                }

                return View(categoria);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try{
                var val = await _categoriasService.Delete(id);
            }
            catch(Exception e)
            {
                TempData["mensagemError"] = e.Message;
            }

            return RedirectToAction(nameof(Index));

        }

        private bool CategoriaExists(int id)
        {
            return _categoriasService.FindById(id).Result != null;
        }
    }
}

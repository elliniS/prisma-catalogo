﻿using System;
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

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
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
            List<CategoriaViewModel> categorias = new List<CategoriaViewModel>();

            if (categoriaViewModel.IdPai == null)
            {
                categorias = (await _categoriasService.GetAll()).Where(c => c.IdPai == null).ToList();
            }
            else
            {
                categorias = (await _categoriasService.GetAll()).Where(c => c.IdPai == categoriaViewModel.IdPai).ToList();   
            }

            CategoriaValidator validationRules = new CategoriaValidator(categorias);

            var result = validationRules.Validate(categoriaViewModel);

            if (result.IsValid)
            {
                var val = await _categoriasService.Create(categoriaViewModel);
                return RedirectToAction(nameof(Index));
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

                ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll(), "Id", "Id", categoria.IdPai);
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

            List<CategoriaViewModel> categorias = new List<CategoriaViewModel>();

            if (categoriaViewModel.IdPai == null) {
                categorias = (await _categoriasService.GetAll()).Where(c => c.IdPai == null && c.Id != categoriaViewModel.Id).ToList();
            }
            else
            {
                categorias = categorias.Where(c => c.Id == categoriaViewModel.IdPai && c.Id != categoriaViewModel.Id).FirstOrDefault().CategoriasFilhas.ToList();
            }

            CategoriaValidator validationRules = new CategoriaValidator(categorias);

            var result = validationRules.Validate(categoriaViewModel);

            if (result.IsValid)
            {
                try
                {
                    var re = await _categoriasService.Update(id, categoriaViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch 
                {
                    ViewData["mensagemError"] = "Erro ao atualizar!";
                }
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            ViewData["IdPai"] = new SelectList(await _categoriasService.GetAll(), "Id", "Id", categoriaViewModel.IdPai);
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
            var categoria = await _categoriasService.FindById(id);

            if (categoria != null)
            {

                CategoriaValidator validationRules = new CategoriaValidator();
                var result = validationRules.Validate(categoria);

                if (result.IsValid)
                {
                    try{
                        var val = await _categoriasService.Delete(id);
                    }
                    catch
                    {
                        ViewData["mensagemError"] = "Erro ao deletar!";
                    }
                }
                
                ModelState.Clear();
                result.AddToModelState(ModelState);

            }

            return RedirectToAction(nameof(Index));

        }

        private bool CategoriaExists(int id)
        {
            return _categoriasService.FindById(id).Result != null;
        }
    }
}

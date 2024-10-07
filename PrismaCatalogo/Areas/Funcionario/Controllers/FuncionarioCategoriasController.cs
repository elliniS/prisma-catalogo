using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using PrismaCatalogo.Validations;

namespace PrismaCatalogo.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class FuncionarioCategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioCategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Funcionario/Categorias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Categorias.Include(c => c.CategoriaPai);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Funcionario/Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .Include(c => c.CategoriaPai)
                .Include(c => c.CategoriasFilhas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Funcionario/Categorias/Create
        public IActionResult Create()
        {
            ViewData["IdPai"] = new SelectList(_context.Categorias, "Id", "Id");
            return View();
        }

        // POST: Funcionario/Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPai,Nome")] Categoria categoria)
        {
            IEnumerable<Categoria> categorias = await _context.Categorias.ToListAsync();

            if(categoria.IdPai != null)
            {
                categorias = categorias.Where(c => c.Id == categoria.IdPai).FirstOrDefault().CategoriasFilhas;
            }

            CategoriaValidator validationRules = new CategoriaValidator(categorias);

            var result = validationRules.Validate(categoria);

            if (result.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            ViewData["IdPai"] = new SelectList(_context.Categorias, "Id", "Id", categoria.IdPai);
            return View(categoria);
        }

        // GET: Funcionario/Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            ViewData["IdPai"] = new SelectList(_context.Categorias, "Id", "Id", categoria.IdPai);
            return View(categoria);
        }

        // POST: Funcionario/Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPai,Nome")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            IEnumerable<Categoria> categorias = await _context.Categorias.ToListAsync();

            if (categoria.IdPai != null)
            {
                categorias = categorias.Where(c => c.Id == categoria.IdPai).FirstOrDefault().CategoriasFilhas;
            }

            CategoriaValidator validationRules = new CategoriaValidator(categorias);

            var result = validationRules.Validate(categoria);

            if (result.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            ViewData["IdPai"] = new SelectList(_context.Categorias, "Id", "Id", categoria.IdPai);
            return View(categoria);
        }

        // GET: Funcionario/Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Funcionario/Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.CategoriasFilhas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria != null)
            {

                CategoriaValidator validationRules = new CategoriaValidator();
                var result = validationRules.Validate(categoria);

                if (result.IsValid)
                {
                    _context.Categorias.Remove(categoria);
                }
                else
                {
                    ModelState.Clear();
                    result.AddToModelState(ModelState);
                    return View(categoria);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}

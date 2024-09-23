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

namespace Prisma.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class FuncionarioTamanhosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioTamanhosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Funcionario/FuncionarioTamanhos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tamanhos.ToListAsync());
        }

        // GET: Funcionario/FuncionarioTamanhos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tamanho = await _context.Tamanhos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tamanho == null)
            {
                return NotFound();
            }

            return View(tamanho);
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
        public async Task<IActionResult> Create([Bind("Id,Nome")] Tamanho tamanho)
        {
            TamanhoValidator validations = new TamanhoValidator(_context.Tamanhos);
            var resul = validations.Validate(tamanho);

            if (resul.IsValid)
            {
                _context.Add(tamanho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(tamanho);
        }

        // GET: Funcionario/FuncionarioTamanhos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho == null)
            {
                return NotFound();
            }
            return View(tamanho);
        }

        // POST: Funcionario/FuncionarioTamanhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Tamanho tamanho)
        {
            if (id != tamanho.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tamanho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TamanhoExists(tamanho.Id))
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
            return View(tamanho);
        }

        // GET: Funcionario/FuncionarioTamanhos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tamanho = await _context.Tamanhos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tamanho == null)
            {
                return NotFound();
            }

            return View(tamanho);
        }

        // POST: Funcionario/FuncionarioTamanhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tamanho = await _context.Tamanhos.FindAsync(id);
            if (tamanho != null)
            {
                _context.Tamanhos.Remove(tamanho);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TamanhoExists(int id)
        {
            return _context.Tamanhos.Any(e => e.Id == id);
        }
    }
}

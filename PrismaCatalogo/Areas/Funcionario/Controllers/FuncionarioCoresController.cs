using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrismaCatalogo.Context;
using PrismaCatalogo.Models;
using PrismaCatalogo.Validations;

namespace PrismaCatalogo.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class FuncionarioCoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioCoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Funcionario/FuncionarioCores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cores.ToListAsync());
        }

        // GET: Funcionario/FuncionarioCores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cor = await _context.Cores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cor == null)
            {
                return NotFound();
            }

            return View(cor);
        }

        // GET: Funcionario/FuncionarioCores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionario/FuncionarioCores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CodigoHexadecimal,FotoBytes")] Cor cor, IFormFile? file)
        {
            CorValidator validations = new CorValidator(_context.Cores);
            var result = validations.Validate(cor);
            
            if (result.IsValid)
            {
                cor.FotoBytes = ConvertImagemTostring64(file);
                _context.Add(cor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);
            return View(cor);
        }

        public string ConvertImagemTostring64(IFormFile file)
        {
            string string64 = null;


            if (file != null)
            {
                try
                {
                    //var fileStream = file.OpenReadStream();

                    using (var fileStream = file.OpenReadStream())
                    {
                        using (var fileStreamFormat = ResizeImage(fileStream, 100, 100))
                        {
                            var bytes = new byte[fileStreamFormat.Length];

                            fileStreamFormat.Read(bytes, 0, (int)fileStreamFormat.Length);
                            string64 = Convert.ToBase64String(bytes);
                        }
                    }
                }
                catch
                {
                   
                }
            }
            
            return string64;
        }

        public Stream ResizeImage(Stream stream, int pWidth, int pHeight)
        {
            try
            {
            using (var img = new Bitmap(stream))
            {
                int oriWidth = img.Width;
                int oriHeight = img.Height;
                float percentX = (float)pWidth / (float)oriWidth;
                float percentY = (float)pHeight / (float)oriHeight;
                float percent = Math.Min(percentX, percentY);

                int width = (int)(oriWidth * percent);
                int height = (int)(oriHeight * percent);

                Bitmap newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, 0, 0, width, height);
                }

                var outputStream = new MemoryStream();
                newImage.Save(outputStream, ImageFormat.Bmp);
                outputStream.Position = 0;

                return outputStream;
                }
            }
            catch
            {
                throw new Exception("Deu ruim");
            }

        }

        // GET: Funcionario/FuncionarioCores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cor = await _context.Cores.FindAsync(id);
            if (cor == null)
            {
                return NotFound();
            }
            return View(cor);
        }

        // POST: Funcionario/FuncionarioCores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CodigoHexadecimal,FotoBytes")] Cor cor)
        {
            if (id != cor.Id)
            {
                return NotFound();
            }

            List<Cor> cores = _context.Cores.Where(c => c.Id != cor.Id).ToList();
            CorValidator validations = new CorValidator(cores);
            var result = validations.Validate(cor);

            if (result.IsValid)
            {
                try
                {
                    _context.Update(cor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorExists(cor.Id))
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

            return View(cor);
        }

        // GET: Funcionario/FuncionarioCores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cor = await _context.Cores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cor == null)
            {
                return NotFound();
            }

            return View(cor);
        }

        // POST: Funcionario/FuncionarioCores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cor = await _context.Cores.FindAsync(id);
            if (cor != null)
            {
                _context.Cores.Remove(cor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorExists(int id)
        {
            return _context.Cores.Any(e => e.Id == id);
        }
    }
}

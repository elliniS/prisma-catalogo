using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;

namespace PrismaCatalogo.Web.Web.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class FuncionarioCoresController : Controller
    {
        private ICorService _corsService;

        public FuncionarioCoresController(ICorService cor)
        {
            _corsService = cor;
        }

        // GET: Funcionario/FuncionarioCores
        public async Task<IActionResult> Index()
        {
            var cor = await _corsService.GetAll();

            if(cor == null)
            {
                cor = new List<CorViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar tamanho!";
            }

            return View(cor);
        }

        // GET: Funcionario/FuncionarioCores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var cor = await _corsService.FindById(Convert.ToInt32(id));

                if (cor == null)
                {
                    throw new Exception();
                }
                return View(cor);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao buscar cor!";
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Create([Bind("Id,Nome,CodigoHexadecimal,FotoBytes")] CorViewModel cor, IFormFile? file)
        {
            CorValidator validations = new CorValidator(await _corsService.GetAll());
            var result = validations.Validate(cor);
            
            if (result.IsValid)
            {
                try
                {
                    cor.FotoBytes = ConvertImagemTostring64(file);
                    var re = await _corsService.Create(cor);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao salvar cor!";
                }
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);
            return View(cor);
        }

        // GET: Funcionario/FuncionarioCores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                var cor = await _corsService.FindById(Convert.ToInt32(id));

                if (cor == null)
                {
                    throw new Exception();
                }
                return View(cor);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioCores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CodigoHexadecimal,FotoBytes")] CorViewModel cor, IFormFile? file)
        {
            List<CorViewModel> cores = (await _corsService.GetAll()).Where(c => c.Id != cor.Id).ToList();
            CorValidator validations = new CorValidator(cores);
            var result = validations.Validate(cor);

            if (result.IsValid)
            {
                try
                {
                    if(file != null)
                    {
                        cor.FotoBytes = ConvertImagemTostring64(file);
                    }

                    var re = await _corsService.Update(id, cor);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao atualizar!";
                }
            }

            ModelState.Clear();
            result.AddToModelState(ModelState);

            return View(cor);
        }

        // GET: Funcionario/FuncionarioCores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var cor = await _corsService.FindById(Convert.ToInt32(id));

                if (cor == null)
                {
                    return NotFound();
                }

                return View(cor);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioCores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cor = await  _corsService.FindById(id);
            if (cor != null)
            {
                try
                {
                    var re = await _corsService.Delete(id);
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao deletar!";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CorExists(int id)
        {
            return _corsService.FindById(id) != null;
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
    }
}

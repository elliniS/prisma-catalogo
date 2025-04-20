using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    [Authorize(Roles = "Funcionario")]
    [Area("Funcionario")]
    public class FuncionarioProdutosFilhosController : Controller
    {
        private IProdutoFilhoService _produtoFilhoService;
        private ICorService _corService;
        private ITamanhoService _tamanhoService;

        public FuncionarioProdutosFilhosController(IProdutoFilhoService produtoFilho, ICorService cor, ITamanhoService tamanho)
        {
            _produtoFilhoService = produtoFilho;
            _corService = cor;
            _tamanhoService = tamanho;
        }


        // GET: Funcionario/FuncionarioProdutosFilhos
        public async Task<IActionResult> Index(int produtoId)
        {
            var produtosFilhos = await _produtoFilhoService.FindByPruduto(produtoId);

            if(produtosFilhos == null)
            {
                produtosFilhos = new List<ProdutoFilhoViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar produtoFilho!";
            }

            ViewData["mensagemError"] = TempData["mensagemError"];

            //foreach (var produto in produtosFilhos)
            //{
            //    produto.FotoCapa = produto.Fotos.OrderBy(f => f.Id).LastOrDefault();
            //}

            ViewBag.ProdutoId = produtoId;

            return View(produtosFilhos);
        }

        // GET: Funcionario/FuncionarioProdutosFilhos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var produtoFilho = await _produtoFilhoService.FindById(Convert.ToInt32(id));

                if (produtoFilho == null)
                {
                    throw new Exception();
                }

                return View(produtoFilho);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao buscar produtoFilho!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Funcionario/FuncionarioProdutosFilhos/Create
        [HttpGet]
        public async Task<IActionResult> Create(int produtoId)
        {
            var coresList = await _corService.GetAll();
            var tamanhosList = await _tamanhoService.GetAll();

            ViewBag.ShowCores = new SelectList(coresList, "Id", "Nome");
            ViewBag.ShowTamanhos = new SelectList(tamanhosList, "Id", "Nome");

            var produtoFilho = new ProdutoFilhoViewModel();
            produtoFilho.ProdutoId = produtoId;

            return View(produtoFilho);

        }

        // POST: Funcionario/FuncionarioProdutosFilhos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,Nome,Preco,QuantEstoque,Descricao,Observacao,Ativo,TamanhoId,CorId")] ProdutoFilhoViewModel produtoFilho, IEnumerable<IFormFile>? files)
        {
            ProdutoFilhoValidator validations = new ProdutoFilhoValidator();
            var resul = validations.Validate(produtoFilho);

            if (resul.IsValid)
            {
                try
                {
                    produtoFilho.Fotos = GerenciaFotos(files);

                    var result = await _produtoFilhoService.Create(produtoFilho);
                    return RedirectToAction(nameof(Index), new { produtoFilho.ProdutoId});
                }
                catch(Exception e)
                {
                    ViewData["mensagemError"] = e.Message;
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(produtoFilho);
        }

        // GET: Funcionario/FuncionarioProdutosFilhos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                var produtoFilho = await _produtoFilhoService.FindById(Convert.ToInt32(id));

                if (produtoFilho == null)
                {
                    throw new Exception();                    
                }

                return View(produtoFilho);
            }
             catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioProdutosFilhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,Nome,Preco,QuantEstoque,Descricao,Observacao,Ativo,TamanhoId,CorId,Fotos")] ProdutoFilhoViewModel produtoFilhoViewModel, IEnumerable<IFormFile>? files)
        {
            ProdutoFilhoValidator validations = new ProdutoFilhoValidator();
            var resul = validations.Validate(produtoFilhoViewModel);

            if (resul.IsValid)
            {
                try
                {
                    if (produtoFilhoViewModel.Fotos == null)
                        produtoFilhoViewModel.Fotos = new List<FotoViewModel>();

                    produtoFilhoViewModel.Fotos.AddRange(GerenciaFotos(files));

                    var re = await _produtoFilhoService.Update(id, produtoFilhoViewModel);
                    return RedirectToAction(nameof(Index), new { produtoFilhoViewModel.ProdutoId });
                }
                catch(Exception e)
                {
                    ViewData["mensagemError"] = e.Message;
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(produtoFilhoViewModel);
        }

        // GET: Funcionario/FuncionarioProdutosFilhos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var produtoFilho = await _produtoFilhoService.FindById(Convert.ToInt32(id));

                if (produtoFilho == null)
                {
                    throw new Exception();
                }

                return View(produtoFilho);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioProdutosFilhos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produtoFilho = await _produtoFilhoService.FindById(id);
            if (produtoFilho != null)
            {
                try
                {
                    var re = await _produtoFilhoService.Delete(id);
                }
                catch
                {
                    TempData["mensagemError"] = "Erro ao deletar!";
                }
            }

            return RedirectToAction(nameof(Index), new { produtoFilho.ProdutoId });
        }

        private bool ProdutoFilhoExists(int id)
        {
            return _produtoFilhoService.FindById(id).Result != null;
        }

        private List<FotoViewModel> GerenciaFotos(IEnumerable<IFormFile>? files)
        {
            var produtosFotos = new List<FotoViewModel>();

            foreach (var file in files)
            {
                var foto = new FotoViewModel();
                foto.FotoBytes = ConvertImagemTostring64(file);
                foto.FgExcluir = false;

                produtosFotos.Add(foto);
            }

            return produtosFotos;
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
                        using (var fileStreamFormat = ResizeImage(fileStream, 900, 900))
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

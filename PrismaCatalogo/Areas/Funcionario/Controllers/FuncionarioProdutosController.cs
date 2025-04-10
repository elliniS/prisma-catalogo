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
    public class FuncionarioProdutosController : Controller
    {
        private IProdutoService _produtoService;
        private ICategoriaService _categoriaService;

        public FuncionarioProdutosController(IProdutoService produto, ICategoriaService categoria)
        {
            _produtoService = produto;
            _categoriaService = categoria;
        }


        // GET: Funcionario/FuncionarioProdutos
        public async Task<IActionResult> Index()
       {
            var produtos = await _produtoService.GetAll();

            if(produtos == null)
            {
                produtos = new List<ProdutoViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar produto!";
            }

            foreach(var produto in produtos)
            {
                produto.FotoCapa = produto.Fotos.OrderBy(f => f.Id).FirstOrDefault();
            }

            return View(produtos);
        }

        // GET: Funcionario/FuncionarioProdutos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var produto = await _produtoService.FindById(Convert.ToInt32(id));

                if (produto == null)
                {
                    throw new Exception();
                }

                return View(produto);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao buscar produto!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Funcionario/FuncionarioProdutos/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoriaList = await _categoriaService.GetAll();

            ViewBag.ShowCategoria = new SelectList(categoriaList, "Id", "Nome");

            return View();
        }

        // POST: Funcionario/FuncionarioProdutos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Observacao,Ativo")] ProdutoViewModel produto, IEnumerable<IFormFile>? files)
        {
            ProdutoValidator validations = new ProdutoValidator(await _produtoService.GetAll());
            var resul = validations.Validate(produto);

            if (resul.IsValid)
            {
                try
                {
                    produto.Fotos = GerenciaFotos(files);

                    var result = await _produtoService.Create(produto);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao cadastrar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(produto);
        }

        // GET: Funcionario/FuncionarioProdutos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var categoriaList = await _categoriaService.GetAll();

            ViewBag.ShowCategoria = new SelectList(categoriaList, "Id", "Nome");

            try {
                var produto = await _produtoService.FindById(Convert.ToInt32(id));

                if (produto == null)
                {
                    throw new Exception();                    
                }
                return View(produto);
            }
             catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioProdutos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Observacao,Ativo,Fotos,CategoriaId")] ProdutoViewModel produtoViewModel, IEnumerable<IFormFile>? files)
        {
            var produtos = (await _produtoService.GetAll()).Where(t => t.Id != id);
            ProdutoValidator validations = new ProdutoValidator(produtos);
            var resul = validations.Validate(produtoViewModel);

            if (resul.IsValid)
            {
                try
                {
                    if (produtoViewModel.Fotos == null)
                        produtoViewModel.Fotos = new List<FotoViewModel>();

                    produtoViewModel.Fotos.AddRange(GerenciaFotos(files));

                    var re = await _produtoService.Update(id, produtoViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao atualizar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(produtoViewModel);
        }

        // GET: Funcionario/FuncionarioProdutos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var produto = await _produtoService.FindById(Convert.ToInt32(id));

                if (produto == null)
                {
                    throw new Exception();
                }

                return View(produto);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioProdutos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _produtoService.FindById(id);
            if (produto != null)
            {
                try
                {
                    var re = await _produtoService.Delete(id);
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao deletar!";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _produtoService.FindById(id).Result != null;
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

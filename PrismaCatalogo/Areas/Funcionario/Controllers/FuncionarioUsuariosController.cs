using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Validations;
using PrismaCatalogo.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PrismaCatalogo.Web.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    [Authorize(Roles = "Funcionario")]
    public class FuncionarioUsuariosController : Controller
    {
        private IUsuarioService _usuarioService;

        public FuncionarioUsuariosController(IUsuarioService usuario)
        {
            _usuarioService = usuario;
        }

        // GET: Funcionario/FuncionarioUsuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetAll();

            if(usuarios == null)
            {
                usuarios = new List<UsuarioViewModel>();
                ViewData["mensagemError"] = "Erro ao buscar usuario!";
            }

            return View(usuarios);
        }

        // GET: Funcionario/FuncionarioUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var usuario = await _usuarioService.FindById(Convert.ToInt32(id));

                if (usuario == null)
                {
                    throw new Exception();
                }

                return View(usuario);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao buscar usuario!";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Funcionario/FuncionarioUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionario/FuncionarioUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,NomeUsuario,Senha")] UsuarioViewModel usuario)
        {
            usuario.UsuarioTipo = Enuns.EnumUsuarioTipo.Funcionario;
            UsuarioValidator validations = new UsuarioValidator(await _usuarioService.GetAll());
            var resul = validations.Validate(usuario);

            if (resul.IsValid)
            {
                try
                {
                    var result = await _usuarioService.Create(usuario);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao cadastrar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(usuario);
        }

        // GET: Funcionario/FuncionarioUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                var usuario = await _usuarioService.FindById(Convert.ToInt32(id));

                if (usuario == null)
                {
                    throw new Exception();                    
                }
                return View(usuario);
            }
             catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de edição!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NomeUsuario,Senha")] UsuarioViewModel usuarioViewModel)
        {
            var usuarios = (await _usuarioService.GetAll()).Where(t => t.Id != id);
            UsuarioValidator validations = new UsuarioValidator(usuarios);
            var resul = validations.Validate(usuarioViewModel);

            if (resul.IsValid)
            {
                try
                {
                    var re = await _usuarioService.Update(id, usuarioViewModel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao atualizar!";
                }
            }

            ModelState.Clear();
            resul.AddToModelState(ModelState);

            return View(usuarioViewModel);
        }

        // GET: Funcionario/FuncionarioUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var usuario = await _usuarioService.FindById(Convert.ToInt32(id));

                if (usuario == null)
                {
                    throw new Exception();
                }

                return View(usuario);
            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao acessar tela de delete!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Funcionario/FuncionarioUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _usuarioService.FindById(id);
            if (usuario != null)
            {
                try
                {
                    var re = await _usuarioService.Delete(id);
                }
                catch
                {
                    ViewData["mensagemError"] = "Erro ao deletar!";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _usuarioService.FindById(id).Result != null;
        }
    }
}

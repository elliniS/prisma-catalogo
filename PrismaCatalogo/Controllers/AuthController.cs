using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System;
using PrismaCatalogo.Validations;

namespace AuthFacil.Mvc.Controllers;

public class AuthController : Controller
{
    private IUsuarioService _usuarioService;

    public AuthController(IUsuarioService usuario)
    {
        _usuarioService = usuario;
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
        usuario.UsuarioTipo = PrismaCatalogo.Enuns.EnumUsuarioTipo.Cliente;
        UsuarioValidator validations = new UsuarioValidator(await _usuarioService.GetAll());
        var resul = validations.Validate(usuario);

        if (resul.IsValid)
        {
            try
            {
                var result = await _usuarioService.Create(usuario);

                return RedirectToAction(nameof(Login));

            }
            catch
            {
                ViewData["mensagemError"] = "Erro ao cadastrar!";
            }
        }

        return View();
    }


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([Bind("NomeUsuario,Senha,FgLembra,ReturnUrl")] UsuarioLoginViewModel logInModel)
    {
        var usuario = await _usuarioService.Login(logInModel);

        if (usuario == null)
        {
            return View();
        }


        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, usuario.NomeUsuario),
            new Claim(ClaimTypes.Role, usuario.UsuarioTipo.ToString()),
            new Claim("Token", usuario.Token)   
        ];
        var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        var identity = new ClaimsIdentity(claims, authScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(authScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = true //logInModel.FgLembra
            });

        if (!String.IsNullOrWhiteSpace(logInModel.ReturnUrl))
        {
            return Redirect(logInModel.ReturnUrl);
        }

        return RedirectToAction("Index","Home");
    }



    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToRoute("Auth.Login");
    }

    public IActionResult Unauthorized()
    {
        return View();
    }
}

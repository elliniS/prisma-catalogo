using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;

namespace AuthFacil.Mvc.Controllers;

public class AuthController : Controller
{
    private IUsuarioService _usuarioService;

    public AuthController(IUsuarioService usuario)
    {
        _usuarioService = usuario;
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
            new Claim(ClaimTypes.Role, usuario.Nome),
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

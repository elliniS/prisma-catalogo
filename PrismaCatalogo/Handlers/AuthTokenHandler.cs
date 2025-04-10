
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using PrismaCatalogo.Web.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace PrismaCatalogo.Web.Handlers
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthTokenHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var user = _httpContextAccessor.HttpContext?.User;
            var token = user?.FindFirst("Token")?.Value;
 
            try
            {
                await base.SendAsync(request, cancellationToken);
            }
            catch
            {

            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);


            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                var newToken = await RefreshTokenAsync();
                if (newToken != null) {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    await _httpContextAccessor.HttpContext.SignOutAsync();
                }
            }

          
            return response;
        }

        private async Task<string> RefreshTokenAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var token = user?.FindFirst("Token")?.Value;
            var refreshToken = user?.FindFirst("RefreshToken")?.Value;

            var url = _configuration["ServiceUri:Api"] + $"/api/usuario/refresh?token={Uri.EscapeDataString(token)}&refreToken={Uri.EscapeDataString(refreshToken)}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var refreshResponse = await base.SendAsync(request, CancellationToken.None);

            if (refreshResponse.IsSuccessStatusCode)
            {
                var authResult = await refreshResponse.Content.ReadFromJsonAsync<UsuarioViewModel>();

                var claimsIdentity = (ClaimsIdentity)user?.Identity;

                var claimTokenAntiga = claimsIdentity.FindFirst("Token");
                var claimRefreshAntiga = claimsIdentity.FindFirst("RefreshToken");


                if (claimTokenAntiga != null)
                {
                    claimsIdentity.RemoveClaim(claimTokenAntiga);
                }

                if (claimRefreshAntiga != null)
                {
                    claimsIdentity.RemoveClaim(claimRefreshAntiga);
                }

                claimsIdentity.AddClaim(new Claim("Token", authResult.Token));
                claimsIdentity.AddClaim(new Claim("RefreshToken", authResult.RefreshToken));

                var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                await _httpContextAccessor.HttpContext.SignInAsync(authScheme, new ClaimsPrincipal(claimsIdentity));

                return authResult?.Token;
            }
            return null;

        }
    }
}
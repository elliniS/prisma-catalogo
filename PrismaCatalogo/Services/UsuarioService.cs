using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCatalogo.Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Usuario/";
        //private UsuarioViewModel usuarioView = new UsuarioViewModel();

        public UsuarioService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAll()
        {
            IEnumerable<UsuarioViewModel> usuarios = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuarios = await JsonSerializer.DeserializeAsync<IEnumerable<UsuarioViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuarios;
            }
        }

        public async Task<UsuarioViewModel> FindById(int id)
        {
            UsuarioViewModel usuario = null;

            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }

        public async Task<UsuarioViewModel> FindByName(string name)
        {
            UsuarioViewModel usuario = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }

        public async Task<UsuarioViewModel> Create(UsuarioViewModel usuarioViewModel)
        {
            UsuarioViewModel usuario = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, usuarioViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }

        public async Task<UsuarioViewModel> Update(int id, UsuarioViewModel usuarioViewModel)
        {
            UsuarioViewModel usuario = null;


            var client = _clientFactory.CreateClient("Api");

            var json = JsonSerializer.Serialize(usuarioViewModel, _options);

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, usuarioViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }

        public async Task<UsuarioViewModel> Delete(int id)
        {
            UsuarioViewModel usuario = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }

        public async Task<UsuarioViewModel> Login(UsuarioLoginViewModel usuarioViewModel)
        {
            UsuarioViewModel usuario = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint + "Login", usuarioViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    usuario = await JsonSerializer.DeserializeAsync<UsuarioViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return usuario;
            }
        }
    }
}

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
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                return await CapituraRetorno<IEnumerable<UsuarioViewModel>>(response);
            }
        }

        public async Task<UsuarioViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                 return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }

        public async Task<UsuarioViewModel> FindByName(string name)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }

        public async Task<UsuarioViewModel> Create(UsuarioViewModel usuarioViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, usuarioViewModel))
            {
                return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }

        public async Task<UsuarioViewModel> Update(int id, UsuarioViewModel usuarioViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, usuarioViewModel))
            {
                return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }

        public async Task<UsuarioViewModel> Delete(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }

        public async Task<UsuarioViewModel> Login(UsuarioLoginViewModel usuarioViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint + "Login", usuarioViewModel))
            {
                return await CapituraRetorno<UsuarioViewModel>(response);
            }
        }


        private async Task<T> CapituraRetorno<T>(HttpResponseMessage response)
        {
            T obj;

            var apiResponse = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                obj = await JsonSerializer.DeserializeAsync<T>(apiResponse, _options);
            }
            else
            {
                var erros = await JsonSerializer.DeserializeAsync<ErrorViewModel>(apiResponse, _options);

                throw new Exception(erros.Errors != null && erros.Errors.Count() > 0 ? string.Join("\n", erros.Errors) : erros.Message);
            }
            return obj;
        }
    }
}

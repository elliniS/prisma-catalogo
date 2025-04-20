using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace PrismaCatalogo.Web.Services
{
    public class CategoriaService : ICategoriaService
    {
        
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Categoria/";
        //private CategoriaViewModel categoriaView = new CategoriaViewModel();

        public CategoriaService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public async Task<IEnumerable<CategoriaViewModel>> GetAll()
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                return await CapituraRetorno<IEnumerable<CategoriaViewModel>>(response);
            }
        }

        public async Task<CategoriaViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<CategoriaViewModel>(response);
            }
        }

        public async Task<CategoriaViewModel> FindByName(string name)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<CategoriaViewModel>(response);
            }
        }

        public async Task<CategoriaViewModel> Create(CategoriaViewModel categoriaViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, categoriaViewModel))
            {
                return await CapituraRetorno<CategoriaViewModel>(response);
            }
        }

        public async Task<CategoriaViewModel> Update(int id, CategoriaViewModel categoriaViewModel)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, categoriaViewModel))
            {
                return await CapituraRetorno<CategoriaViewModel>(response);
            }
        }

        public async Task<CategoriaViewModel> Delete(int id)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<CategoriaViewModel>(response);
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

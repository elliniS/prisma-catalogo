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
            IEnumerable<CategoriaViewModel> categorias = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categorias = await JsonSerializer.DeserializeAsync<IEnumerable<CategoriaViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categorias;
            }
        }

        public async Task<CategoriaViewModel> FindById(int id)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoria = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categoria;
            }
        }

        public async Task<CategoriaViewModel> FindByName(string name)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoria = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categoria;
            }
        }

        public async Task<CategoriaViewModel> Create(CategoriaViewModel categoriaViewModel)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, categoriaViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoria = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categoria;
            }
        }

        public async Task<CategoriaViewModel> Update(int id, CategoriaViewModel categoriaViewModel)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, categoriaViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoria = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categoria;
            }
        }

        public async Task<CategoriaViewModel> Delete(int id)
        {
            CategoriaViewModel categoria = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    categoria = await JsonSerializer.DeserializeAsync<CategoriaViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return categoria;
            }
        }
    }
}

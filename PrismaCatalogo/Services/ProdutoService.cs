using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCatalogo.Web.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Produto/";
        //private ProdutoViewModel produtoView = new ProdutoViewModel();

        public ProdutoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<IEnumerable<ProdutoViewModel>> GetAll()
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                return await CapituraRetorno<IEnumerable<ProdutoViewModel>>(response);
            }
        }

        public async Task<ProdutoViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<ProdutoViewModel>(response);
            }
        }

        public async Task<ProdutoViewModel> FindByName(string name)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<ProdutoViewModel>(response);
            }
        }

        public async Task<ProdutoViewModel> Create(ProdutoViewModel produtoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, produtoViewModel))
            {
                return await CapituraRetorno<ProdutoViewModel>(response);
            }
        }

        public async Task<ProdutoViewModel> Update(int id, ProdutoViewModel produtoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, produtoViewModel))
            {
                return await CapituraRetorno<ProdutoViewModel>(response);
            }
        }

        public async Task<ProdutoViewModel> Delete(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<ProdutoViewModel>(response);
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

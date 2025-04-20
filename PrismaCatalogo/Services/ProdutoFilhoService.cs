using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCatalogo.Web.Services
{
    public class ProdutoFilhoService : IProdutoFilhoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/ProdutoFilho/";
        //private ProdutoFilhoViewModel produtoFilhoView = new ProdutoFilhoViewModel();

        public ProdutoFilhoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<IEnumerable<ProdutoFilhoViewModel>> GetAll()
        {
            IEnumerable<ProdutoFilhoViewModel> produtoFilhos = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilhos = await JsonSerializer.DeserializeAsync<IEnumerable<ProdutoFilhoViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilhos;
            }
        }

        public async Task<IEnumerable<ProdutoFilhoViewModel>> FindByPruduto(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + "GetByProduto/" + id))
            {
                return await CapituraRetorno<IEnumerable<ProdutoFilhoViewModel>>(response);
            }
        }

        public async Task<ProdutoFilhoViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<ProdutoFilhoViewModel>(response);
            }
        }

        public async Task<ProdutoFilhoViewModel> FindByName(string name)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<ProdutoFilhoViewModel>(response);
            }
        }

        public async Task<ProdutoFilhoViewModel> Create(ProdutoFilhoViewModel produtoFilhoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, produtoFilhoViewModel))
            {
                return await CapituraRetorno<ProdutoFilhoViewModel>(response);
            }
        }

        public async Task<ProdutoFilhoViewModel> Update(int id, ProdutoFilhoViewModel produtoFilhoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, produtoFilhoViewModel))
            {
                return await CapituraRetorno<ProdutoFilhoViewModel>(response);
            }
        }

        public async Task<ProdutoFilhoViewModel> Delete(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<ProdutoFilhoViewModel>(response);
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

                throw new Exception(erros.Message);
            }
            return obj;
        }
    }
}

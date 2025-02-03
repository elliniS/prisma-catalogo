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
            IEnumerable<ProdutoFilhoViewModel> produtoFilhos = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + "GetByProduto/" + id))
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

        public async Task<ProdutoFilhoViewModel> FindById(int id)
        {
            ProdutoFilhoViewModel produtoFilho = null;

            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilho = await JsonSerializer.DeserializeAsync<ProdutoFilhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilho;
            }
        }

        public async Task<ProdutoFilhoViewModel> FindByName(string name)
        {
            ProdutoFilhoViewModel produtoFilho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilho = await JsonSerializer.DeserializeAsync<ProdutoFilhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilho;
            }
        }

        public async Task<ProdutoFilhoViewModel> Create(ProdutoFilhoViewModel produtoFilhoViewModel)
        {
            ProdutoFilhoViewModel produtoFilho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, produtoFilhoViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilho = await JsonSerializer.DeserializeAsync<ProdutoFilhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilho;
            }
        }

        public async Task<ProdutoFilhoViewModel> Update(int id, ProdutoFilhoViewModel produtoFilhoViewModel)
        {
            ProdutoFilhoViewModel produtoFilho = null;


            var client = _clientFactory.CreateClient("Api");

            var json = JsonSerializer.Serialize(produtoFilhoViewModel, _options);

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, produtoFilhoViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilho = await JsonSerializer.DeserializeAsync<ProdutoFilhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilho;
            }
        }

        public async Task<ProdutoFilhoViewModel> Delete(int id)
        {
            ProdutoFilhoViewModel produtoFilho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    produtoFilho = await JsonSerializer.DeserializeAsync<ProdutoFilhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return produtoFilho;
            }
        }
    }
}

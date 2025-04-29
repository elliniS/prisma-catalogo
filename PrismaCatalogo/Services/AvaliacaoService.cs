using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;

namespace PrismaCatalogo.Web.Services
{
    public class AvaliacaoService : IAvaliacaoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Avaliacao/";
        //private AvaliacaoViewModel avaliacaoView = new AvaliacaoViewModel();

        public AvaliacaoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<AvaliacaoViewModel>> FindByProduto(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + "GetByProduto/" + id))
            {
                return await CapituraRetorno<IEnumerable<AvaliacaoViewModel>>(response);
            }
        }

        public async Task<AvaliacaoViewModel> FindByProdutoUsuario(int produtoId, int usuarioId)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + "GetByProduto/" + produtoId + "/Usuario/" + usuarioId))
            {
                return await CapituraRetorno<AvaliacaoViewModel>(response);
            }
        }

        public async Task<AvaliacaoViewModel> Create(AvaliacaoViewModel avaliacaoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, avaliacaoViewModel))
            {
                return await CapituraRetorno<AvaliacaoViewModel>(response);
            }
        }

        public async Task<AvaliacaoViewModel> Update(int id, AvaliacaoViewModel avaliacaoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, avaliacaoViewModel))
            {
                return await CapituraRetorno<AvaliacaoViewModel>(response);
            }
        }

        public async Task<AvaliacaoViewModel> Delete(int id)
        {
            AvaliacaoViewModel avaliacao = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<AvaliacaoViewModel>(response);
            }
        }

        private async Task<T> CapituraRetorno<T>(HttpResponseMessage response)
        {
            T obj;

            var apiResponse = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }
                else { 
                    obj = await JsonSerializer.DeserializeAsync<T>(apiResponse, _options);
                }
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

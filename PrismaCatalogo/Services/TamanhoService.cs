using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PrismaCatalogo.Web.Services
{
    public class TamanhoService : ITamanhoService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Tamanho/";
        //private TamanhoViewModel tamanhoView = new TamanhoViewModel();

        public TamanhoService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<IEnumerable<TamanhoViewModel>> GetAll()
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                return await CapituraRetorno<IEnumerable<TamanhoViewModel>>(response);
            }
        }

        public async Task<TamanhoViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<TamanhoViewModel>(response);
            }
        }

        public async Task<TamanhoViewModel> FindByName(string name)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<TamanhoViewModel>(response);
            }
        }

        public async Task<TamanhoViewModel> Create(TamanhoViewModel tamanhoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, tamanhoViewModel))
            {
                return await CapituraRetorno<TamanhoViewModel>(response);
            }
        }

        public async Task<TamanhoViewModel> Update(int id, TamanhoViewModel tamanhoViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            var json = JsonSerializer.Serialize(tamanhoViewModel, _options);

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, tamanhoViewModel))
            {
                return await CapituraRetorno<TamanhoViewModel>(response);
            }
        }

        public async Task<TamanhoViewModel> Delete(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<TamanhoViewModel>(response);
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

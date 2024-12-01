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
            IEnumerable<TamanhoViewModel> tamanhos = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanhos = await JsonSerializer.DeserializeAsync<IEnumerable<TamanhoViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanhos;
            }
        }

        public async Task<TamanhoViewModel> FindById(int id)
        {
            TamanhoViewModel tamanho = null;

            var client = _clientFactory.CreateClient("Api");

            using ( var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanho = await JsonSerializer.DeserializeAsync<TamanhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanho;
            }
        }

        public async Task<TamanhoViewModel> FindByName(string name)
        {
            TamanhoViewModel tamanho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanho = await JsonSerializer.DeserializeAsync<TamanhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanho;
            }
        }

        public async Task<TamanhoViewModel> Create(TamanhoViewModel tamanhoViewModel)
        {
            TamanhoViewModel tamanho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, tamanhoViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanho = await JsonSerializer.DeserializeAsync<TamanhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanho;
            }
        }

        public async Task<TamanhoViewModel> Update(int id, TamanhoViewModel tamanhoViewModel)
        {
            TamanhoViewModel tamanho = null;


            var client = _clientFactory.CreateClient("Api");

            var json = JsonSerializer.Serialize(tamanhoViewModel, _options);

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, tamanhoViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanho = await JsonSerializer.DeserializeAsync<TamanhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanho;
            }
        }

        public async Task<TamanhoViewModel> Delete(int id)
        {
            TamanhoViewModel tamanho = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    tamanho = await JsonSerializer.DeserializeAsync<TamanhoViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return tamanho;
            }
        }
    }
}

using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Xml.Linq;

namespace PrismaCatalogo.Web.Services
{
    public class CorService : ICorService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Cor/";
        //private CorViewModel corView = new CorViewModel();

        public CorService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public async Task<IEnumerable<CorViewModel>> GetAll()
        {
            IEnumerable<CorViewModel> cors = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cors = await JsonSerializer.DeserializeAsync<IEnumerable<CorViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cors;
            }
        }

        public async Task<CorViewModel> FindById(int id)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cor = await JsonSerializer.DeserializeAsync<CorViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cor;
            }
        }

        public async Task<CorViewModel> FindByName(string name)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cor = await JsonSerializer.DeserializeAsync<CorViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cor;
            }
        }

        public async Task<CorViewModel> Create(CorViewModel corViewModel)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, corViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cor = await JsonSerializer.DeserializeAsync<CorViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cor;
            }
        }

        public async Task<CorViewModel> Update(int id, CorViewModel corViewModel)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, corViewModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cor = await JsonSerializer.DeserializeAsync<CorViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cor;
            }
        }

        public async Task<CorViewModel> Delete(int id)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    cor = await JsonSerializer.DeserializeAsync<CorViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
                return cor;
            }
        }
    }
}

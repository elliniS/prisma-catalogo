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
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint))
            {
                return await CapituraRetorno<IEnumerable<CorViewModel>>(response);
            }
        }

        public async Task<CorViewModel> FindById(int id)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<CorViewModel>(response);
            }
        }

        public async Task<CorViewModel> FindByName(string name)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.GetAsync(apiEndpoint + name))
            {
                return await CapituraRetorno<CorViewModel>(response);
            }
        }

        public async Task<CorViewModel> Create(CorViewModel corViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, corViewModel))
            {
                return await CapituraRetorno<CorViewModel>(response);
            }
        }

        public async Task<CorViewModel> Update(int id, CorViewModel corViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PutAsJsonAsync(apiEndpoint + id, corViewModel))
            {
                return await CapituraRetorno<CorViewModel>(response);
            }
        }

        public async Task<CorViewModel> Delete(int id)
        {
            CorViewModel cor = null;

            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.DeleteAsync(apiEndpoint + id))
            {
                return await CapituraRetorno<CorViewModel>(response);
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

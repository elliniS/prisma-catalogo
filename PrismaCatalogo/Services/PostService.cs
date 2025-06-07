using PrismaCatalogo.Web.Models;
using PrismaCatalogo.Web.Services.Interfaces;
using System.Text.Json;
using System.Xml.Linq;

namespace PrismaCatalogo.Web.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private const string apiEndpoint = "/api/Post/";

        public PostService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<PostViewModel> Create(PostViewModel postViewModel)
        {
            var client = _clientFactory.CreateClient("Api");

            using (var response = await client.PostAsJsonAsync(apiEndpoint, postViewModel))
            {
                return await CapituraRetorno<PostViewModel>(response);
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

        public Task<IEnumerable<PostViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<PostViewModel> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PostViewModel> FindByName(string name)
        {
            throw new NotImplementedException();
        }


        public Task<PostViewModel> Update(int id, PostViewModel t)
        {
            throw new NotImplementedException();
        }

        public Task<PostViewModel> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

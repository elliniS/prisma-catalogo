using Newtonsoft.Json;
using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PrismaCatalogo.Api.Services
{

    public class PostService : IPostService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions? _options;
        private string _n8nUrl;

        public PostService(IHttpClientFactory clientFactory) {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<string> PublicAsync(Post post)
        {
            var json = JsonConvert.SerializeObject(post);

            string resul;
            var client = _clientFactory.CreateClient("n8n_media");

            using (var response = await client.PostAsJsonAsync("", post))
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    resul = "Envio realizado com susseco";
                }
                else
                {
                    throw new Exception("Erro ao envia Post");
                }
            }

            return resul;
        }
    }
}

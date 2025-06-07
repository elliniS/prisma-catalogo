using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface IPostService
    {
        Task<string> PublicAsync(Post post); 
    }
}

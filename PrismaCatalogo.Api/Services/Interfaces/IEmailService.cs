using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task<string> SendEmail(Email email);
    }
}

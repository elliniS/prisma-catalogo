using CloudinaryDotNet.Actions;

namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface IImagemService
    {
        ImageUploadResult Upload(string baseb4);
        DeletionResult Delete(string url);
    }
}

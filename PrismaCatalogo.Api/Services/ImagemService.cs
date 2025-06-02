using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.Extensions.Options;
using PrismaCatalogo.Api.Repositories.Interfaces;
using PrismaCatalogo.Api.Services.Interfaces;

namespace PrismaCatalogo.Api.Services
{
    public class ImagemService : IImagemService
    {
        readonly Cloudinary _cloudinary;

        public ImagemService()
        {
            DotEnv.Load();
            _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            _cloudinary.Api.Secure = true;
        }

        public ImageUploadResult Upload(string baseb4)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription($"data:image/jpeg;base64,{baseb4}")
               //File = new FileDescription(@"C:\Users\faculdade\Pictures\Img Projeto prisma\Mochila-branca.jpeg")
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult;
        }

        public DeletionResult Delete(string nome)
        {
            var deleteParams = new DeletionParams(nome);
            var result = _cloudinary.Destroy(deleteParams);

            return result;
        }
    }
}

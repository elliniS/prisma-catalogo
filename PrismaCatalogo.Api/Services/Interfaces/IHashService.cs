namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface IHashService
    {
        string GeraHash(string valor);
        bool ConparaValor(string valor, string hash);
    }
}

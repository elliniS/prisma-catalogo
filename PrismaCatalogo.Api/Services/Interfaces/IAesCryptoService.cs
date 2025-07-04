namespace PrismaCatalogo.Api.Services.Interfaces
{
    public interface IAesCryptoService
    {
        byte[] HexStringToBytes(string valor);
        string Encrypt(string valor, byte[] chave);
        string Decrypt(string valor, byte[] chave);
    }
}

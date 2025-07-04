using PrismaCatalogo.Api.Models;
using PrismaCatalogo.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PrismaCatalogo.Api.Services
{
    public class HashService : IHashService
    {
        public string GeraHash(string valor)
        {
            return BCrypt.Net.BCrypt.HashPassword(valor, workFactor: 12);
        }

        public bool ConparaValor(string valor, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(valor, hash);
        }
    }
}

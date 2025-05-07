using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PropertyManager.API.Security.Common.Interface;
using PropertyManager.API.Security.Domain;

namespace PropertyManager.API.Security.Common.Implementation
{
    public class ServiceHash : IServiceHash
    {
        public ResultHash Hash(string input)
        {
            var sal = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }

            return Hash(input, sal);
        }

        public ResultHash Hash(string input, byte[] sal)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: sal,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10_000,
                numBytesRequested: 256 / 8
                ));

            return new ResultHash
            {
                Hash = hashed,
                Sal = sal
            };
        }
    }
}

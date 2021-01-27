using Decryptor.Interfaces;
using DevOne.Security.Cryptography.BCrypt;
using System;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class BCryptHash : IHash
    {
        private readonly int _workFactor;

        public BCryptHash(int workFactor)
        {
            _workFactor = workFactor;
        }

        public Task<bool> CheckHashAsync(string clearText, string hash)
        {
            try
            {
                return Task.Run(() => BCryptHelper.CheckPassword(clearText, hash));
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<string> GetHashAsync(string clearText)
        {
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            return Task.Run(() => BCryptHelper.HashPassword(clearText, salt));
        }
    }
}
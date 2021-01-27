using Decryptor.Interfaces;
using Scrypt;
using System;
using System.Threading.Tasks;

namespace Decryptor.Utilities.Hashing
{
    public class ScryptHash : IHash
    {
        private readonly int _scryptIterations;
        private readonly int _blockCount;
        private readonly int _threadCount;

        public ScryptHash(int scryptIterations, int blockCount, int threadCount)
        {
            _scryptIterations = scryptIterations;
            _blockCount = blockCount;
            _threadCount = threadCount;
        }

        public Task<bool> CheckHashAsync(string clearText, string hash)
        {
            ScryptEncoder encoder = new ScryptEncoder(_scryptIterations, _blockCount, _threadCount);
            try
            {
                return Task.Run(() => encoder.Compare(clearText, hash));
            }
            catch (ArgumentException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<string> GetHashAsync(string clearText)
        {
            ScryptEncoder encoder = new ScryptEncoder(_scryptIterations, _blockCount, _threadCount);
            return Task.Run(() => encoder.Encode(clearText));
        }
    }
}
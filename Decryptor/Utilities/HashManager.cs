using DevOne.Security.Cryptography.BCrypt;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Decryptor.Utilities
{
    public class HashManager
    {
        private readonly HashAlgorithm _algorithm;
        private readonly int _workFactor;
        private readonly int _degreesOfParallelism;
        private readonly int _iterations;
        private readonly int _memorySize;

        //public HashManager(HashAlgorithm algorithm)
        //{
        //    _algorithm = algorithm;

        //    _workFactor = Properties.Settings.Default.WorkFactor;
        //    _degreesOfParallelism = Properties.Settings.Default.DegreesOfParallelism;
        //    _iterations = Properties.Settings.Default.Iterations;
        //    _memorySize = Properties.Settings.Default.MemorySize;
        //}

        public HashManager(HashAlgorithm algorithm, int workFactor, int degrees, int iterations, int memorySize)
        {
            _algorithm = algorithm;
            _workFactor = workFactor;
            _degreesOfParallelism = degrees;
            _iterations = iterations;
            _memorySize = memorySize;
        }

        public static HashManager NewHashManager(HashAlgorithm algorithm)
        {
            return new HashManager(algorithm,
                                   Properties.Settings.Default.WorkFactor,
                                   Properties.Settings.Default.DegreesOfParallelism,
                                   Properties.Settings.Default.Iterations,
                                   Properties.Settings.Default.MemorySize);
        }

        public string GetHash(string input)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => GetBCryptHash(input),
                HashAlgorithm.None => throw new NotImplementedException(),
                HashAlgorithm.Scrypt => throw new NotImplementedException(),
                HashAlgorithm.Argon2 => throw new NotImplementedException(),
                HashAlgorithm.MD5 => throw new NotImplementedException(),
                HashAlgorithm.SHA1 => throw new NotImplementedException(),
                HashAlgorithm.SHA256 => throw new NotImplementedException(),
                HashAlgorithm.SHA512 => throw new NotImplementedException(),
                _ => throw new InvalidOperationException($"{_algorithm} is not a valid algorithm"),
            };
        }

        public bool CheckHash(string input, string hash)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => CheckBCryptHash(input, hash),
                HashAlgorithm.None => throw new NotImplementedException(),
                HashAlgorithm.Scrypt => throw new NotImplementedException(),
                HashAlgorithm.Argon2 => throw new NotImplementedException(),
                HashAlgorithm.MD5 => throw new NotImplementedException(),
                HashAlgorithm.SHA1 => throw new NotImplementedException(),
                HashAlgorithm.SHA256 => throw new NotImplementedException(),
                HashAlgorithm.SHA512 => throw new NotImplementedException(),
                _ => throw new InvalidOperationException($"{_algorithm} is not a valid algorithm"),
            };
        }

        private string GetBCryptHash(string input)
        {
            string salt = BCryptHelper.GenerateSalt(_workFactor);
            return BCryptHelper.HashPassword(input, salt);
        }

        private bool CheckBCryptHash(string input, string hash)
        {
            try
            {
                return BCryptHelper.CheckPassword(input, hash);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}

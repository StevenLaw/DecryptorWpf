using DevOne.Security.Cryptography.BCrypt;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;

namespace Decryptor.Utilities
{
    public class HashManager
    {
        private readonly HashAlgorithm _algorithm;
        private readonly int _workFactor;
        private readonly int _scryptIterations;
        private readonly int _blockCount;
        private readonly int _threadCount;
        private readonly int _degreesOfParallelism;
        private readonly int _argon2Iterations;
        private readonly int _memorySize;

        //public HashManager(HashAlgorithm algorithm)
        //{
        //    _algorithm = algorithm;

        //    _workFactor = Properties.Settings.Default.WorkFactor;
        //    _degreesOfParallelism = Properties.Settings.Default.DegreesOfParallelism;
        //    _iterations = Properties.Settings.Default.Iterations;
        //    _memorySize = Properties.Settings.Default.MemorySize;
        //}

        public HashManager(HashAlgorithm algorithm, int workFactor, int scryptIterations, int blockCount, int threadCount, int degrees, int argon2Iterations, int memorySize)
        {
            _algorithm = algorithm;
            _workFactor = workFactor;
            _scryptIterations = scryptIterations;
            _blockCount = blockCount;
            _threadCount = threadCount;
            _degreesOfParallelism = degrees;
            _argon2Iterations = argon2Iterations;
            _memorySize = memorySize;
        }

        public static HashManager NewHashManager(HashAlgorithm algorithm)
        {
            return new HashManager(algorithm,
                                   Properties.Settings.Default.BCryptWorkFactor,
                                   Properties.Settings.Default.ScryptIterationCount,
                                   Properties.Settings.Default.ScryptBlockCount,
                                   Properties.Settings.Default.ScryptThreadCount,
                                   Properties.Settings.Default.Argon2DegreesOfParallelism,
                                   Properties.Settings.Default.Argon2Iterations,
                                   Properties.Settings.Default.Argon2MemorySize);
        }

        public string GetHash(string input)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => GetBCryptHash(input),
                HashAlgorithm.None => throw new NotImplementedException(),
                HashAlgorithm.Scrypt => GetScryptHash(input),
                HashAlgorithm.Argon2 => throw new NotImplementedException(),
                HashAlgorithm.MD5 => throw new NotImplementedException(),
                HashAlgorithm.SHA1 => throw new NotImplementedException(),
                HashAlgorithm.SHA256 => throw new NotImplementedException(),
                HashAlgorithm.SHA512 => throw new NotImplementedException(),
                _ => throw new InvalidOperationException($"{_algorithm} is not a valid algorithm"),
            };
        }

        public bool CheckHash(string clearText, string hash)
        {
            return _algorithm switch
            {
                HashAlgorithm.BCrypt => CheckBCryptHash(clearText, hash),
                HashAlgorithm.None => throw new NotImplementedException(),
                HashAlgorithm.Scrypt => CheckScryptHash(clearText, hash),
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

        private bool CheckBCryptHash(string clearText, string hash)
        {
            try
            {
                return BCryptHelper.CheckPassword(clearText, hash);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private string GetScryptHash(string input)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            return encoder.Encode(input);
        }

        private bool CheckScryptHash(string clearText, string hash)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            try
            {
                return encoder.Compare(clearText, hash);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}

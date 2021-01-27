using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Interfaces
{
    public interface IHash
    {
        Task<bool> CheckHashAsync(string clearText, string hash);
        Task<string> GetHashAsync(string clearText);
    }
}

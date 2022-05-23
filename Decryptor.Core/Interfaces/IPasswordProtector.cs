using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decryptor.Core.Interfaces
{
    public interface IPasswordProtector
    {
        string DecryptString(string encryptedText);
        string EncryptString(string clearText);
    }
}

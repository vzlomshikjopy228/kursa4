using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace приложение.Services
{
    public interface IHashingService
    {
        byte[] HashPassword(string password);
        bool VerifyPassword(byte[] hashedPasswordFromDb, byte[] hashedPasswordToCheck);
    }
}

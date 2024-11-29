using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace приложение.Services
{
    public class HashingService : IHashingService
    {
        public byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyPassword(byte[] hashedPasswordFromDb, byte[] hashedPasswordToCheck)
        {
            return hashedPasswordFromDb.SequenceEqual(hashedPasswordToCheck);
        }
    }
}

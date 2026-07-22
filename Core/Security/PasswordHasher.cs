using System;
using System.Linq;
using System.Security.Cryptography;

namespace Core.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public static string Hash(string password)
        {
            using (var rfc = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] salt = rfc.Salt;
                byte[] hash = rfc.GetBytes(HashSize);

                // "salt:hash" formatında, ikisi de Base64 - tek bir string olarak saklanır
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        public static bool Verify(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            using (var rfc = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] actualHash = rfc.GetBytes(HashSize);
                return actualHash.Length == expectedHash.Length &&
                        actualHash.SequenceEqual(expectedHash);   
            }
        }
    }
}
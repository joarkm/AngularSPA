using System;
using System.Security.Cryptography;

namespace AngularSPA.Helpers
{

    public interface IPasswordHasher
    {
        string CreatePasswordHash(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        /* Implementation from https://stackoverflow.com/a/10402129/7949327 */

        public string CreatePasswordHash(string password)
        {
            // Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            
            // Apply PBKDF2 to the password with the provided salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations: 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and the output of PBKDF2 (the hashed password)
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Return the base64 encoded variant of the combined salt + hash
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {

            byte[] hashBytes = Convert.FromBase64String(hash);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] computedHash = pbkdf2.GetBytes(20);

            /* Compare the results */
            var hashOffset = 16;
            for (var i = 0; i < 20; i++)
                if (hashBytes[hashOffset + i] != computedHash[i])
                    return false;
            return true;
        }
    }
}

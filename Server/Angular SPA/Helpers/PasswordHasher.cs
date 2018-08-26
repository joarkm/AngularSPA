using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularSPA.Helpers
{

    public interface IPasswordHasher
    {
        string CreatePasswordHash(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string CreatePasswordHash(string password)
        {
            throw new NotImplementedException();
        }

        public bool VerifyPassword(string password, string hash)
        {
            throw new NotImplementedException();
        }
    }
}

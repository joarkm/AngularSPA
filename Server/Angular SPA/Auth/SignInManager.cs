using AngularSPA.Auth.Helpers;
using AngularSPA.Helpers;
using AngularSPA.Models;
using System;

namespace AngularSPA.Auth
{
    public interface ISignInManager
    {
        SignInResult TrySignInWithPassword(User user, string password);
    }

    public class SignInManager : ISignInManager
    {
        private readonly IPasswordHasher _passwordHasher;

        public SignInManager(
            IPasswordHasher passwordHasher
        )
        {
            _passwordHasher = passwordHasher;
        }

        public SignInResult TrySignInWithPassword(User user, string password)
        {
            if(user.Password == null)
                throw new ArgumentNullException(nameof(user));
            var hash = user.Password.Hash;
            return !_passwordHasher.VerifyPassword(password, hash) ? SignInResult.Failed() : SignInResult.Success;
        }
    }
}

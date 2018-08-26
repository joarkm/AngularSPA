using Microsoft.AspNetCore.Identity;
using System;

namespace AngularSPA.Models
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public Guid UserLoginId { get; set; }
        public override Guid UserId { get; set; }
        public override string LoginProvider { get; set; }
        public override string ProviderKey { get; set; }
        public override string ProviderDisplayName { get; set; }
        public string Discriminator { get; set; }

        public User User { get; set; }
    }
}


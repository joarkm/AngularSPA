using Microsoft.AspNetCore.Identity;
using System;

namespace AngularSPA.Models
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public Guid UserTokenId { get; set; }
        public override Guid UserId { get; set; }
        public override string Name { get; set; }
        public override string Value { get; set; }
        public string Discriminator { get; set; }
        public override string LoginProvider { get; set; }

        public User User { get; set; }
    }
}
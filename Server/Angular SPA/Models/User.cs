using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AngularSPA.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            UserLogin = new HashSet<UserLogin>();
            UserToken = new HashSet<UserToken>();
        }

        public Guid UserId { get; set; }
        public Guid? PasswordId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? RoleId { get; set; }
        
        public Password Password { get; set; }
        public Person Person { get; set; }
        public Role Role { get; set; }
        public ICollection<UserLogin> UserLogin { get; set; }
        public ICollection<UserToken> UserToken { get; set; }
    }
}

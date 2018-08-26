using System;
using System.Collections.Generic;

namespace AngularSPA.Models
{
    public class Password
    {
        public Password()
        {
            User = new HashSet<User>();
        }

        public Guid PasswordId { get; set; }
        public string Hash { get; set; }

        public ICollection<User> User { get; set; }
    }
}

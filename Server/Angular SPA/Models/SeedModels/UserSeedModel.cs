using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularSPA.Models.SeedModels
{
    public class UserSeedModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public PersonSeedModel Person { get; set; }
    }

    public class PersonSeedModel
    {
        public string GivenName { get; set; }
        public string SurName { get; set; }
    }
}

using Newtonsoft.Json;

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

    public class ClaimSeedModel
    {
        //[JsonProperty("Type")]
        public string ClaimType { get; set; }

        //[JsonProperty("Value")]
        public string ClaimValue { get; set; }
    }
}

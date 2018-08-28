using Newtonsoft.Json;

namespace AngularSPA.Models.SeedModels
{
    public class RoleSeedModel
    {
        //[JsonProperty("Name")]
        public string RoleName { get; set; }

        //[JsonProperty("Description")]
        public string RoleDescription { get; set; }
        
        public ClaimSeedModel[] Claim { get; set; }
    }
}

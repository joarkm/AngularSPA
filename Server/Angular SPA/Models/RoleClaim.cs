using System;

namespace AngularSPA.Models
{
    public class RoleClaim
    {
        public Guid RoleClaimId { get; set; }
        public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }

        public Claim Claim { get; set; }
        public Role Role { get; set; }
    }
}

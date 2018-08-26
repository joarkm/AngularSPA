using System;
using System.Collections.Generic;

namespace AngularSPA.Models
{
    public class ClaimType
    {
        public ClaimType()
        {
            Claim = new HashSet<Claim>();
        }

        public Guid ClaimTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Claim> Claim { get; set; }
    }
}

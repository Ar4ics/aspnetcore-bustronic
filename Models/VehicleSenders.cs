using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleSenders
    {
        public VehicleSenders()
        {
            UnverifiedVehicles = new HashSet<UnverifiedVehicles>();
            VerifiedVehicles = new HashSet<VerifiedVehicles>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public int CountCorrect { get; set; }
        public int CountIncorrect { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Mark { get; set; }

        public virtual ICollection<UnverifiedVehicles> UnverifiedVehicles { get; set; }
        public virtual ICollection<VerifiedVehicles> VerifiedVehicles { get; set; }
    }
}

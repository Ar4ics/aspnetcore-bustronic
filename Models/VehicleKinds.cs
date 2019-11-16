using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleKinds
    {
        public VehicleKinds()
        {
            UnverifiedVehicles = new HashSet<UnverifiedVehicles>();
            Vehicles = new HashSet<Vehicles>();
            VerifiedVehicles = new HashSet<VerifiedVehicles>();
        }

        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual VehicleTypes VehicleType { get; set; }
        public virtual ICollection<UnverifiedVehicles> UnverifiedVehicles { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
        public virtual ICollection<VerifiedVehicles> VerifiedVehicles { get; set; }
    }
}

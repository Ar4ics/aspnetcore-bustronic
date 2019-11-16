using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleTypes
    {
        public VehicleTypes()
        {
            Routes = new HashSet<Routes>();
            StationPlatformVehicleTypes = new HashSet<StationPlatformVehicleTypes>();
            TestLocationStats = new HashSet<TestLocationStats>();
            TestSpeedStats = new HashSet<TestSpeedStats>();
            VehicleKinds = new HashSet<VehicleKinds>();
            Vehicles = new HashSet<Vehicles>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int Color { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Routes> Routes { get; set; }
        public virtual ICollection<StationPlatformVehicleTypes> StationPlatformVehicleTypes { get; set; }
        public virtual ICollection<TestLocationStats> TestLocationStats { get; set; }
        public virtual ICollection<TestSpeedStats> TestSpeedStats { get; set; }
        public virtual ICollection<VehicleKinds> VehicleKinds { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}

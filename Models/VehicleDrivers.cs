using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleDrivers
    {
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int Id { get; set; }

        public virtual Drivers Driver { get; set; }
        public virtual Vehicles Vehicle { get; set; }
    }
}

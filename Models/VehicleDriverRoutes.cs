using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleDriverRoutes
    {
        public int VehicleId { get; set; }
        public int DriverRouteId { get; set; }
        public int Id { get; set; }

        public virtual DriverRoutes DriverRoute { get; set; }
        public virtual Vehicles Vehicle { get; set; }
    }
}

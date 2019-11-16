using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class DriverRoutes
    {
        public DriverRoutes()
        {
            DriverCompetitors = new HashSet<DriverCompetitors>();
            VehicleDriverRoutes = new HashSet<VehicleDriverRoutes>();
        }

        public int Id { get; set; }
        public int DriverId { get; set; }
        public int RouteId { get; set; }

        public virtual Drivers Driver { get; set; }
        public virtual Routes Route { get; set; }
        public virtual ICollection<DriverCompetitors> DriverCompetitors { get; set; }
        public virtual ICollection<VehicleDriverRoutes> VehicleDriverRoutes { get; set; }
    }
}

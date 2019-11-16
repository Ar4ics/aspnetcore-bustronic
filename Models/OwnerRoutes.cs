using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class OwnerRoutes
    {
        public OwnerRoutes()
        {
            VehicleOwnerRoutes = new HashSet<VehicleOwnerRoutes>();
        }

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int RouteId { get; set; }

        public virtual Owners Owner { get; set; }
        public virtual Routes Route { get; set; }
        public virtual ICollection<VehicleOwnerRoutes> VehicleOwnerRoutes { get; set; }
    }
}

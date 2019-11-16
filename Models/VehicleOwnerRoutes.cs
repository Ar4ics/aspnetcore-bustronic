using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VehicleOwnerRoutes
    {
        public int VehicleId { get; set; }
        public int OwnerRouteId { get; set; }
        public int Id { get; set; }

        public virtual OwnerRoutes OwnerRoute { get; set; }
        public virtual Vehicles Vehicle { get; set; }
    }
}

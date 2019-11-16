using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class StationPlatformVehicleTypes
    {
        public int StationPlatformId { get; set; }
        public int VehicleTypeId { get; set; }
        public int Id { get; set; }

        public virtual StationPlatforms StationPlatform { get; set; }
        public virtual VehicleTypes VehicleType { get; set; }
    }
}

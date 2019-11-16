﻿using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class VerifiedVehicles
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int RouteId { get; set; }
        public string Gosnomer { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int VehicleKindId { get; set; }
        public int VehicleSenderId { get; set; }

        public virtual Cities City { get; set; }
        public virtual Routes Route { get; set; }
        public virtual VehicleKinds VehicleKind { get; set; }
        public virtual VehicleSenders VehicleSender { get; set; }
    }
}

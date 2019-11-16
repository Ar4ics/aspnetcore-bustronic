using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class TestSpeedStats
    {
        public int VehicleTypeId { get; set; }
        public int RouteSegmentId { get; set; }
        public int AverageSpeed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Id { get; set; }

        public virtual RouteSegments RouteSegment { get; set; }
        public virtual VehicleTypes VehicleType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class TestLocationStats
    {
        public int UniqueId { get; set; }
        public int VehicleTypeId { get; set; }
        public int RouteSegmentId { get; set; }
        public int AverageSpeed { get; set; }
        public Point Real { get; set; }
        public Point Normalized { get; set; }
        public DateTime ObtainedAt { get; set; }
        public int Id { get; set; }

        public virtual RouteSegments RouteSegment { get; set; }
        public virtual VehicleTypes VehicleType { get; set; }
    }
}

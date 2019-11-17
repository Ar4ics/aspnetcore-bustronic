using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class MovingVehicles
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? DriverId { get; set; }
        public int RouteId { get; set; }
        public int SegmentId { get; set; }
        public string SourceType { get; set; }
        public int? AverageSpeed { get; set; }
        public Point Real { get; set; }
        public Point Normalized { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string VehicleFromTable { get; set; }
        public bool? IsOnline { get; set; }
        public int? RouteSegmentId { get; set; }
        public int? LastPathPosition { get; set; }
        public double? DistanceFromLastPath { get; set; }
        public int? Relevance { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public virtual Routes Route { get; set; }
        public virtual RouteSegments RouteSegment { get; set; }
        public virtual Vehicles Vehicle { get; set; }
    }
}

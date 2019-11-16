using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class RouteSegments
    {
        public RouteSegments()
        {
            Competitors = new HashSet<Competitors>();
            DriverCompetitors = new HashSet<DriverCompetitors>();
            MovingVehicles = new HashSet<MovingVehicles>();
            RouteSegmentPaths = new HashSet<RouteSegmentPaths>();
            RouteSegmentSpeeds = new HashSet<RouteSegmentSpeeds>();
            TestLocationStats = new HashSet<TestLocationStats>();
            TestSpeedStats = new HashSet<TestSpeedStats>();
        }

        public int Id { get; set; }
        public int RouteId { get; set; }
        public int SegmentId { get; set; }
        public int Position { get; set; }
        public LineString Path { get; set; }
        public int? Distance { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsReverse { get; set; }
        public int? SegmentPathId { get; set; }

        public virtual Routes Route { get; set; }
        public virtual Segments Segment { get; set; }
        public virtual ICollection<Competitors> Competitors { get; set; }
        public virtual ICollection<DriverCompetitors> DriverCompetitors { get; set; }
        public virtual ICollection<MovingVehicles> MovingVehicles { get; set; }
        public virtual ICollection<RouteSegmentPaths> RouteSegmentPaths { get; set; }
        public virtual ICollection<RouteSegmentSpeeds> RouteSegmentSpeeds { get; set; }
        public virtual ICollection<TestLocationStats> TestLocationStats { get; set; }
        public virtual ICollection<TestSpeedStats> TestSpeedStats { get; set; }
    }
}

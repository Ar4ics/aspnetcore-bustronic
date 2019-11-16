using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Competitors
    {
        public int OwnerId { get; set; }
        public int RouteId { get; set; }
        public int RouteSegmentId { get; set; }
        public int CompetitorRouteId { get; set; }
        public int? Rate { get; set; }
        public bool IsAssigned { get; set; }
        public int Id { get; set; }
        public bool AutoAssigned { get; set; }

        public virtual Routes CompetitorRoute { get; set; }
        public virtual Owners Owner { get; set; }
        public virtual Routes Route { get; set; }
        public virtual RouteSegments RouteSegment { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class DriverCompetitors
    {
        public int Id { get; set; }
        public int DriverRouteId { get; set; }
        public int CompetitorRouteId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int RouteSegmentId { get; set; }

        public virtual Routes CompetitorRoute { get; set; }
        public virtual DriverRoutes DriverRoute { get; set; }
        public virtual RouteSegments RouteSegment { get; set; }
    }
}

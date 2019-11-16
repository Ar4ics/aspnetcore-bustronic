using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Segments
    {
        public Segments()
        {
            RouteSegments = new HashSet<RouteSegments>();
            SegmentPaths = new HashSet<SegmentPaths>();
        }

        public int Id { get; set; }
        public int FromStationPlatformId { get; set; }
        public int ToStationPlatformId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CityId { get; set; }

        public virtual StationPlatforms FromStationPlatform { get; set; }
        public virtual StationPlatforms ToStationPlatform { get; set; }
        public virtual ICollection<RouteSegments> RouteSegments { get; set; }
        public virtual ICollection<SegmentPaths> SegmentPaths { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class RouteSegmentSpeeds
    {
        public int Id { get; set; }
        public int RouteSegmentId { get; set; }
        public int TimeIntervalId { get; set; }
        public double AverageSpeed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? History { get; set; }

        public virtual RouteSegments RouteSegment { get; set; }
        public virtual TimeIntervals TimeInterval { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class RouteSegmentPaths
    {
        public int Id { get; set; }
        public int RouteSegmentId { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public int Position { get; set; }
        public double Distance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RouteSegments RouteSegment { get; set; }
    }
}

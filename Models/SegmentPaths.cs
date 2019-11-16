using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class SegmentPaths
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public LineString Path { get; set; }
        public int? Distance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Segments Segment { get; set; }
    }
}

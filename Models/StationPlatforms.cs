using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class StationPlatforms
    {
        public StationPlatforms()
        {
            SegmentsFromStationPlatform = new HashSet<Segments>();
            SegmentsToStationPlatform = new HashSet<Segments>();
            StationPlatformVehicleTypes = new HashSet<StationPlatformVehicleTypes>();
        }

        public int Id { get; set; }
        public int StationId { get; set; }
        public string Title { get; set; }
        public Point Location { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? OsmId { get; set; }

        public virtual Stations Station { get; set; }
        public virtual ICollection<Segments> SegmentsFromStationPlatform { get; set; }
        public virtual ICollection<Segments> SegmentsToStationPlatform { get; set; }
        public virtual ICollection<StationPlatformVehicleTypes> StationPlatformVehicleTypes { get; set; }
    }
}

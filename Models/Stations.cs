using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Stations
    {
        public Stations()
        {
            RouteStations = new HashSet<RouteStations>();
            StationPlatforms = new HashSet<StationPlatforms>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string PublicTitle { get; set; }

        public virtual Cities City { get; set; }
        public virtual ICollection<RouteStations> RouteStations { get; set; }
        public virtual ICollection<StationPlatforms> StationPlatforms { get; set; }
    }
}

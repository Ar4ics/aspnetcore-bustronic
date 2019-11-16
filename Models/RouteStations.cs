using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class RouteStations
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int StationId { get; set; }
        public int Position { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsReverse { get; set; }

        public virtual Routes Route { get; set; }
        public virtual Stations Station { get; set; }
    }
}

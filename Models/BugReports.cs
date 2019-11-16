using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class BugReports
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Message { get; set; }
        public bool? IsSolved { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Point Real { get; set; }
        public Point Normalized { get; set; }
        public int? RouteId { get; set; }

        public virtual Routes Route { get; set; }
    }
}

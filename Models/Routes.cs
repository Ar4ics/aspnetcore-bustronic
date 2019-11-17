using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Routes
    {
        public Routes()
        {
            BugReports = new HashSet<BugReports>();
            CompetitorsCompetitorRoute = new HashSet<Competitors>();
            CompetitorsRoute = new HashSet<Competitors>();
            DriverCompetitors = new HashSet<DriverCompetitors>();
            DriverRoutes = new HashSet<DriverRoutes>();
            MovingVehicles = new HashSet<MovingVehicles>();
            OwnerRoutes = new HashSet<OwnerRoutes>();
            RouteSegments = new HashSet<RouteSegments>();
            RouteStations = new HashSet<RouteStations>();
            UnverifiedVehicles = new HashSet<UnverifiedVehicles>();
            Vehicles = new HashSet<Vehicles>();
            VerifiedVehicles = new HashSet<VerifiedVehicles>();
        }

        public int Id { get; set; }
        public int CityId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsReady { get; set; }
        public int? Cost { get; set; }

        public virtual Cities City { get; set; }
        public virtual VehicleTypes VehicleType { get; set; }
        public virtual ICollection<BugReports> BugReports { get; set; }
        public virtual ICollection<Competitors> CompetitorsCompetitorRoute { get; set; }
        public virtual ICollection<Competitors> CompetitorsRoute { get; set; }
        public virtual ICollection<DriverCompetitors> DriverCompetitors { get; set; }
        public virtual ICollection<DriverRoutes> DriverRoutes { get; set; }
        public virtual ICollection<MovingVehicles> MovingVehicles { get; set; }
        public virtual ICollection<OwnerRoutes> OwnerRoutes { get; set; }
        public virtual ICollection<RouteSegments> RouteSegments { get; set; }
        public virtual ICollection<RouteStations> RouteStations { get; set; }
        public virtual ICollection<UnverifiedVehicles> UnverifiedVehicles { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
        public virtual ICollection<VerifiedVehicles> VerifiedVehicles { get; set; }
    }
}

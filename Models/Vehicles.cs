using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Vehicles
    {
        public Vehicles()
        {
            MovingVehicles = new HashSet<MovingVehicles>();
            VehicleDriverRoutes = new HashSet<VehicleDriverRoutes>();
            VehicleDrivers = new HashSet<VehicleDrivers>();
            VehicleOwnerRoutes = new HashSet<VehicleOwnerRoutes>();
        }

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int VehicleTypeId { get; set; }
        public int? VehicleKindId { get; set; }
        public int? CityId { get; set; }
        public int? DriverId { get; set; }
        public int? RouteId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Number { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Cities City { get; set; }
        public virtual Drivers Driver { get; set; }
        public virtual Owners Owner { get; set; }
        public virtual Routes Route { get; set; }
        public virtual VehicleKinds VehicleKind { get; set; }
        public virtual VehicleTypes VehicleType { get; set; }
        public virtual ICollection<MovingVehicles> MovingVehicles { get; set; }
        public virtual ICollection<VehicleDriverRoutes> VehicleDriverRoutes { get; set; }
        public virtual ICollection<VehicleDrivers> VehicleDrivers { get; set; }
        public virtual ICollection<VehicleOwnerRoutes> VehicleOwnerRoutes { get; set; }
    }
}

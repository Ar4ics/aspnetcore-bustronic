using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Drivers
    {
        public Drivers()
        {
            DriverRoutes = new HashSet<DriverRoutes>();
            VehicleDrivers = new HashSet<VehicleDrivers>();
            Vehicles = new HashSet<Vehicles>();
        }

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public DateTime JoinedAt { get; set; }
        public string DrivingLicenseCategory { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string PhoneNo { get; set; }
        public int? CityId { get; set; }
        public string Email { get; set; }
        public bool? IsReady { get; set; }

        public virtual Cities City { get; set; }
        public virtual Owners Owner { get; set; }
        public virtual ICollection<DriverRoutes> DriverRoutes { get; set; }
        public virtual ICollection<VehicleDrivers> VehicleDrivers { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AspNetCoreBustronic.Models
{
    public partial class Cities
    {
        public Cities()
        {
            CityVersions = new HashSet<CityVersions>();
            Drivers = new HashSet<Drivers>();
            Managers = new HashSet<Managers>();
            OwnerCities = new HashSet<OwnerCities>();
            PassengerDevices = new HashSet<PassengerDevices>();
            Routes = new HashSet<Routes>();
            Stations = new HashSet<Stations>();
            UnverifiedVehicles = new HashSet<UnverifiedVehicles>();
            Vehicles = new HashSet<Vehicles>();
            VerifiedVehicles = new HashSet<VerifiedVehicles>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Point Location { get; set; }
        public int Timezone { get; set; }
        public string Description { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<CityVersions> CityVersions { get; set; }
        public virtual ICollection<Drivers> Drivers { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
        public virtual ICollection<OwnerCities> OwnerCities { get; set; }
        public virtual ICollection<PassengerDevices> PassengerDevices { get; set; }
        public virtual ICollection<Routes> Routes { get; set; }
        public virtual ICollection<Stations> Stations { get; set; }
        public virtual ICollection<UnverifiedVehicles> UnverifiedVehicles { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
        public virtual ICollection<VerifiedVehicles> VerifiedVehicles { get; set; }
    }
}

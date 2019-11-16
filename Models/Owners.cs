using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Owners
    {
        public Owners()
        {
            Competitors = new HashSet<Competitors>();
            Drivers = new HashSet<Drivers>();
            OwnerCities = new HashSet<OwnerCities>();
            OwnerRoutes = new HashSet<OwnerRoutes>();
            Vehicles = new HashSet<Vehicles>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string JuridicalTitle { get; set; }
        public string Description { get; set; }
        public string PhoneNo { get; set; }
        public bool? IsActive { get; set; }
        public bool IsVirtual { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Competitors> Competitors { get; set; }
        public virtual ICollection<Drivers> Drivers { get; set; }
        public virtual ICollection<OwnerCities> OwnerCities { get; set; }
        public virtual ICollection<OwnerRoutes> OwnerRoutes { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}

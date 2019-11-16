using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class OwnerCities
    {
        public int OwnerId { get; set; }
        public int CityId { get; set; }
        public int Id { get; set; }

        public virtual Cities City { get; set; }
        public virtual Owners Owner { get; set; }
    }
}

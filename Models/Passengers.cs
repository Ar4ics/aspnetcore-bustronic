using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class Passengers
    {
        public Passengers()
        {
            PassengerDevices = new HashSet<PassengerDevices>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<PassengerDevices> PassengerDevices { get; set; }
    }
}

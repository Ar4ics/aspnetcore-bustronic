using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class PassengerDevices
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int? CityId { get; set; }
        public string Uuid { get; set; }
        public string PushToken { get; set; }
        public string DeviceType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Cities City { get; set; }
        public virtual Passengers Passenger { get; set; }
    }
}

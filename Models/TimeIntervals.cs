using System;
using System.Collections.Generic;

namespace AspNetCoreBustronic.Models
{
    public partial class TimeIntervals
    {
        public TimeIntervals()
        {
            RouteSegmentSpeeds = new HashSet<RouteSegmentSpeeds>();
        }

        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public int DayOfWeek { get; set; }

        public virtual ICollection<RouteSegmentSpeeds> RouteSegmentSpeeds { get; set; }
    }
}

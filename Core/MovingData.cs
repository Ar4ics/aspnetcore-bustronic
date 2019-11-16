using System.Collections.Generic;
using System.Linq;
using AspNetCoreBustronic.Models;

namespace AspNetCoreBustronic.Core
{
    public class MovingData
    {
        public Dictionary<int, List<RouteSegments>> routeSegments { get; set; }
        public Dictionary<int, List<RouteSegmentPaths>> routeSegmentPaths { get; set; }
        public Dictionary<int, List<RouteSegmentSpeeds>> routeSegmentSpeeds { get; set; }
        public Dictionary<int, TimeIntervals> timeIntervals { get; set; }
        public List<MovingVehicles> movingVehicles { get; set; }

        private readonly BustronicContext _context;

        public MovingData(BustronicContext context)
        {
            _context = context;
        }

        public static MovingData Create(BustronicContext context)
        {
            var routeSegments = context.RouteSegments.ToList();
            var routeSegmentPaths = context.RouteSegmentPaths.ToList();
            var routeSegmentSpeeds = context.RouteSegmentSpeeds.ToList();
            var timeIntervals = context.TimeIntervals.ToList();
            var movingVehicles = context.MovingVehicles.ToList();

            var movingData = new MovingData(context)
            {
                routeSegments = routeSegments.GroupBy(e => e.RouteId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                routeSegmentPaths = routeSegmentPaths.GroupBy(e => e.RouteSegmentId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                routeSegmentSpeeds = routeSegmentSpeeds.GroupBy(e => e.RouteSegmentId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                timeIntervals = timeIntervals.ToDictionary(x => x.Id, x => x),
                movingVehicles = movingVehicles
            };
            return movingData;
        }
    }
}
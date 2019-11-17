using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreBustronic.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBustronic.Core
{
    public class MovingData
    {
        public Dictionary<int, List<RouteSegments>> RouteSegments { get; set; }
        public Dictionary<int, List<RouteSegmentPaths>> RouteSegmentPaths { get; set; }
        public Dictionary<int, Dictionary<int, RouteSegmentSpeeds>> RouteSegmentSpeeds { get; set; }
        public List<TimeIntervals> TimeIntervals { get; set; }
        public List<MovingVehicles> MovingVehicles { get; set; }

        private readonly BustronicContext _context;

        public TimeIntervals GetTimeInterval(DateTime time) {
            return TimeIntervals.Where(e =>
            {
                return time.TimeOfDay >= e.TimeStart.TimeOfDay && time.TimeOfDay <= e.TimeEnd.TimeOfDay;
            }).FirstOrDefault();
        }

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
                RouteSegments = routeSegments.GroupBy(e => e.RouteId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                RouteSegmentPaths = routeSegmentPaths.GroupBy(e => e.RouteSegmentId).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                RouteSegmentSpeeds = routeSegmentSpeeds
                    .GroupBy(e => e.RouteSegmentId)
                    .ToDictionary(gdc => gdc.Key, 
                        gdc => gdc.Where(g => g.History == 0).ToDictionary(c => c.TimeIntervalId, c => c)),
                TimeIntervals = timeIntervals.ToList(),
                MovingVehicles = movingVehicles
            };
            return movingData;
        }
    }
}
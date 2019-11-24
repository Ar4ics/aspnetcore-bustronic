using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreBustronic.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBustronic.Core
{
    public class MovingData
    {
        public List<Routes> Routes { get; set; }
        public Dictionary<int, List<RouteSegmentPaths>> RouteSegmentPaths { get; set; }
        public Dictionary<int, Dictionary<int, RouteSegmentSpeeds>> RouteSegmentSpeeds { get; set; }
        public List<TimeIntervals> TimeIntervals { get; set; }

        private readonly BustronicContext _context;

        public TimeIntervals GetTimeInterval(DateTime time)
        {
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
            var routes = context.Routes
                .Where(e => e.IsReady.HasValue && e.IsReady.Value == true && e.DeletedAt == null)
                .Include(e => e.RouteSegments)
                .AsEnumerable()
                .Select(e =>
                {
                    e.RouteSegments = e.RouteSegments.Where(e => e.DeletedAt == null).OrderBy(e => e.Position).ToList();
                    return e;
                })
                .ToList();
            var routeSegmentPaths = context.RouteSegmentPaths.ToList();
            var routeSegmentSpeeds = context.RouteSegmentSpeeds.ToList();
            var timeIntervals = context.TimeIntervals.ToList();

            var movingData = new MovingData(context)
            {
                Routes = routes,
                RouteSegmentPaths = routeSegmentPaths
                    .GroupBy(e => e.RouteSegmentId)
                    .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList()),
                RouteSegmentSpeeds = routeSegmentSpeeds
                    .GroupBy(e => e.RouteSegmentId)
                    .ToDictionary(gdc => gdc.Key,
                        gdc => gdc.Where(g => g.History == 0).ToDictionary(c => c.TimeIntervalId, c => c)),
                TimeIntervals = timeIntervals.ToList(),
            };
            return movingData;
        }

        public List<MovingVehicles> GetMovingVehicles() {
            return _context.MovingVehicles.ToList();
        }

        static Random rnd = new Random();

        public void Remove(List<MovingVehicleInfo> movingVehicleInfos)
        {
            // _context.MovingVehicles.RemoveRange(movingVehicleInfos.Select(e => e.MovingVehicle));
            // _context.SaveChanges();
            _context.BulkDelete(movingVehicleInfos.Select(e => e.MovingVehicle));
        }

        public void Update(List<MovingVehicleInfo> movingVehicleInfos)
        {
            _context.BulkUpdate(movingVehicleInfos.Select(e => e.MovingVehicle));
        }

        public void Insert(List<MovingVehicleInfo> movingVehicleInfos)
        {
            var list = new List<MovingVehicles>();
            foreach (var item in movingVehicleInfos)
            {
                var cityId = item.Route.CityId;
                var routes = Routes.Where(e => e.CityId == cityId).ToList();
                if (routes.Count <= 0)
                {
                    continue;
                }
                int r = rnd.Next(routes.Count);
                var route = routes[r];
                var routeSegments = route.RouteSegments.ToList();
                if (routeSegments.Count <= 0)
                {
                    continue;
                }
                r = rnd.Next(routeSegments.Count);
                var routeSegment = routeSegments[r];

                List<RouteSegmentPaths> routeSegmentPaths;
                if (!RouteSegmentPaths.TryGetValue(routeSegment.Id, out routeSegmentPaths))
                {
                    continue;
                }
                if (routeSegmentPaths.Count <= 0)
                {
                    continue;
                }
                var path = routeSegmentPaths[0];
                var now = DateTime.UtcNow;
                var mv = new MovingVehicles()
                {
                    RouteId = routeSegment.RouteId,
                    RouteSegmentId = routeSegment.Id,
                    SegmentId = item.MovingVehicle.SegmentId,
                    SourceType = "bot",
                    AverageSpeed = 25,
                    Real = path.StartPoint,
                    Normalized = path.StartPoint,
                    CreatedAt = now,
                    UpdatedAt = now,
                    ConfirmedAt = now,
                    LastPathPosition = 1,
                    DistanceFromLastPath = 0.0,
                    Relevance = 100
                };
                list.Add(mv);
                //_context.MovingVehicles.Add(mv);
            }
            //_context.SaveChanges();
            _context.BulkInsert(list);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreBustronic.Models;
using CSharpFunctionalExtensions;

namespace AspNetCoreBustronic.Core
{
    public class MovingVehicleInfo
    {
        public MovingVehicles MovingVehicle { get; set; }
        public List<RouteSegments> RouteSegments { get; private set; }
        public int FirstRouteSegmentIndex { get; private set; }
        public List<RouteSegmentPaths> RouteSegmentPaths { get; private set; }
        public int LastPathIndex { get; private set; }
        public Dictionary<int, RouteSegmentSpeeds> RouteSegmentSpeeds { get; private set; }
        public static List<MovingVehicleInfo> Create(MovingData movingData)
        {
            var list = new List<MovingVehicleInfo>();
            foreach (var item in movingData.MovingVehicles)
            {
                if (
                    !item.RouteSegmentId.HasValue ||
                    !item.ConfirmedAt.HasValue ||
                    !item.LastPathPosition.HasValue ||
                    !item.DistanceFromLastPath.HasValue) { continue; }

                List<RouteSegments> routeSegments;
                if (!movingData.RouteSegments.TryGetValue(item.RouteId, out routeSegments)) { continue; }
            
                List<RouteSegmentPaths> routeSegmentPaths;
                if (!movingData.RouteSegmentPaths.TryGetValue(item.RouteSegmentId.Value, out routeSegmentPaths)) { continue; }

                Dictionary<int, RouteSegmentSpeeds> routeSegmentSpeeds;
                if (!movingData.RouteSegmentSpeeds.TryGetValue(item.RouteSegmentId.Value, out routeSegmentSpeeds)) { continue; }
                
                var firstRouteSegmentIndex = routeSegments.FindIndex(e =>
                {
                    return e.Id == item.RouteSegmentId.Value;
                });
                if (firstRouteSegmentIndex == -1) { continue; }

                var lastPathIndex = routeSegmentPaths.FindIndex(e =>
                {
                    return e.Position == item.LastPathPosition.Value;
                });
                if (lastPathIndex == -1) { continue; }


                var o = new MovingVehicleInfo
                {
                    MovingVehicle = item,
                    RouteSegments = routeSegments,
                    RouteSegmentPaths = routeSegmentPaths,
                    RouteSegmentSpeeds = routeSegmentSpeeds,
                    FirstRouteSegmentIndex = firstRouteSegmentIndex,
                    LastPathIndex = lastPathIndex
                };
                list.Add(o);
            }
            return list;
        }
    }
}
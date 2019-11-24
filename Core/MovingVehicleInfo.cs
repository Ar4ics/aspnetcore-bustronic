using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreBustronic.Models;
using CSharpFunctionalExtensions;

namespace AspNetCoreBustronic.Core
{
    public class MovingVehicleInfo
    {
        public MovingVehicles MovingVehicle { get; private set; }
        public Routes Route { get; private set; }
        public List<RouteSegments> RouteSegments { get; private set; }
        public int FirstRouteSegmentIndex { get; set; }
        public List<RouteSegmentPaths> RouteSegmentPaths { get; set; }
        public int LastPathIndex { get; set; }
        public Dictionary<int, RouteSegmentSpeeds> RouteSegmentSpeeds { get; set; }
        public string Error { get; set; } = String.Empty;

        public static List<MovingVehicleInfo> Create(MovingData movingData)
        {
            var list = new List<MovingVehicleInfo>();
            foreach (var item in movingData.GetMovingVehicles())
            {
                if (
                    !item.RouteSegmentId.HasValue ||
                    !item.ConfirmedAt.HasValue ||
                    !item.LastPathPosition.HasValue ||
                    !item.DistanceFromLastPath.HasValue)
                {
                    continue;
                }
                var route = movingData.Routes.Find(e => e.Id == item.RouteId);
                if (route == null)
                {
                    continue;
                }
                List<RouteSegments> routeSegments = route.RouteSegments.ToList();
                list.Add(new MovingVehicleInfo()
                {
                    MovingVehicle = item,
                    Route = route,
                    RouteSegments = routeSegments,
                });

            }
            return list;
        }

        public dynamic ToClient()
        {
            var item = MovingVehicle;
            return new
            {
                id = item.Id,
                normalized = new[] { item.Normalized.X, item.Normalized.Y },
                route_segment_id = item.RouteSegmentId,
                relevance = item.Relevance,
                route_id = item.RouteId,
                title = item.Route.Title,
                updated_at = item.UpdatedAt,
                average_speed = item.AverageSpeed,
                city_id = item.Route.CityId,
                confirmed_at = item.ConfirmedAt,
                distance_from_last_path = item.DistanceFromLastPath,
                last_path_position = item.LastPathPosition
            };
        }
    }
}
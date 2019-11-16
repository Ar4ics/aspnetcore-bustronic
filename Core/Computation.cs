using System;
using System.Collections.Generic;
using AspNetCoreBustronic.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using CSharpFunctionalExtensions;
using System.Diagnostics;
using AspNetCoreBustronic.Core;

namespace AspNetCoreBustronic.Controllers
{
    public class Computation
    {
        private MovingData _movingData;
        public Computation(MovingData movingData)
        {
            _movingData = movingData;
        }

        public Result<List<MovingVehicles>> compute(List<MovingVehicles> movingVehicles)
        {
            try
            {
                var result = predictAll(movingVehicles);
                Debug.Assert(result.Count == movingVehicles.Count);
                return Result.Success(result);
            }
            catch (Exception e)
            {
                return Result.Failure<List<MovingVehicles>>(e.Message);
            }

        }

        private List<MovingVehicles> predictAll(List<MovingVehicles> movingVehicles, int countMilliseconds = 5)
        {
            var prevTime = DateTime.Now;
            var nextTime = prevTime + TimeSpan.FromSeconds(countMilliseconds);
            var res = new List<MovingVehicles>();
            foreach (var mv in movingVehicles)
            {
                var r = predictLocation(mv, prevTime, nextTime);
                res.Add(r);
            }
            return res;
        }
        private MovingVehicles predictLocation(
            MovingVehicles mv,
            DateTime prevTime,
            DateTime nextTime,
            int relevance = 5
            )
        {
            var routeSegmentId = mv.RouteSegmentId ?? throw new Exception();
            var confirmedAt = mv.ConfirmedAt ?? throw new Exception();
            var distanceFromLastPath = mv.DistanceFromLastPath ?? throw new Exception();
            var lastPathPosition = mv.LastPathPosition ?? throw new Exception();

            var routeId = mv.RouteId;
            var rss = _movingData.routeSegmentSpeeds[routeSegmentId];

            var averageSpeed = rss.Where(e =>
            {
                var timeIntervalId = e.TimeIntervalId;
                var timeInterval = _movingData.timeIntervals[timeIntervalId];
                return nextTime.TimeOfDay >= timeInterval.TimeStart.TimeOfDay
                && nextTime.TimeOfDay <= timeInterval.TimeEnd.TimeOfDay;
            }).First().AverageSpeed;
            mv.AverageSpeed = Convert.ToInt32(averageSpeed);
            var diff = Convert.ToInt32((nextTime - confirmedAt).TotalMilliseconds);
            mv.Relevance = Convert.ToInt32(100 * (1.0 - diff / (relevance * 60 * 1000)));
            mv.UpdatedAt = nextTime;

            var routeSegments = _movingData.routeSegments[routeId];
            var firstRouteSegmentIndex = routeSegments.FindIndex(e =>
            {
                return e.Id == routeSegmentId;
            });
            if (firstRouteSegmentIndex == -1) throw new Exception();

            var paths = _movingData.routeSegmentPaths[routeSegmentId];
            var lastPathIndex = paths.FindIndex(e =>
            {
                return e.Position == lastPathPosition;
            });
            if (lastPathIndex == -1) throw new Exception();

            var deltaTime = Convert.ToInt32((nextTime - prevTime).TotalMilliseconds);
            var theoreticDistance = averageSpeed * deltaTime / 3600;
            if (theoreticDistance <= 0)
            {
                throw new Exception();
            }

            var nextRouteSegmentIndex = 0;
            var distanceToEndOfLastPath = paths[lastPathIndex].Distance - distanceFromLastPath;
            var distanceToEndOfFirstSegment = distanceToEndOfLastPath;
            var pathToEndOfFirstSegment = new List<RouteSegmentPaths>();
            for (var i = lastPathIndex + 1; i < paths.Count; i++)
            {
                distanceToEndOfFirstSegment += paths[i].Distance;
                pathToEndOfFirstSegment.Add(paths[i]);
            }

            if (theoreticDistance > distanceToEndOfFirstSegment)
            {
                nextRouteSegmentIndex++;
                var nextTheoreticDistance = theoreticDistance - distanceToEndOfFirstSegment;
                while (true)
                {
                    var nextRouteSegment = (firstRouteSegmentIndex + nextRouteSegmentIndex >= routeSegments.Count())
                    ? routeSegments[0]
                    : routeSegments[firstRouteSegmentIndex + nextRouteSegmentIndex];

                    var distanceBetweenTwoSegments = nextRouteSegment.Distance ?? throw new Exception();

                    if (Math.Round(nextTheoreticDistance - distanceBetweenTwoSegments) > 0)
                    {
                        nextRouteSegmentIndex++;
                        nextTheoreticDistance -= distanceBetweenTwoSegments;
                    }
                    else
                    {
                        var nextPath = _movingData.routeSegmentPaths[nextRouteSegment.Id];
                        return getLocationOnPath(mv, nextPath, nextTheoreticDistance, nextRouteSegment);
                    }
                }
            }
            else
            {
                if (theoreticDistance <= distanceToEndOfLastPath)
                {
                    if ((mv.DistanceFromLastPath + theoreticDistance) > paths[lastPathIndex].Distance)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        return calculateLocationOnPath(
                            mv,
                            paths[lastPathIndex],
                            distanceFromLastPath + theoreticDistance,
                            routeSegments[firstRouteSegmentIndex]
                        );
                    }
                }
                else
                {
                    return getLocationOnPath(
                        mv,
                        pathToEndOfFirstSegment,
                        theoreticDistance - distanceToEndOfLastPath,
                        routeSegments[firstRouteSegmentIndex]
                    );
                }
            }

        }

        private MovingVehicles getLocationOnPath(
            MovingVehicles mv,
            List<RouteSegmentPaths> path,
            double distance,
            RouteSegments routeSegment
            )
        {
            var nextDistance = distance;
            for (var i = 0; i < path.Count(); i++)
            {
                var currentDistance = path[i].Distance;
                var remainDistance = nextDistance - currentDistance;
                if (remainDistance > 0)
                {
                    nextDistance -= currentDistance;
                }
                else
                {
                    var fraction = nextDistance / currentDistance;
                    if (fraction < 0.0 || fraction > 1.0)
                    {
                        throw new Exception();
                    }
                    if (nextDistance < 0 && nextDistance > currentDistance)
                    {
                        throw new Exception();
                    }
                    return calculateLocationOnPath(mv, path[i], nextDistance, routeSegment);
                }
            }
            return calculateLocationOnPath(mv, path[path.Count() - 1], 1.0, routeSegment);
        }

        private double pi = Math.PI;

        private MovingVehicles calculateLocationOnPath(
            MovingVehicles mv,
            RouteSegmentPaths path,
            double distance,
            RouteSegments routeSegment
        )
        {
            var fraction = distance / path.Distance;
            var lat1 = path.StartPoint.Y / 180 * pi;
            var lng1 = path.StartPoint.X / 180 * pi;
            var lat2 = path.EndPoint.Y / 180 * pi;
            var lng2 = path.EndPoint.X / 180 * pi;
            var lng = lng1 + (lng2 - lng1) * fraction;
            if (lng2 == lng1)
            {
                lng2 += Math.Pow(10.0, -15.0);
            }
            var lat = Math.Atan(
                (Math.Tan(lat1) * Math.Sin(lng2 - lng) +
                        Math.Tan(lat2) * Math.Sin(lng - lng1)) / Math.Sin(lng2 - lng1)
            );
            var normalized = new Point(lng * 180 / pi, lat * 180 / pi);
            mv.Normalized = normalized;
            mv.RouteSegmentId = routeSegment.Id;
            mv.LastPathPosition = path.Position;
            mv.DistanceFromLastPath = distance;
            return mv;
        }

    }

}
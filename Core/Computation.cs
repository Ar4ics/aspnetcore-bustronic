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
        private DateTime _prevTime = DateTime.UtcNow;
        private DateTime _nextTime = DateTime.UtcNow;
        private int _relevance = 5;

        public Computation(MovingData movingData)
        {
            _movingData = movingData;
        }


        private Result<MovingVehicleInfo> SelectValid(MovingVehicleInfo movingVehicleInfo)
        {
            if (
                !movingVehicleInfo.MovingVehicle.RouteSegmentId.HasValue ||
                !movingVehicleInfo.MovingVehicle.ConfirmedAt.HasValue ||
                !movingVehicleInfo.MovingVehicle.LastPathPosition.HasValue ||
                !movingVehicleInfo.MovingVehicle.DistanceFromLastPath.HasValue)
            {
                return IsInvalid();
            }
            List<RouteSegmentPaths> routeSegmentPaths;
            if (!_movingData.RouteSegmentPaths.TryGetValue(movingVehicleInfo.MovingVehicle.RouteSegmentId.Value, out routeSegmentPaths))
            {
                return IsInvalid();
            }

            Dictionary<int, RouteSegmentSpeeds> routeSegmentSpeeds;
            if (!_movingData.RouteSegmentSpeeds.TryGetValue(movingVehicleInfo.MovingVehicle.RouteSegmentId.Value, out routeSegmentSpeeds))
            {
                return IsInvalid();
            }

            var firstRouteSegmentIndex = movingVehicleInfo.RouteSegments.FindIndex(e =>
            {
                return e.Id == movingVehicleInfo.MovingVehicle.RouteSegmentId.Value;
            });
            if (firstRouteSegmentIndex == -1)
            {
                return IsInvalid();
            }

            var lastPathIndex = routeSegmentPaths.FindIndex(e =>
            {
                return e.Position == movingVehicleInfo.MovingVehicle.LastPathPosition.Value;
            });
            if (lastPathIndex == -1)
            {
                return IsInvalid();
            }
            movingVehicleInfo.RouteSegmentPaths = routeSegmentPaths;
            movingVehicleInfo.RouteSegmentSpeeds = routeSegmentSpeeds;
            movingVehicleInfo.FirstRouteSegmentIndex = firstRouteSegmentIndex;
            movingVehicleInfo.LastPathIndex = lastPathIndex;

            return Result.Success(movingVehicleInfo);
        }

        private Result<MovingVehicleInfo> IsInvalid()
        {
            return Result.Failure<MovingVehicleInfo>("MovingVehicle is invalid");
        }

        public List<MovingVehicleInfo> compute(List<MovingVehicleInfo> movingVehicleInfos)
        {
            _prevTime = _nextTime;
            _nextTime = DateTime.UtcNow;
            return movingVehicleInfos.AsParallel()
                .Select(e => SelectValid(e)).Where(result => result.IsSuccess).Select(r => r.Value)
                .Select(e => PredictLocation(e)).Where(result => result.IsSuccess).Select(r => r.Value).ToList();
        }

        private Result<MovingVehicleInfo> PredictLocation(MovingVehicleInfo mvi)
        {
            var diff = Convert.ToInt32((_nextTime - mvi.MovingVehicle.ConfirmedAt.Value).TotalMilliseconds);
            var relevance = Convert.ToInt32(100 * (1.0 - diff / (_relevance * 60 * 1000)));
            if (relevance <= 0) {
                return Result.Failure<MovingVehicleInfo>("relevance <= 0");
            }
            mvi.MovingVehicle.Relevance = relevance;
            var timeInterval = _movingData.GetTimeInterval(_nextTime);
            if (timeInterval == null)
            {
                return Result.Failure<MovingVehicleInfo>("timeInterval not found");
            }
            RouteSegmentSpeeds routeSegmentSpeed;
            if (!mvi.RouteSegmentSpeeds.TryGetValue(timeInterval.Id, out routeSegmentSpeed))
            {
                return Result.Failure<MovingVehicleInfo>("routeSegmentSpeed not found");
            }
            mvi.MovingVehicle.AverageSpeed = Convert.ToInt32(routeSegmentSpeed.AverageSpeed);

            var deltaTime = Convert.ToInt32((_nextTime - _prevTime).TotalMilliseconds);
            var theoreticDistance = ((double)(mvi.MovingVehicle.AverageSpeed.Value)) * deltaTime / 3600;
            if (theoreticDistance <= 0)
            {
                return Result.Failure<MovingVehicleInfo>("theoreticDistance <= 0");
            }

            var nextRouteSegmentIndex = 0;
            var distanceToEndOfLastPath = mvi.RouteSegmentPaths[mvi.LastPathIndex].Distance - mvi.MovingVehicle.DistanceFromLastPath.Value;
            var distanceToEndOfFirstSegment = distanceToEndOfLastPath;
            var pathToEndOfFirstSegment = new List<RouteSegmentPaths>();
            for (var i = mvi.LastPathIndex + 1; i < mvi.RouteSegmentPaths.Count; i++)
            {
                distanceToEndOfFirstSegment += mvi.RouteSegmentPaths[i].Distance;
                pathToEndOfFirstSegment.Add(mvi.RouteSegmentPaths[i]);
            }

            if (theoreticDistance > distanceToEndOfFirstSegment)
            {
                nextRouteSegmentIndex++;
                var nextTheoreticDistance = theoreticDistance - distanceToEndOfFirstSegment;
                while (true)
                {
                    var i = (mvi.FirstRouteSegmentIndex + nextRouteSegmentIndex >= mvi.RouteSegments.Count()) ? 0
                        : mvi.FirstRouteSegmentIndex + nextRouteSegmentIndex;
                    var nextRouteSegment = mvi.RouteSegments[i];

                    var distanceBetweenTwoSegments = nextRouteSegment.Distance.Value;

                    if (Math.Round(nextTheoreticDistance - distanceBetweenTwoSegments) > 0)
                    {
                        nextRouteSegmentIndex++;
                        nextTheoreticDistance -= distanceBetweenTwoSegments;
                    }
                    else
                    {
                        List<RouteSegmentPaths> nextPath;
                        if (!_movingData.RouteSegmentPaths.TryGetValue(nextRouteSegment.Id, out nextPath))
                        {
                            return Result.Failure<MovingVehicleInfo>("nextPath not found");
                        }
                        return GetLocationOnPath(mvi, nextPath, nextTheoreticDistance, nextRouteSegment);
                    }
                }
            }
            else
            {
                if (theoreticDistance <= distanceToEndOfLastPath)
                {
                    if ((mvi.MovingVehicle.DistanceFromLastPath.Value + theoreticDistance)
                        > mvi.RouteSegmentPaths[mvi.LastPathIndex].Distance)
                    {
                        return Result.Failure<MovingVehicleInfo>("DistanceFromLastPath > LastPathIndex.Distance");
                    }
                    else
                    {
                        return CalculateLocationOnPath(
                            mvi,
                            mvi.RouteSegmentPaths[mvi.LastPathIndex],
                            mvi.MovingVehicle.DistanceFromLastPath.Value + theoreticDistance,
                            mvi.RouteSegments[mvi.FirstRouteSegmentIndex]
                        );
                    }
                }
                else
                {
                    return GetLocationOnPath(
                        mvi,
                        pathToEndOfFirstSegment,
                        theoreticDistance - distanceToEndOfLastPath,
                        mvi.RouteSegments[mvi.FirstRouteSegmentIndex]
                    );
                }
            }

        }

        private Result<MovingVehicleInfo> GetLocationOnPath(
            MovingVehicleInfo mvi,
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
                        return Result.Failure<MovingVehicleInfo>("fraction < 0.0 || fraction > 1.0");
                    }
                    if (nextDistance < 0 && nextDistance > currentDistance)
                    {
                        return Result.Failure<MovingVehicleInfo>("nextDistance < 0 && nextDistance > currentDistance");
                    }
                    return CalculateLocationOnPath(mvi, path[i], nextDistance, routeSegment);
                }
            }
            return CalculateLocationOnPath(mvi, path[path.Count() - 1], 1.0, routeSegment);
        }

        private double pi = Math.PI;

        private Result<MovingVehicleInfo> CalculateLocationOnPath(
            MovingVehicleInfo mvi,
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
            mvi.MovingVehicle.Normalized = normalized;
            mvi.MovingVehicle.RouteSegmentId = routeSegment.Id;
            mvi.MovingVehicle.LastPathPosition = path.Position;
            mvi.MovingVehicle.DistanceFromLastPath = distance;
            mvi.MovingVehicle.UpdatedAt = _nextTime;
            return Result.Success(mvi);
        }

    }

}
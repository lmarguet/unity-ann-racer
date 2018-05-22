using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class DriveUtils
    {

        public static Dictionary<string,float> PerformRayCasts(Transform transform, float visibleDistance)
        {
            var position = transform.position;
            var rightDirection = transform.right;

            var distances = new Dictionary<string, float>
            {
                {"forward", CastDistance(position, transform.forward, visibleDistance)},
                {"right", CastDistance(position, rightDirection, visibleDistance)},
                {"right45", CastDistance(position, CalculateAngleDirection(-45, rightDirection), visibleDistance)},
                {"left", CastDistance(position, -rightDirection, visibleDistance)},
                {"left45", CastDistance(position, CalculateAngleDirection(45, -rightDirection), visibleDistance)}
            };


            return distances;
        }

        public static Vector3 CalculateAngleDirection(float angle, Vector3 direction)
        {
            return Quaternion.AngleAxis(angle, Vector3.up) * direction;
        }

        public static float CastDistance(Vector3 position, Vector3 direction, float visibleDistance)
        {
            RaycastHit hit;
            return Physics.Raycast(position, direction, out hit, visibleDistance)
                ? NormalizeHitDistance(visibleDistance, hit.distance)
                : 0;
        }

        public static float NormalizeHitDistance(float visibleDistance, float hitDistance)
        {
            return 1 - Round(hitDistance / visibleDistance);
        }
    
        public static float Round(float x)
        {
            return (float) Math.Round(x, MidpointRounding.AwayFromZero) / 2.0f;
        }
    }
}
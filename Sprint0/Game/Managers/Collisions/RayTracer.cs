using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint0
{
    public static class RayTracer
    {
        public static bool Raycast(Vector2 origin, Vector2 target, List<Entity> entities, 
                                     out Entity closestHitEntity, out Vector2 closestHitPoint)
        {
            closestHitEntity = null;
            closestHitPoint = Vector2.Zero;
            float nearestDistance = float.MaxValue;
            bool intersectionFound = false;

            foreach (Entity entity in entities)
            {
                Rectangle bounds = entity.GetBounds();
                Vector2 intersection;
                if (LineIntersectsRectangle(origin, target, bounds, out intersection))
                {
                    float distance = Vector2.Distance(origin, intersection);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        closestHitEntity = entity;
                        closestHitPoint = intersection;
                        intersectionFound = true;
                    }
                }
            }
            return intersectionFound;
        }

        public static bool LineIntersectsRectangle(Vector2 p1, Vector2 p2, Rectangle rect, out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.Zero;
            Vector2[] corners = new Vector2[]
            {
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Right, rect.Bottom),
                new Vector2(rect.Left, rect.Bottom)
            };

            bool foundIntersection = false;
            float closestDistance = float.MaxValue;
            Vector2 closestIntersection = Vector2.Zero;

            for (int i = 0; i < 4; i++)
            {
                int next = (i + 1) % 4;
                Vector2 tempIntersection;
                if (LineSegmentIntersection(p1, p2, corners[i], corners[next], out tempIntersection))
                {
                    float distance = Vector2.Distance(p1, tempIntersection);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIntersection = tempIntersection;
                        foundIntersection = true;
                    }
                }
            }
            intersectionPoint = closestIntersection;
            return foundIntersection;
        }

        public static bool LineSegmentIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End, out Vector2 intersection)
        {
            intersection = Vector2.Zero;
            float denominator = (line2End.Y - line2Start.Y) * (line1End.X - line1Start.X) - (line2End.X - line2Start.X) * (line1End.Y - line1Start.Y);
            if (denominator == 0)
                return false;

            float ua = ((line2End.X - line2Start.X) * (line1Start.Y - line2Start.Y) - (line2End.Y - line2Start.Y) * (line1Start.X - line2Start.X)) / denominator;
            float ub = ((line1End.X - line1Start.X) * (line1Start.Y - line2Start.Y) - (line1End.Y - line1Start.Y) * (line1Start.X - line2Start.X)) / denominator;

            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                intersection = new Vector2(line1Start.X + ua * (line1End.X - line1Start.X),
                                           line1Start.Y + ua * (line1End.Y - line1Start.Y));
                return true;
            }
            return false;
        }

        public static Vector2 ReflectVector(Vector2 incident, Vector2 normal)
        {
            return incident - 2 * Vector2.Dot(incident, normal) * normal;
        }

        // Determines if the player is fully exposed (i.e., no obstacles blocking their entire bounding box).
        public static bool IsPlayerFullyExposed(Vector2 mobPos, Rectangle playerBounds, List<Entity> obstacles)
        {
            Vector2[] checkPoints = new Vector2[]
            {
                new Vector2(playerBounds.Left, playerBounds.Top),      // Top-left
                new Vector2(playerBounds.Right, playerBounds.Top),     // Top-right
                new Vector2(playerBounds.Left, playerBounds.Bottom),   // Bottom-left
                new Vector2(playerBounds.Right, playerBounds.Bottom),  // Bottom-right
                new Vector2(playerBounds.Center.X, playerBounds.Center.Y) // Center
            };

            foreach (Vector2 point in checkPoints)
            {
                Entity hitEntity;
                Vector2 hitPoint;
                bool obstructed = RayTracer.Raycast(mobPos, point, obstacles, out hitEntity, out hitPoint);

                // If any one of these points is blocked, the player is NOT fully exposed
                if (obstructed && Vector2.Distance(mobPos, hitPoint) < Vector2.Distance(mobPos, point))
                {
                    return false;
                }
            }

            return true; // All sample points were clear
        }
    }
}

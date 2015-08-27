using System;
using UnityEngine;

namespace Assets.Code.Statics {
    static public class Extensions {
        public static void Scale(this BoxCollider2D box, float expansionFactor) {
            
            box.size = new Vector2(box.size.x + 2*expansionFactor, box.size.y + 2*expansionFactor);
        }

        public static Vector2 ToVector2(this Vector3 that) {
            return new Vector2(that.x,that.y);
        }

        public static void Scale(this CircleCollider2D circle, float expansionFactor) {
            circle.radius += expansionFactor;
        }

        public static bool PositionEquals(this Vector2 that, Vector2 rhs) {
            return (Math.Abs(that.x - rhs.x) < .01f && Math.Abs(that.y - rhs.y) < .01f);
        }
        public static bool IsSet(this Enum input, Enum matchTo)
        {
            return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
        }

        public static Vector2 GetCenter2D(this BoxCollider2D box) {
            var g_position = box.transform.position;
            var center = box.offset + (Vector2)g_position;
            return center;
        }
        public static Vector2 GetCenter2D(this PolygonCollider2D poly)
        {
            var highestX = -100000000f;
            var highestY = -100000000f;
            var lowestX = 1000000000f;
            var lowestY = 1000000000f;
            foreach (var point in poly.points)
            {
                if (point.x > highestX) highestX = point.x;
                if (point.y > highestY) highestY = point.y;
                if (point.x < lowestX) lowestX = point.y;
                if (point.y > lowestY) lowestY = point.y;
            }
            var xCenter = (highestX + lowestX) / 2;
            var yCenter = (highestY + lowestY) / 2;
            return new Vector2(xCenter, yCenter);
        }

        static public float DistanceTo(this Vector2 that, Vector2 rhs) {

            return Vector2.Distance(that, rhs);
        }
    }
}
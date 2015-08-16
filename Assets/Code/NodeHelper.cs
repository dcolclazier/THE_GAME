using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code
{
    public static class NodeHelper
    {
        public static IEnumerable<Node> GetNodes(Entity entity)
        {

            var circlecheck = entity.Collider as CircleCollider2D;
            if (circlecheck)
            {
                //Node[] nodeList = new Node[2];
                //Vector2 center = (Vector2)circ.transform.position;
                //Ray2D ray = new Ray2D(center, from - center);
                //Quaternion q = Quaternion.AngleAxis(90, Vector2.up);
                //ray.direction = q * ray.direction;
                //nodeList[0] = new Node(ray.GetPoint(buffer));
                //nodeList[1] = new Node(ray.GetPoint(-buffer));
                //return nodeList;

            }
            else
            {
                var polygoncheck = entity.Collider as PolygonCollider2D;
                if (!polygoncheck) yield break;
                foreach (var point in polygoncheck.points)
                {
                    yield return new Node(point);
                }
            }
        }


    }
}

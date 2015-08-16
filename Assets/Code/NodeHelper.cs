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
        //this code kinda sucks... maybe refactor it?
        public static IEnumerable<Node> GetNodes(Entity entity)
        {

            var circlecheck = entity.Collider as CircleCollider2D;
            var polygonCheck = entity.Collider as PolygonCollider2D;
            var boxCheck = entity.Collider as BoxCollider2D;
            if (circlecheck) {
                //how do you want to create the nodes for this? 

                //say wha?
                //Node[] nodeList = new Node[2];
                //Vector2 center = (Vector2)circ.transform.position;
                //Ray2D ray = new Ray2D(center, from - center);
                //Quaternion q = Quaternion.AngleAxis(90, Vector2.up);
                //ray.direction = q * ray.direction;
                //nodeList[0] = new Node(ray.GetPoint(buffer));
                //nodeList[1] = new Node(ray.GetPoint(-buffer));
                //return nodeList;

            } 
            else if(polygonCheck) {
                foreach (var point in polygonCheck.points) {
                    yield return new Node(point);
                }
                
            }
            else if (boxCheck) {
                Debug.Log("Yep - it's a box.");
                var nodes = new List<Node>();
                Debug.Log("Offset X: " + boxCheck.offset.x);
                Debug.Log("Offset Y: " + boxCheck.offset.y);
                Debug.Log("Box size mag: " + boxCheck.size.magnitude);

                nodes.Add(new Node(new Vector2(boxCheck.offset.x-boxCheck.size.x/2,
                                                boxCheck.offset.y+boxCheck.size.y/2)));

                nodes.Add(new Node(new Vector2(boxCheck.offset.x-boxCheck.size.x/2,
                                                boxCheck.offset.y-boxCheck.size.y/2)));

                nodes.Add(new Node(new Vector2(boxCheck.offset.x+boxCheck.size.x/2,
                                                boxCheck.offset.y+boxCheck.size.y/2)));

                nodes.Add(new Node(new Vector2(boxCheck.offset.x+boxCheck.size.x/2,
                                                boxCheck.offset.y-boxCheck.size.y/2)));
                foreach (var node in nodes) yield return node;
            }
            
        }


    }
}

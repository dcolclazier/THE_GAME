using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code
{
    public static class NodeManager
    {
        //this code kinda sucks... maybe refactor it?
        //1 - static classes blow - global state!
        //2 - repeating if's? I know there's only 4 cases, but it's like... c'mon.
        
        private static NodeHelper _nodeHelper = (new GameObject("NodeHelper")).AddComponent<NodeHelper>();

        public static IEnumerable<Node> GetNodes(Entity entity)
        {

            var circlecheck = entity.Collider as CircleCollider2D;
            var polygonCheck = entity.Collider as PolygonCollider2D;
            var boxCheck = entity.Collider as BoxCollider2D;
            if (circlecheck) {
                //how do you want to create the nodes for this?
                //it would be easiest to assign a precision, which defines how many
                //points are in the circle, such that the nodes are an approximation
                //of the circle represented as a polygon.

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
                var nodes = new List<Node> {
                    new Node(new Vector2(boxCheck.offset.x - boxCheck.size.x/2,
                        boxCheck.offset.y + boxCheck.size.y/2)),
                    new Node(new Vector2(boxCheck.offset.x - boxCheck.size.x/2,
                        boxCheck.offset.y - boxCheck.size.y/2)),
                    new Node(new Vector2(boxCheck.offset.x + boxCheck.size.x/2,
                        boxCheck.offset.y + boxCheck.size.y/2)),
                    new Node(new Vector2(boxCheck.offset.x + boxCheck.size.x/2,
                        boxCheck.offset.y - boxCheck.size.y/2))
                };
                foreach (var node in nodes) yield return node;
            }
            
        }

        //returns an IEnumerable containing every node of "Solid" objects
        //currently does not return the entity associated with the nodes, but
        //that can be added very easily if need-be
        public static IEnumerable<Node> GetSolidNodes() {
            return NodeHelper.EntityNodes.Where(item => item.Key.Solid).SelectMany(entity => entity.Value);
        }

        public static void Cleanup() {
            //clean up for scene change
        }
    }
}

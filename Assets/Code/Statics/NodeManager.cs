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
        public static Dictionary<Entity, List<Node>> EntityNodes;
        //this code kinda sucks... maybe refactor it?
        //1 - static classes blow - global state!
        //2 - repeating if's? redundant poly-casts? I know there's only 4 cases, but it's like... c'mon.
        
        private static NodeHelper _nodeHelper = (new GameObject("NodeHelper")).AddComponent<NodeHelper>();

        static NodeManager()  {
            EntityNodes = new Dictionary<Entity, List<Node>>();
        }
        public static void Init() {
            
        }
        public static IEnumerable<Node> GetNodes(Entity entity)
        {

            var circlecheck = entity.Collider as CircleCollider2D;
            var polygonCheck = entity.Collider as PolygonCollider2D;
            var boxCheck = entity.Collider as BoxCollider2D;
            if (circlecheck) {
                const float precision = 8f;
                const float radians = (2f*Mathf.PI)/precision;
                for (var i = 0; i < precision; i++) {
                    
                    var angle = radians*(i+1);
                    var xMag = Mathf.Round(Mathf.Cos(angle)*1000f)/1000f;
                    var yMag = Mathf.Round(Mathf.Sin(angle)*1000f)/1000f;
                    Debug.Log(string.Format("Angle: {0} .. Radius: {1} Cos(angle): {2}  Sin(angle): {3}",
                                            angle, circlecheck.radius, xMag, yMag));
                    yield return new Node(new Vector2(xMag,yMag));
                }
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

            return EntityNodes.Where(item => item.Key.Solid).SelectMany(entity => entity.Value);
        }

        public static void Cleanup() {
            //clean up for scene change
        }
    }
}

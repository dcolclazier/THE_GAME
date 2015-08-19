using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Statics
{
    //after refactor - MUCH BETTER! 
    //todo - I don't like that this is static - static is bad.
    //todo - add in capability to scale polygon colliders prior to node creation.
    public static class NodeManager
    {
        public enum ColliderType { Box, Circle, Polygon, Count}

        private static readonly Func<Entity, float, IEnumerable<Node>>[] NodeGrabber; 
        private static readonly List<Entity> Entities;
 
        //static constructor for NodeManager - creates a listener for "EntityCreated"
        //event, assigns delegate functions for Node Retrieval types
        static NodeManager()  {
            Messenger.AddListener<Entity>("EntityCreated", EntityCreated);
            Messenger.MarkAsPermanent("EntityCreated");
            
            Entities = new List<Entity>();
            NodeGrabber = new Func<Entity, float, IEnumerable<Node>>[(int)ColliderType.Count];
            
            NodeGrabber[(int)ColliderType.Circle] = GetCircleNodes;
            NodeGrabber[(int)ColliderType.Box] = GetBoxNodes;
            NodeGrabber[(int)ColliderType.Polygon] = GetPolygonNodes;
        }
        //Callback function for "EntityCreated" message broadcast
        private static void EntityCreated(Entity entity) {
            if (entity.Collider == null)
            {
                throw new NullReferenceException("Collider wasn't found - are you sure it was created prior to it's monobehaviour awake ran?");
            }
            if (Entities.Contains(entity))
            {
                throw new Exception("Duplicate entity detected - EntityCreated()");
            }
            Entities.Add(entity);
            Debug.Log(string.Format("NODEMANAGER: new Entity added: {0}, Solid? {1} Collider: {2}", entity.Collider.name, entity.Solid, entity.ColliderType));
          
        }

        //NodeGrabber method for expanding and retrieving nodes for BoxCollider2d
        private static IEnumerable<Node> GetBoxNodes(Entity entity, float expansionfactor) {
            var box = ((BoxCollider2D) entity.Collider);
            var buffer = 0.05f;
            box.Scale(expansionfactor);
            var nodes = new List<Node> {
                    new Node(new Vector2(box.offset.x - box.size.x*entity.transform.localScale.x/2 + entity.transform.position.x - buffer, //top left
                                         box.offset.y + box.size.y*entity.transform.localScale.y/2 + entity.transform.position.y + buffer)), 
                    new Node(new Vector2(box.offset.x - box.size.x*entity.transform.localScale.x/2 + entity.transform.position.x - buffer, //bottom left
                                         box.offset.y - box.size.y*entity.transform.localScale.y/2 + entity.transform.position.y - buffer)),
                    new Node(new Vector2(box.offset.x + box.size.x*entity.transform.localScale.x/2 + entity.transform.position.x + buffer, //top right
                                         box.offset.y + box.size.y*entity.transform.localScale.y/2 + entity.transform.position.y + buffer)),
                    new Node(new Vector2(box.offset.x + box.size.x*entity.transform.localScale.x/2 + entity.transform.position.x + buffer, //bottom right
                                         box.offset.y - box.size.y*entity.transform.localScale.y/2 + entity.transform.position.y - buffer))
                };
            return nodes;
        }

        //NodeGrabber method for expanding and retrieving nodes for PolygonCollider2d - NOT WORKING
        private static IEnumerable<Node> GetPolygonNodes(Entity entity, float expansionfactor) {
            if (entity.Collider == null) yield break;
            
            //var center = ((PolygonCollider2D) entity.Collider).GetCenter2D();
            foreach (var point in ((PolygonCollider2D)entity.Collider).points) {
                //var newX = (1.01f*(point.x - center.x) + center.x) + entity.transform.position.x;
                //var newY = (1.01f*(point.y - center.y) + center.y) + entity.transform.position.x;
                //yield return new Node(newX,newY);
                yield return new Node(new Vector2(point.x + entity.transform.position.x,
                                                    point.y + entity.transform.position.y));
            }
        }

        //NodeGrabber method for expanding and retrieving nodes for CircleCollider2d
        private static IEnumerable<Node> GetCircleNodes(Entity entity, float expansionFactor) {
            if (entity.Collider == null) yield break;

            const float precision = 6f;
            const float radians = (2f * Mathf.PI) / precision;
            var collider = ((CircleCollider2D) entity.Collider);
            
            collider.Scale(expansionFactor);
            for (var i = 0; i < precision; i++) {
                var angle = radians * (i + 1);
                var xMag = entity.gameObject.transform.position.x 
                            + Mathf.Round((collider.radius + .1f) 
                            * Mathf.Cos(angle) * 1000f) / 1000f;
                var yMag = entity.gameObject.transform.position.y 
                            + Mathf.Round((collider.radius + .1f) 
                            * Mathf.Sin(angle) * 1000f) / 1000f;
                yield return new Node(new Vector2(xMag, yMag));
            }
        }

        //responsible for picking the right NodeGrabber for the right type of collider.
        public static IEnumerable<Node> GetNodes(Entity entity, float expRadius) {
            
            return NodeGrabber[(int)entity.ColliderType](entity, expRadius);     
        }

        //Public facing function - retreives nodes for all colliders in a scene, automatically 
        //expanding them by a certain expansionFactor to facilitate size of moving entity.
        public static IEnumerable<Node> GetAllSolidNodes() {
            var nodelist = new List<Node>();

            foreach (var entity in Entities.Where(entity => entity.Solid)) {
                nodelist.AddRange(entity.CollisionNodes.Where(
                    e => {
                        var collider = Physics2D.OverlapPoint(e.Position, 1 << 10);
                        return collider == null || collider.gameObject == entity.Collider.gameObject;
                    }));
            }
            return nodelist;
        }

        //Clears all entries from Entities list
        public static void ClearEntities() {
            Entities.Clear();
            Debug.Log("NODEMANAGER Cleanup... All entity records cleared.");
        }
    }
}

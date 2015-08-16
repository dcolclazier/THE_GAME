using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code
{
    //after refactor - MUCH BETTER! 
    //todo - I don't like that this is static - static is bad.
    //todo - add in capability to scale polygon and box colliders prior to node creation.
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
            Debug.Log(string.Format("NODEMANAGER: new Entity added: {0}, Solid? {1} Collider: {2}", entity.Collider.name, entity.Solid, entity.colliderType));
          
        }

        //NodeGrabber method for expanding and retrieving nodes for BoxCollider2d
        private static IEnumerable<Node> GetBoxNodes(Entity entity, float expansionfactor) {
            var box = ((BoxCollider2D) entity.Collider);
            box.Scale(expansionfactor);
            var nodes = new List<Node> {
                    new Node(new Vector2(box.offset.x - box.size.x/2,
                        box.offset.y + box.size.y/2)),
                    new Node(new Vector2(box.offset.x - box.size.x/2,
                        box.offset.y - box.size.y/2)),
                    new Node(new Vector2(box.offset.x + box.size.x/2,
                        box.offset.y + box.size.y/2)),
                    new Node(new Vector2(box.offset.x + box.size.x/2,
                        box.offset.y - box.size.y/2))
                };
            return nodes;
        }

        //NodeGrabber method for expanding and retrieving nodes for PolygonCollider2d
        private static IEnumerable<Node> GetPolygonNodes(Entity entity, float expansionfactor) {
            if (entity.Collider == null) yield break;
            
            foreach (var point in ((PolygonCollider2D)entity.Collider).points) {
                yield return new Node(point);
            }
        }

        //NodeGrabber method for expanding and retrieving nodes for CircleCollider2d
        private static IEnumerable<Node> GetCircleNodes(Entity entity, float expansionFactor) {
            if (entity.Collider == null) yield break;

            const float precision = 8f;
            const float radians = (2f * Mathf.PI) / precision;
            var collider = ((CircleCollider2D) entity.Collider);
            
            collider.Scale(expansionFactor);
            for (var i = 0; i < precision; i++) {
                var angle = radians * (i + 1);
                var xMag = collider.offset.x + Mathf.Round(collider.radius * Mathf.Cos(angle) * 1000f) / 1000f;
                var yMag = collider.offset.y + Mathf.Round(collider.radius * Mathf.Sin(angle) * 1000f) / 1000f;
                yield return new Node(new Vector2(xMag, yMag));
            }
        }

        //responsible for picking the right NodeGrabber for the right type of collider.
        private static IEnumerable<Node> GetNodes(Entity entity, float expRadius) {
            
            return NodeGrabber[(int)entity.colliderType](entity, expRadius);     
        }

        //Public facing function - retreives nodes for all colliders in a scene, automatically 
        //expanding them by a certain expansionFactor to facilitate size of moving entity.
        public static IEnumerable<Node> GetAllSolidNodes(float expansionFactor) {
            var nodelist = new List<Node>();

            foreach (var entity in Entities.Where(entity => entity.Solid)) {
                nodelist.AddRange(GetNodes(entity, expansionFactor));
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.Abstract;
using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code
{
    //after refactor - MUCH BETTER! 
    //todo - I don't like that this is static - static is bad.
    public static class NodeManager
    {
      
        private delegate IEnumerable<Node> NodeRetreiveDelegate(Entity entity, float expansionFactor);
        public enum ColliderType { Box, Circle, Polygon, Count}
        private static NodeRetreiveDelegate[] _nodeRetreival;
        private static readonly List<Entity> Entities;
 
        //todo - add in capability to scale polygon and box colliders prior to node creation.

        static NodeManager()  {
            //Messenger.AddListener<Entity>("EntityCreated", EntityCreated);
            Entities = new List<Entity>();
        }
        public static void Init() {
            Messenger.AddListener<Entity>("EntityCreated", EntityCreated);
            _nodeRetreival = new NodeRetreiveDelegate[(int)ColliderType.Count];
            _nodeRetreival[(int) ColliderType.Circle] = GetCircleNodes;
            _nodeRetreival[(int) ColliderType.Box] = GetBoxNodes;
            _nodeRetreival[(int) ColliderType.Polygon] = GetPolygonNodes;
        }
        private static void EntityCreated(Entity entity)
        {
            if (entity.Collider == null)
            {
                throw new NullReferenceException("Collider wasn't found - are you sure it was created prior to it's monobehaviour awake ran?");
            }
            if (Entities.Contains(entity))
            {
                throw new Exception("Duplicate entity detected - EntityCreated()");
            }
            Entities.Add(entity);
            Debug.Log(string.Format("Node Helper: new Entity added: {0}, Solid? {1}", entity.Collider.name, entity.Solid));
          
        }
        private static IEnumerable<Node> GetBoxNodes(Entity entity, float expansionfactor) {
            var box = ((BoxCollider2D) entity.Collider);
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

        private static IEnumerable<Node> GetPolygonNodes(Entity entity, float expansionfactor) {
            if (entity.Collider == null) yield break;
            
            foreach (var point in ((PolygonCollider2D)entity.Collider).points) {
                yield return new Node(point);
            }
        }

        private static IEnumerable<Node> GetCircleNodes(Entity entity, float expansionFactor) {
            const float precision = 8f;
            const float radians = (2f * Mathf.PI) / precision;
            var circlerad = ((CircleCollider2D) entity.Collider).radius + expansionFactor;
            for (var i = 0; i < precision; i++)
            {

                var angle = radians * (i + 1);
                var xMag = Mathf.Round(circlerad * Mathf.Cos(angle) * 1000f) / 1000f;
                var yMag = Mathf.Round(circlerad * Mathf.Sin(angle) * 1000f) / 1000f;
                yield return new Node(new Vector2(xMag, yMag));
            }
        }

        private static IEnumerable<Node> GetNodes(Entity entity, float expRadius) {
            
            return _nodeRetreival[(int)entity.colliderType](entity, expRadius);     
        }
        public static IEnumerable<Node> GetAllSolidNodes(float expansionFactor) {
            var nodelist = new List<Node>();

            foreach (var entity in Entities.Where(entity => entity.Solid)) {
                nodelist.AddRange(GetNodes(entity, expansionFactor));
            }
            return nodelist;
        }

        public static void ClearEntities() {
            Entities.Clear();
            Debug.Log("NODEMANAGER Cleanup... All entitie records cleared.");
        }
    }
}

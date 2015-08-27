using System;
using System.Collections.Generic;
using Assets.Code.Entities;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Movement
{
    //todo - add in capability to scale polygon colliders prior to node creation.
    public class NodeManager
    {
        public enum ColliderType { Box, Circle, Polygon, Count}
        private readonly Func<Entity, IEnumerable<Node>>[] _nodeGrabber; 
 

        //Action
        //Func
        public NodeManager()  {
            
            _nodeGrabber = new Func<Entity, IEnumerable<Node>>[(int)ColliderType.Count];
            
            _nodeGrabber[(int)ColliderType.Circle] = GetCircleNodes;
            _nodeGrabber[(int)ColliderType.Box] = GetBoxNodes;
            _nodeGrabber[(int)ColliderType.Polygon] = GetPolygonNodes;
        }
        public static ColliderType GetColliderType(Collider2D baseCollider)
        {
            ColliderType colliderType;
            if (baseCollider is CircleCollider2D) colliderType = ColliderType.Circle;
            else if (baseCollider is PolygonCollider2D) colliderType = ColliderType.Polygon;
            else if (baseCollider is BoxCollider2D) colliderType = ColliderType.Box;
            else { throw new Exception("ENTITY: Could not determine collider type. "); }
            return colliderType;
        }
       
        private static IEnumerable<Node> GetBoxNodes(Entity entity) {
            var box = (entity.Attributes.Get<Collider2D>("ObstructCollider") as BoxCollider2D) ;
            var gameObject = entity.Attributes.Get<GameObject>("GameObject");
            var scale = new Vector2(1,1);
            var position = gameObject.transform.position;
            var buffer = 0.05f;
            var nodes = new List<Node> {
                    new Node(new Vector2(box.offset.x - box.size.x*scale.x/2 + position.x - buffer, //top left
                                         box.offset.y + box.size.y*scale.y/2 + position.y + buffer),entity), 
                    new Node(new Vector2(box.offset.x - box.size.x*scale.x/2 + position.x - buffer, //bottom left
                                         box.offset.y - box.size.y*scale.y/2 + position.y - buffer),entity),
                    new Node(new Vector2(box.offset.x + box.size.x*scale.x/2 + position.x + buffer, //top right
                                         box.offset.y + box.size.y*scale.y/2 + position.y + buffer),entity),
                    new Node(new Vector2(box.offset.x + box.size.x*scale.x/2 + position.x + buffer, //bottom right
                                         box.offset.y - box.size.y*scale.y/2 + position.y - buffer),entity)
                };
            return nodes;
        }

        private static IEnumerable<Node> GetPolygonNodes(Entity entity) {
            throw new NotImplementedException();
        }

        private static IEnumerable<Node> GetCircleNodes(Entity entity) {
            //if (entity.Collider == null) yield break;

            const float precision = 6f;
            const float radians = (2f * Mathf.PI) / precision;
            var circle = ((CircleCollider2D)entity.Attributes.Get<Collider2D>("ObstructCollider"));
            var centerX = circle.transform.position.x;
            var centerY = circle.transform.position.y;
            for (var i = 0; i < precision; i++) {
                var angle = radians * (i + 1);
                var xMag = centerX + Mathf.Round((circle.radius + .1f) 
                            * Mathf.Cos(angle) * 1000f) / 1000f;
                var yMag = centerY + Mathf.Round((circle.radius + .1f) 
                            * Mathf.Sin(angle) * 1000f) / 1000f;
                yield return new Node(new Vector2(xMag, yMag),entity);
            }
        }

        public IEnumerable<Node> GetNodes(Entity entity) {
            
            return _nodeGrabber[(int)entity.Attributes.Get<ColliderType>("ObstructColliderType")](entity);     
        }

        public static IEnumerable<Node> GetAllSolidNodes() {
            var nodelist = new List<Node>();
            foreach (var entity in EntityManager.MasterEntityList) {
                if (entity.Attributes.Get<bool>("CurrentlyObstructing")) {
                    foreach (var node in entity.Attributes.Get<List<Node>>("CollisionNodes")) {
                        var collider = Physics2D.OverlapPoint(node.Position, 1 << 11);
                        if (collider == null || collider.gameObject == entity.Attributes.Get<GameObject>("GameObject"))
                            nodelist.Add(node);
                    }
                }
            }
            return nodelist;
//LINQ -->  //foreach (var entity in EntityManager.MasterEntityList.Where(entity => entity.Attributes.Get<bool>("CurrentlyObstructing"))) {
            //    nodelist.AddRange(entity.Attributes.Get<List<Node>>("CollisionNodes").Where(
            //        node => {
            //            var collider = Physics2D.OverlapPoint(node.Position, 1 << 11);
            //            return collider == null || collider.gameObject == entity.Attributes.Get<GameObject>("GameObject");
            //        }));
            //}
            //return nodelist;
        }

        public static void ClearNodes() {
            foreach (var entity in EntityManager.MasterEntityList) {
                foreach (var node in entity.Attributes.Get<List<Node>>("CollisionNodes")) {
                    node.GuessH = 0;
                    node.CameFrom = null;
                    node.TotalScoreF = 0;
                    node.PathDistanceG = 0;
                }
            }
//LINQ -->  //foreach (var node in EntityManager.MasterEntityList.SelectMany(entity => entity.Attributes.Get<List<Node>>("CollisionNodes"))) {
            //    node.GuessH = 0;
            //    node.CameFrom = null;
            //    node.TotalScoreF = 0;
            //    node.PathDistanceG = 0;
            //}
        }
    }
}

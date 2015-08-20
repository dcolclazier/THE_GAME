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
    public class NodeManager
    {
        public enum ColliderType { Box, Circle, Polygon, Count}

        

        private readonly Func<Entity, IEnumerable<Node>>[] _nodeGrabber; 
        //private readonly List<entity> Entities;
 
        //static constructor for NodeManager - creates a listener for "EntityCreated"
        //event, assigns delegate functions for Node Retrieval types
        public NodeManager()  {
            //Messenger.AddListener<entity>("EntityCreated", EntityCreated);
            //Messenger.MarkAsPermanent("EntityCreated");
            
            //Entities = new List<entity>();
            _nodeGrabber = new Func<Entity, IEnumerable<Node>>[(int)ColliderType.Count];
            
            _nodeGrabber[(int)ColliderType.Circle] = GetCircleNodes;
            _nodeGrabber[(int)ColliderType.Box] = GetBoxNodes;
            _nodeGrabber[(int)ColliderType.Polygon] = GetPolygonNodes;
        }
       
        //NodeGrabber method for expanding and retrieving nodes for BoxCollider2d
        private IEnumerable<Node> GetBoxNodes(Entity entity) {
            var box = (entity.Attributes.Get<Collider2D>("ObstructCollider") as BoxCollider2D) ;
            Debug.Log("Box is " + (box == null ? "null" : "not null"));
            var gameObject = entity.Attributes.Get<GameObject>("GameObject");
            Debug.Log("gameObject is " + (gameObject == null ? "null" : "not null"));
            var scale = gameObject.transform.localScale;
            var position = box.transform.position;
            var buffer = 0.05f;
            //box.Scale(expansionfactor);
            var nodes = new List<Node> {
                    new Node(new Vector2(box.offset.x - box.size.x*scale.x/2 + position.x - buffer, //top left
                                         box.offset.y + box.size.y*scale.y/2 + position.y + buffer)), 
                    new Node(new Vector2(box.offset.x - box.size.x*scale.x/2 + position.x - buffer, //bottom left
                                         box.offset.y - box.size.y*scale.y/2 + position.y - buffer)),
                    new Node(new Vector2(box.offset.x + box.size.x*scale.x/2 + position.x + buffer, //top right
                                         box.offset.y + box.size.y*scale.y/2 + position.y + buffer)),
                    new Node(new Vector2(box.offset.x + box.size.x*scale.x/2 + position.x + buffer, //bottom right
                                         box.offset.y - box.size.y*scale.y/2 + position.y - buffer))
                };
            return nodes;
            return null;
        }

        //NodeGrabber method for expanding and retrieving nodes for PolygonCollider2d - NOT WORKING
        private IEnumerable<Node> GetPolygonNodes(Entity entity) {
            //if (entity.Collider == null) yield break;
            
            //var center = ((PolygonCollider2D) entity.Collider).GetCenter2D();
            //foreach (var point in ((PolygonCollider2D)entity.Collider).points) {
                //var newX = (1.01f*(point.x - center.x) + center.x) + entity.transform.position.x;
                //var newY = (1.01f*(point.y - center.y) + center.y) + entity.transform.position.x;
                //yield return new Node(newX,newY);
                //yield return new Node(new Vector2(point.x + entity.transform.position.x,
                 //                                   point.y + entity.transform.position.y));
            //}
            throw new NotImplementedException();
        }

        //NodeGrabber method for expanding and retrieving nodes for CircleCollider2d
        private IEnumerable<Node> GetCircleNodes(Entity entity) {
            //if (entity.Collider == null) yield break;

            const float precision = 6f;
            const float radians = (2f * Mathf.PI) / precision;
            var circle = ((CircleCollider2D)entity.Attributes.Get<Collider2D>("ObstructCollider"));
            var centerX = circle.transform.position.x;
            var centerY = circle.transform.position.y;
             //circle.Scale(expansionFactor);
            for (var i = 0; i < precision; i++) {
                var angle = radians * (i + 1);
                var xMag = centerX + Mathf.Round((circle.radius + .1f) 
                            * Mathf.Cos(angle) * 1000f) / 1000f;
                var yMag = centerY + Mathf.Round((circle.radius + .1f) 
                            * Mathf.Sin(angle) * 1000f) / 1000f;
                yield return new Node(new Vector2(xMag, yMag));
            }
        }

        //responsible for picking the right NodeGrabber for the right type of collider.
        public IEnumerable<Node> GetNodes(Entity entity) {
            
            return _nodeGrabber[(int)entity.Attributes.Get<ColliderType>("ObstructColliderType")](entity);     
        }

        //Public facing function - retreives nodes for all colliders in a scene, automatically 
        //expanding them by a certain expansionFactor to facilitate size of moving entity.
        public IEnumerable<Node> GetAllSolidNodes() {
            var nodelist = new List<Node>();

            foreach (var entity in EntityManager.MasterEntityList.Where(entity => entity.Attributes.Get<bool>("CurrentlyObstructing"))) {
                
                nodelist.AddRange(entity.Attributes.Get<List<Node>>("CollisionNodes").Where(
                    e => {
                        var collider = Physics2D.OverlapPoint(e.Position, 1 << 11);
                        return collider == null || collider.gameObject == entity.Attributes.Get<GameObject>("GameObject");
                    }));
            }
            return nodelist;
        }

        //Clears all entries from Entities list
        public void ClearEntities() {
            //Entities.Clear();
            Debug.Log("NODEMANAGER Cleanup... All entity records cleared.");
        }
    }
}

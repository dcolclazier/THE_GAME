using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class ObstructableComponent : IComponent, IToggle {
        public Collider2D Collider { get; set; }
        public NodeManager.ColliderType ColliderType { get; private set; }
        public List<Node> CollisionNodes { get; private set; }
        public Entity Parent { get; private set; }
        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                    "ObstructCollider"
                };
            }
        }

        public bool Enabled {
            get { return _solid; }
            set {
                var changed = value != _solid;
                _solid = value;
                if (changed) {
                    UpdateListeners();
                }
            } 
        }
        private bool _solid;

        private void UpdateListeners() {
            Parent.Attributes.Update("CurrentlyObstructing",_solid);
            Messenger.Broadcast(Enabled ? "ObstructionAdded" : "ObstructionRemoved", Parent);
        }

        public ObstructableComponent(Entity entity) {
            Parent = entity;
            Enabled = true;
            CollisionNodes = new List<Node>(EntityManager.NodeMgr.GetNodes(entity));
            Parent.Attributes.Register("CollisionNodes", CollisionNodes);

            Collider = Parent.Attributes.Get<Collider2D>("ObstructCollider");
            
            if (Collider is CircleCollider2D) ColliderType = NodeManager.ColliderType.Circle;
            else if (Collider is PolygonCollider2D) ColliderType = NodeManager.ColliderType.Polygon;
            else if (Collider is BoxCollider2D) ColliderType = NodeManager.ColliderType.Box;
            else { throw new Exception("ENTITY: Could not determine collider type. "); }

            Parent.Attributes.Register("ObstructColliderType", ColliderType);
            Parent.Attributes.Register("CurrentlyObstructing", _solid);
            //Collider = Parent.Components.Get<GameObject>("GameObject").AddComponent<Collider2D>();
            //Parent.Attributes.Register("Collider", Collider);

        }

        public void OnAwake() {
            

        }

        public void OnStart() {
            //Messenger.Broadcast("EntityCreated", Parent);
        }

        

        public void OnUpdate() {
            
        }
        public void OnMessage() {
            throw new NotImplementedException();
        }
    }
}
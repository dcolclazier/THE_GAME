using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract {
    public class ObstructableComponent : IComponent, IToggle {
        private Collider2D _obstructCollider;
        private NodeManager.ColliderType _colliderType;
        private List<Node> _collisionNodes;
        public Entity Parent { get; set; }
        public List<string> Dependencies {
            get {
                return new List<string>() {
                    "GameObject",
                };
            }
        }

        public bool Enabled {
            get { return _solid; }
            private set {
                var changed = value != _solid;
                _solid = value;
                if (changed) {
                    UpdateListeners();
                }
            } 
        }
        private bool _solid = true;

        private void UpdateListeners() {
            Parent.Attributes.Update("CurrentlyObstructing",_solid);
            Messenger.Broadcast(Enabled ? "ObstructionAdded" : "ObstructionRemoved", Parent);
        }

        public void OnAwake() {
            

        }

        public void OnStart() {

        }

        public void Init() {
            //get the obstruct collider from the game object and register it as an attribute for the entity.
            _obstructCollider = Parent.Attributes.Get<GameObject>("GameObject").GetComponent<Collider2D>();
            if (_obstructCollider == null)
                throw new Exception(
                    "Trying to init an Obstructable Component, but init couldn't find the collider. " +
                    "Make sure it is attached to the game object itself, not the child collider object. ");
            Parent.Attributes.Register("ObstructCollider", (Collider2D) _obstructCollider);

            //get and assign the colliderType attribute for the entity.
            if (_obstructCollider is CircleCollider2D) _colliderType = NodeManager.ColliderType.Circle;
            else if (_obstructCollider is PolygonCollider2D) _colliderType = NodeManager.ColliderType.Polygon;
            else if (_obstructCollider is BoxCollider2D) _colliderType = NodeManager.ColliderType.Box;
            else { throw new Exception("ENTITY: Could not determine collider type. "); }
            Parent.Attributes.Register("ObstructColliderType", _colliderType);

            Parent.Attributes.Register("CurrentlyObstructing", _solid);
            Enabled = true;
            _collisionNodes = new List<Node>(EntityManager.GetNodesForEntity(Parent));
            Parent.Attributes.Register("CollisionNodes", _collisionNodes);

            

            
        }


        public void OnUpdate() {
            
        }
        public void OnMessage() {
            throw new NotImplementedException();
        }
    }
}
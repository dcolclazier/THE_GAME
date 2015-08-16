using System;
using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity, IObstructable {
        
        private bool _currentlySolid;
        public NodeManager.ColliderType colliderType;

        protected virtual void Start() {
            Messenger.Broadcast("EntityCreated", this);
            if (Collider is CircleCollider2D) colliderType = NodeManager.ColliderType.Circle;
            else if (Collider is BoxCollider2D) colliderType = NodeManager.ColliderType.Box;
            else if (Collider is PolygonCollider2D) colliderType = NodeManager.ColliderType.Polygon;
        }
        public bool Solid {
            get {
                return _currentlySolid;
            }
            protected set {
                var changed = value != _currentlySolid;
                _currentlySolid = value;
                if(changed) UpdateNodes();
            }
        }

        public Collider2D Collider {
            get {
                
                return GetComponent<Collider2D>();
            }
        }

        public virtual void Phase() {
            Solid = !Solid;
        }

        protected void UpdateNodes() {
            Messenger.Broadcast(Solid ? "EntityAppeared" : "EntityDisappeared", this);
        }
    }


    public static class Extensions {

        static void Expand(this CircleCollider2D collider, float expRadius) {
            collider.radius += expRadius;
        }
    }
}

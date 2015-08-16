using System;
using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity, IObstructable {
        
        private bool _currentlySolid;
        public NodeManager.ColliderType colliderType;

        protected void Awake() {
            if (Collider is CircleCollider2D) colliderType = NodeManager.ColliderType.Circle;
            else if (Collider is PolygonCollider2D) colliderType = NodeManager.ColliderType.Polygon;
            else if (Collider is BoxCollider2D) colliderType = NodeManager.ColliderType.Box;
            else { throw new Exception("ENTITY: Could not determine collider type. ");}
            _currentlySolid = true;
        }
        protected virtual void Start() {
            Messenger.Broadcast("EntityCreated", this);
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

        public Collider2D Collider { get { return GetComponent<Collider2D>(); } }

        public virtual void Phase() {
            Solid = !Solid;
        }

        private void UpdateNodes() {
            Messenger.Broadcast(Solid ? "EntityAppeared" : "EntityDisappeared", this);
        }
    }
}

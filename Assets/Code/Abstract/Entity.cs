using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract
{
    public abstract class Entity : MonoBehaviour, IEntity, IObstructable {
        
        private bool _currentlySolid = true;

        public NodeManager.ColliderType ColliderType { get; private set; }
        public List<Node> CollisionNodes { get; private set; }

        protected void Awake() {
            if (Collider is CircleCollider2D) ColliderType = NodeManager.ColliderType.Circle;
            else if (Collider is PolygonCollider2D) ColliderType = NodeManager.ColliderType.Polygon;
            else if (Collider is BoxCollider2D) ColliderType = NodeManager.ColliderType.Box;
            else { throw new Exception("ENTITY: Could not determine collider type. ");}
            CollisionNodes = new List<Node>(NodeManager.GetNodes(this, 0f));

            //bug this hides the trait - it should be dropped to subclass level after kyle knows how it works
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

        protected virtual void Update() {
            
        }
    }
}

using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity, IObstructable {
        
        private bool _currentlySolid;
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
        public Collider2D Collider { get { return GetComponent<PolygonCollider2D>(); } }
        public virtual void Phase() {
            Solid = !Solid;
        }

        protected void UpdateNodes() {
            Messenger.Broadcast(Solid ? "EntityAppeared" : "EntityDisappeared", this);
        }
    }
}

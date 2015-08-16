using Assets.Code.Abstract;
using UnityEngine;

namespace Assets.Code.Entities
{
    public class Entity : MonoBehaviour, IEntity, IObstructable {
        
        private bool _currentlySolid;
        protected virtual void Awake() {
            
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
        protected void UpdateNodes() {
            Messenger.Broadcast(Solid ? "EntityAppeared" : "EntityDisappeared", (IObstructable) this);
        }
    }

 

   
    
}
